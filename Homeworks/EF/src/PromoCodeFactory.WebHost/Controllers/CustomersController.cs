using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.WebHost.Models;
using System;
using System.Threading.Tasks;

namespace PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Клиенты
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CustomersController: ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        public CustomersController(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper=mapper;
        }
        /// <summary>
        /// Получить список всех клиентов
        /// </summary>

        [HttpGet]
        public async Task<ActionResult<CustomerShortResponse>> GetCustomersAsync()
        {
            try
            {
                var customers = await _customerRepository.GetCustomerListAsync();
                return Ok(customers); 
            }
            catch (Exception exp)
            {
                return BadRequest();
            }
        }
        /// <summary>
        /// Получить клиента по идентификатору
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerResponse>> GetCustomerAsync(Guid id)
        {
            try
            {
                var customer = await _customerRepository.GetCustomerAsync(id);
                return Ok(customer);
            }
            catch (Exception exp)
            {
                return BadRequest();
            }
        }
        /// <summary>
        /// Добавить клиента
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateCustomerAsync(CreateOrEditCustomerRequest request)
        {
            //TODO: Добавить создание нового клиента вместе с его предпочтениями
            try
            {
                Customer customer = _mapper.Map<Customer>(request);
                await _customerRepository.AddCustomerAsync(customer);
                return Ok();
            }
            catch (Exception exp)
            {
                return BadRequest();
            }
        }
        /// <summary>
        /// Изменить клиента
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> EditCustomersAsync(Guid id, CreateOrEditCustomerRequest request)
        {
            //TODO: Обновить данные клиента вместе с его предпочтениями
            try
            {
                Customer customer = _mapper.Map<Customer>(request);
                await _customerRepository.UpateCustomerAsync(customer);
                return Ok();
            }
            catch (Exception exp)
            {
                return BadRequest();
            }
        }
        /// <summary>
        /// Удалить клиента
        /// </summary>
        [HttpDelete]
        public async Task<IActionResult> DeleteCustomerAsync(Guid id)
        {
            try
            {
                await _customerRepository.DeleteCustomerAsync(id);
                return Ok();
            }
            catch (Exception exp)
            {
                return BadRequest();
            }
        }
    }
}