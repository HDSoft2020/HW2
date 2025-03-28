using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PromoCodeFactory.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "T_Customers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "T_Preferences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Preferences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "T_Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerPreference",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CustomerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PreferenceId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerPreference", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerPreference_T_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "T_Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerPreference_T_Preferences_PreferenceId",
                        column: x => x.PreferenceId,
                        principalTable: "T_Preferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_Employees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    RoleId = table.Column<Guid>(type: "TEXT", nullable: true),
                    AppliedPromocodesCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_T_Employees_T_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "T_Roles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "T_PromoCodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Code = table.Column<string>(type: "TEXT", nullable: true),
                    ServiceInfo = table.Column<string>(type: "TEXT", nullable: true),
                    BeginDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PartnerName = table.Column<string>(type: "TEXT", nullable: true),
                    PartnerManagerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PreferenceId = table.Column<string>(type: "TEXT", nullable: true),
                    PreferenceId1 = table.Column<Guid>(type: "TEXT", nullable: true),
                    CustomerId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_PromoCodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_T_PromoCodes_T_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "T_Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_PromoCodes_T_Employees_PartnerManagerId",
                        column: x => x.PartnerManagerId,
                        principalTable: "T_Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_PromoCodes_T_Preferences_PreferenceId1",
                        column: x => x.PreferenceId1,
                        principalTable: "T_Preferences",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPreference_CustomerId",
                table: "CustomerPreference",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerPreference_PreferenceId",
                table: "CustomerPreference",
                column: "PreferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_T_Employees_RoleId",
                table: "T_Employees",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_T_PromoCodes_CustomerId",
                table: "T_PromoCodes",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_T_PromoCodes_PartnerManagerId",
                table: "T_PromoCodes",
                column: "PartnerManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_T_PromoCodes_PreferenceId1",
                table: "T_PromoCodes",
                column: "PreferenceId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerPreference");

            migrationBuilder.DropTable(
                name: "T_PromoCodes");

            migrationBuilder.DropTable(
                name: "T_Customers");

            migrationBuilder.DropTable(
                name: "T_Employees");

            migrationBuilder.DropTable(
                name: "T_Preferences");

            migrationBuilder.DropTable(
                name: "T_Roles");
        }
    }
}
