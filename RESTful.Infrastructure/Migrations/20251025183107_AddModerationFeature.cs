using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RESTful.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddModerationFeature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ModeratedBy",
                table: "Presentations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModerationComment",
                table: "Presentations",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModerationDate",
                table: "Presentations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Presentations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Presentations_ModeratedBy",
                table: "Presentations",
                column: "ModeratedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Presentations_Participants_ModeratedBy",
                table: "Presentations",
                column: "ModeratedBy",
                principalTable: "Participants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Presentations_Participants_ModeratedBy",
                table: "Presentations");

            migrationBuilder.DropIndex(
                name: "IX_Presentations_ModeratedBy",
                table: "Presentations");

            migrationBuilder.DropColumn(
                name: "ModeratedBy",
                table: "Presentations");

            migrationBuilder.DropColumn(
                name: "ModerationComment",
                table: "Presentations");

            migrationBuilder.DropColumn(
                name: "ModerationDate",
                table: "Presentations");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Presentations");
        }
    }
}
