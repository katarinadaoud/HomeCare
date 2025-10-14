using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeCareApp.Migrations
{
    /// <inheritdoc />
    public partial class UppdatePatientEmployeeValidation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "phonenumber",
                table: "Patients",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "phonenumber",
                table: "Patients");
        }
    }
}
