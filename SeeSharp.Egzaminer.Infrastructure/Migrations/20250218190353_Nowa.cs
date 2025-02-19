using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeeSharp.Egzaminer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Nowa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsManuallyGraded",
                table: "AnswerSubmitted",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ManualGradingResults",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnswerSubmittedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Points = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    GradedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AnswerSubmittedId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManualGradingResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ManualGradingResults_AnswerSubmitted_AnswerSubmittedId",
                        column: x => x.AnswerSubmittedId,
                        principalTable: "AnswerSubmitted",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ManualGradingResults_AnswerSubmitted_AnswerSubmittedId1",
                        column: x => x.AnswerSubmittedId1,
                        principalTable: "AnswerSubmitted",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ManualGradingResults_AnswerSubmittedId",
                table: "ManualGradingResults",
                column: "AnswerSubmittedId");

            migrationBuilder.CreateIndex(
                name: "IX_ManualGradingResults_AnswerSubmittedId1",
                table: "ManualGradingResults",
                column: "AnswerSubmittedId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ManualGradingResults");

            migrationBuilder.DropColumn(
                name: "IsManuallyGraded",
                table: "AnswerSubmitted");
        }
    }
}
