using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevConnect.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInterview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CvUrl",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExperienceLevel",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<List<string>>(
                name: "Skills",
                table: "Users",
                type: "text[]",
                nullable: false,
                defaultValue: new List<string>());

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResultNote",
                table: "Interviews",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "Score",
                table: "Interviews",
                type: "smallint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CvUrl",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ExperienceLevel",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Skills",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ResultNote",
                table: "Interviews");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "Interviews");
        }
    }
}
