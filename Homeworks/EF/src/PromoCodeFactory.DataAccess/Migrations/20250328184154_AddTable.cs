using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PromoCodeFactory.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerPreference_T_Customers_CustomerId",
                table: "CustomerPreference");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerPreference_T_Preferences_PreferenceId",
                table: "CustomerPreference");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerPreference",
                table: "CustomerPreference");

            migrationBuilder.RenameTable(
                name: "CustomerPreference",
                newName: "T_Customer_Preferences");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerPreference_PreferenceId",
                table: "T_Customer_Preferences",
                newName: "IX_T_Customer_Preferences_PreferenceId");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerPreference_CustomerId",
                table: "T_Customer_Preferences",
                newName: "IX_T_Customer_Preferences_CustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_T_Customer_Preferences",
                table: "T_Customer_Preferences",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_T_Customer_Preferences_T_Customers_CustomerId",
                table: "T_Customer_Preferences",
                column: "CustomerId",
                principalTable: "T_Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_T_Customer_Preferences_T_Preferences_PreferenceId",
                table: "T_Customer_Preferences",
                column: "PreferenceId",
                principalTable: "T_Preferences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_Customer_Preferences_T_Customers_CustomerId",
                table: "T_Customer_Preferences");

            migrationBuilder.DropForeignKey(
                name: "FK_T_Customer_Preferences_T_Preferences_PreferenceId",
                table: "T_Customer_Preferences");

            migrationBuilder.DropPrimaryKey(
                name: "PK_T_Customer_Preferences",
                table: "T_Customer_Preferences");

            migrationBuilder.RenameTable(
                name: "T_Customer_Preferences",
                newName: "CustomerPreference");

            migrationBuilder.RenameIndex(
                name: "IX_T_Customer_Preferences_PreferenceId",
                table: "CustomerPreference",
                newName: "IX_CustomerPreference_PreferenceId");

            migrationBuilder.RenameIndex(
                name: "IX_T_Customer_Preferences_CustomerId",
                table: "CustomerPreference",
                newName: "IX_CustomerPreference_CustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerPreference",
                table: "CustomerPreference",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerPreference_T_Customers_CustomerId",
                table: "CustomerPreference",
                column: "CustomerId",
                principalTable: "T_Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerPreference_T_Preferences_PreferenceId",
                table: "CustomerPreference",
                column: "PreferenceId",
                principalTable: "T_Preferences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
