using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.WebHost.Controllers;
using PromoCodeFactory.WebHost.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PromoCodeFactory.UnitTests.WebHost.Controllers.Partners
{
    public class SetPartnerPromoCodeLimitAsyncTests
    {
        private readonly IFixture _fixture;
        private readonly PartnerTestDataFactory _dataFactory;
        public SetPartnerPromoCodeLimitAsyncTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _dataFactory = new PartnerTestDataFactory();
        }
        /// <summary>
        /// Если партнер не найден, то также нужно выдать ошибку 404;
        /// </summary>
        [Fact]
        public async Task SetPartnerPromoCodeLimitAsync_PartnerNotFound_Returns404()
        {
            //Arrange
            var partnerId = _fixture.Create<Guid>();
            var partnerRepositoryMock = new Mock<IRepository<Partner>>();
            PartnersController partnersController = new PartnersController(partnerRepositoryMock.Object);

            // Act
            var result = await partnersController.SetPartnerPromoCodeLimitAsync(partnerId, _dataFactory.CreateLimitRequest(100));

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }
        /// <summary>
        /// Если партнер заблокирован, то есть поле IsActive=false в классе Partner, то также нужно выдать ошибку 400;
        /// </summary>
        [Fact]
        public async Task SetPartnerPromoCodeLimitAsync_PartnerIsBlocked_Returns400()
        {
            //Arrange
            var partnerRepositoryMock = new Mock<IRepository<Partner>>();
            var partnerId = _fixture.Create<Guid>();
            var blockedPartner = _dataFactory.CreatePartner(id: partnerId,isActive: false);

            partnerRepositoryMock
                .Setup(x => x.GetByIdAsync(partnerId))
                .ReturnsAsync(blockedPartner);
            PartnersController partnersController = new PartnersController(partnerRepositoryMock.Object);

            // Act
            var result = await partnersController.SetPartnerPromoCodeLimitAsync(partnerId, new SetPartnerPromoCodeLimitRequest());

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.Equal("Данный партнер не активен", badRequestResult.Value);
        }
        /// <summary>
        /// Если партнеру выставляется лимит, то мы должны обнулить количество промокодов, которые партнер выдал NumberIssuedPromoCodes , если лимит закончился, то количество не обнуляется;
        /// </summary>
        [Fact]
        public async Task SetPartnerPromoCodeLimitAsync_ShouldResetNumberIssuedPromoCodes_WhenNewLimitIsSet()
        {
            //Arrange
            var partnerId = _fixture.Create<Guid>();
            var partnerRepositoryMock = new Mock<IRepository<Partner>>();

            // Текущий лимит 
            var currentLimit = new PartnerPromoCodeLimit
            {
                Limit = 10,
                EndDate = DateTime.UtcNow
            };

            var partner = new Partner
            {
                Id = partnerId,
                IsActive = true,
                NumberIssuedPromoCodes = 50, // Партнер выдал 15 промокодов
                PartnerLimits = new List<PartnerPromoCodeLimit> { currentLimit }
            };

            // Новый лимит (активный)
            var newLimitRequest = new SetPartnerPromoCodeLimitRequest
            {
                Limit = 100,
                EndDate = DateTime.UtcNow.AddHours(2) // Новый лимит активен 2 часа
            };

            partnerRepositoryMock
                .Setup(x => x.GetByIdAsync(partnerId))
                .ReturnsAsync(partner);

            Partner updatedPartner = null;
            partnerRepositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<Partner>()))
                .Callback<Partner>(p => updatedPartner = p);

            var controller = new PartnersController(partnerRepositoryMock.Object);

            // Act
            var result = await controller.SetPartnerPromoCodeLimitAsync(partnerId, newLimitRequest);

            // Assert
            Assert.IsType<CreatedAtActionResult>(result); // Проверяем успешный ответ
            Assert.NotNull(updatedPartner); // Проверяем, что партнер был обновлен
            Assert.Equal(0, updatedPartner.NumberIssuedPromoCodes); // Должно обнулиться, т.к. новый лимит активен
        }
        //
        //Если лимит истек, то NumberIssuedPromoCodes не обнуляется.
        //
        [Fact]
        public async Task SetPartnerPromoCodeLimitAsync_ShouldNotResetNumberIssuedPromoCodes_WhenLimitExpired()
        {
            // Arrange
            var partnerId = _fixture.Create<Guid>();
            var partnerRepositoryMock = new Mock<IRepository<Partner>>();

            var partner = new Partner
            {
                Id = partnerId,
                IsActive = true,
                NumberIssuedPromoCodes = 10, // Партнер выдал 10 промокодов
                PartnerLimits = new List<PartnerPromoCodeLimit>()
            };

            // Новый лимит 
            var newLimitRequest = new SetPartnerPromoCodeLimitRequest
            {
                Limit = 1,
                EndDate = DateTime.UtcNow.AddHours(2) // Новый лимит
            };

            partnerRepositoryMock
                .Setup(x => x.GetByIdAsync(partnerId))
                .ReturnsAsync(partner);

            Partner updatedPartner = null;
            partnerRepositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<Partner>()))
                .Callback<Partner>(p => updatedPartner = p);

            var controller = new PartnersController(partnerRepositoryMock.Object);

            // Act
            var result = await controller.SetPartnerPromoCodeLimitAsync(partnerId, newLimitRequest);

            // Assert
            Assert.IsType<CreatedAtActionResult>(result);                   // Проверяем успешный ответ
            Assert.NotNull(updatedPartner);                                 // Проверяем, что партнер был обновлен
            Assert.Equal(10, updatedPartner.NumberIssuedPromoCodes);        // Не должно обнулиться
        }
        /// <summary>
        /// При установке лимита нужно отключить предыдущий лимит
        /// </summary>
        [Fact]
        public async Task SetPartnerPromoCodeLimitAsync_Partner_NewLimit_ResetsOldLimit()
        {
            //Arrange
            var partnerId = _fixture.Create<Guid>();
            var partnerRepositoryMock = new Mock<IRepository<Partner>>();
            // Текущий активный лимит (без CancelDate)
            var currentActiveLimit = new PartnerPromoCodeLimit
            {
                Id = Guid.NewGuid(),
                Limit = 20,
                CreateDate = DateTime.UtcNow.AddDays(-1),
                EndDate = DateTime.UtcNow.AddDays(1),
                CancelDate = null // Активный лимит
            };
            var partner = new Partner
            {
                Id = partnerId,
                IsActive = true,
                NumberIssuedPromoCodes = 15, // Партнер уже выдал 15 промокодов
                PartnerLimits = new List<PartnerPromoCodeLimit> { currentActiveLimit }
            };

            // Новый лимит
            var newLimitRequest = new SetPartnerPromoCodeLimitRequest
            {
                Limit = 100,
                EndDate = DateTime.UtcNow.AddHours(3)
            };

            // Настройка моков
            partnerRepositoryMock
                .Setup(x => x.GetByIdAsync(partnerId))
                .ReturnsAsync(partner);

            Partner updatedPartner = null;
            partnerRepositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<Partner>()))
                .Callback<Partner>(p => updatedPartner = p);

            var controller = new PartnersController(partnerRepositoryMock.Object);


            // Act
            var result = await controller.SetPartnerPromoCodeLimitAsync(partnerId, newLimitRequest);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(PartnersController.GetPartnerLimitAsync), createdAtActionResult.ActionName);

            //Проверяем, что старый лимит отключен (CancelDate установлен)
            var deactivatedLimit = updatedPartner.PartnerLimits.First(l => l.Id == currentActiveLimit.Id);
            Assert.NotNull(deactivatedLimit.CancelDate);
            Assert.True(deactivatedLimit.CancelDate <= DateTime.UtcNow);
        }
        /// <summary>
        ///Лимит должен быть больше 0;
        /// </summary>
        [Theory]
        [InlineData(0)]    // Лимит = 0
        [InlineData(-1)]   // Лимит отрицательный
        [InlineData(-999)] // Большое отрицательное число
        public async Task SetPartnerPromoCodeLimitAsync_Partner_CheckLimitMoreThenZero(int invalidLimit)
        {
            //Arrange
            var partnerId = _fixture.Create<Guid>();
            var partnerRepositoryMock = new Mock<IRepository<Partner>>();

            var validPartner = new Partner
            {
                Id = partnerId,
                IsActive = true,
                PartnerLimits = new List<PartnerPromoCodeLimit>()
            };

            partnerRepositoryMock
                .Setup(x => x.GetByIdAsync(partnerId))
                .ReturnsAsync(validPartner);

            var controller = new PartnersController(partnerRepositoryMock.Object);

            var invalidRequest = new SetPartnerPromoCodeLimitRequest
            {
                Limit = invalidLimit, // Некорректный лимит
                EndDate = DateTime.UtcNow.AddHours(1)
            };

            // Act
            var result = await controller.SetPartnerPromoCodeLimitAsync(partnerId, invalidRequest);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Лимит должен быть больше 0", badRequestResult.Value);
        }
        /// <summary>
        ///Нужно убедиться, что сохранили новый лимит в базу данных (это нужно проверить Unit-тестом);
        /// </summary>
        [Fact]
        public async Task SetPartnerPromoCodeLimitAsync_Partner_CheckLimitSave()
        {
            //Arrange
            var partnerId = _fixture.Create<Guid>();
            var partnerRepositoryMock = new Mock<IRepository<Partner>>();
            // Партнер без активных лимитов
            var partner = new Partner
            {
                Id = partnerId,
                IsActive = true,
                PartnerLimits = new List<PartnerPromoCodeLimit>()
            };

            partnerRepositoryMock
                .Setup(x => x.GetByIdAsync(partnerId))
                .ReturnsAsync(partner);

            Partner savedPartner = null;
            partnerRepositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<Partner>()))
                .Callback<Partner>(p => savedPartner = p) // Захватываем сохраненного партнера
                .Returns(Task.CompletedTask);

            var controller = new PartnersController(partnerRepositoryMock.Object);

            var validRequest = new SetPartnerPromoCodeLimitRequest
            {
                Limit = 100,
                EndDate = DateTime.UtcNow.AddHours(3)
            };


            // Act
            var result = await controller.SetPartnerPromoCodeLimitAsync(partnerId, validRequest);

            // Assert
            // 1. Проверяем, что метод завершился успешно
            Assert.IsType<CreatedAtActionResult>(result);

            // 2. Проверяем, что UpdateAsync был вызван
            partnerRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Partner>()), Times.Once);

            // 3. Проверяем, что новый лимит добавлен в партнера перед сохранением
            Assert.NotNull(savedPartner);
            var savedLimit = Assert.Single(savedPartner.PartnerLimits); // Должен быть ровно один лимит
            Assert.Equal(validRequest.Limit, savedLimit.Limit);
            Assert.Equal(validRequest.EndDate, savedLimit.EndDate);
            Assert.Equal(partnerId, savedLimit.PartnerId);
            Assert.NotNull(savedLimit.CreateDate);

        }

    }
    // Фабричный класс для создания тестовых данных
    public class PartnerTestDataFactory
    {
        public Partner CreatePartner(
            Guid? id = null,
            bool isActive = true,
            int numberIssuedPromoCodes = 0,
            List<PartnerPromoCodeLimit> limits = null)
        {
            return new Partner
            {
                Id = id ?? Guid.NewGuid(),
                IsActive = isActive,
                NumberIssuedPromoCodes = numberIssuedPromoCodes,
                PartnerLimits = limits ?? new List<PartnerPromoCodeLimit>()
            };
        }

        public PartnerPromoCodeLimit CreateLimit(
            int limit,
            DateTime? endDate = null,
            DateTime? cancelDate = null)
        {
            return new PartnerPromoCodeLimit
            {
                Id = Guid.NewGuid(),
                Limit = limit,
                CreateDate = DateTime.UtcNow,
                EndDate = endDate ?? DateTime.UtcNow.AddDays(1),
                CancelDate = cancelDate
            };
        }

        public SetPartnerPromoCodeLimitRequest CreateLimitRequest(
            int limit,
            DateTime? endDate = null)
        {
            return new SetPartnerPromoCodeLimitRequest
            {
                Limit = limit,
                EndDate = endDate ?? DateTime.UtcNow.AddDays(1)
            };
        }
    }
}