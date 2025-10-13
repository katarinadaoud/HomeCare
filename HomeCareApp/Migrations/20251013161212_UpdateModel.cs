using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeCareApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmergencyContacts_Patients_PatientId",
                table: "EmergencyContacts");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_AppointmentId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_EmergencyContacts_PatientId",
                table: "EmergencyContacts");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "EmergencyContacts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TaskId",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "Work_area",
                table: "Employees",
                newName: "FullName");

            migrationBuilder.RenameColumn(
                name: "Specialty",
                table: "Employees",
                newName: "Department");

            migrationBuilder.RenameColumn(
                name: "Availability",
                table: "Employees",
                newName: "Address");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Patients",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_AppointmentId",
                table: "Tasks",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_EmergencyContactId",
                table: "Patients",
                column: "EmergencyContactId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_EmergencyContacts_EmergencyContactId",
                table: "Patients",
                column: "EmergencyContactId",
                principalTable: "EmergencyContacts",
                principalColumn: "EmergencyContactId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_EmergencyContacts_EmergencyContactId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_AppointmentId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Patients_EmergencyContactId",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Patients");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "Employees",
                newName: "Work_area");

            migrationBuilder.RenameColumn(
                name: "Department",
                table: "Employees",
                newName: "Specialty");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Employees",
                newName: "Availability");

            migrationBuilder.AddColumn<int>(
                name: "PatientId",
                table: "EmergencyContacts",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TaskId",
                table: "Appointments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_AppointmentId",
                table: "Tasks",
                column: "AppointmentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmergencyContacts_PatientId",
                table: "EmergencyContacts",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmergencyContacts_Patients_PatientId",
                table: "EmergencyContacts",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "PatientId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
