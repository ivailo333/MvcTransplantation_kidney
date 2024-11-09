using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MvcTransplantation_kidney.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDateSendMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "SentDate",
                table: "Messages",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "SentDate",
                table: "Messages",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
