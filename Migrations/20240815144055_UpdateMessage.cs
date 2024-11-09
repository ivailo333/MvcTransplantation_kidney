using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MvcTransplantation_kidney.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SenderId",
                table: "Messages",
                newName: "PatientId");

            migrationBuilder.RenameColumn(
                name: "ReceiverId",
                table: "Messages",
                newName: "DoctorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PatientId",
                table: "Messages",
                newName: "SenderId");

            migrationBuilder.RenameColumn(
                name: "DoctorId",
                table: "Messages",
                newName: "ReceiverId");
        }
    }
}
