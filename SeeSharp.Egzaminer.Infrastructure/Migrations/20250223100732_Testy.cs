using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeeSharp.Egzaminer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Testy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswerSubmitted_Questions_QuestionId",
                table: "AnswerSubmitted");

            migrationBuilder.DropForeignKey(
                name: "FK_TestSubmission_AspNetUsers_UserId1",
                table: "TestSubmission");

            migrationBuilder.AlterColumn<string>(
                name: "UserId1",
                table: "TestSubmission",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<DateTime>(
                name: "SubmissionDate",
                table: "TestSubmission",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsManuallyGraded",
                table: "AnswerSubmitted",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "TestPublicationId",
                table: "AnswerSubmitted",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "ManualGradingResults",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnswerSubmittedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Points = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    GradedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnswerSubmitted_TestPublicationId",
                table: "AnswerSubmitted",
                column: "TestPublicationId");

            migrationBuilder.CreateIndex(
                name: "IX_ManualGradingResults_AnswerSubmittedId",
                table: "ManualGradingResults",
                column: "AnswerSubmittedId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerSubmitted_Questions_QuestionId",
                table: "AnswerSubmitted",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerSubmitted_TestPublications_TestPublicationId",
                table: "AnswerSubmitted",
                column: "TestPublicationId",
                principalTable: "TestPublications",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TestSubmission_AspNetUsers_UserId1",
                table: "TestSubmission",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswerSubmitted_Questions_QuestionId",
                table: "AnswerSubmitted");

            migrationBuilder.DropForeignKey(
                name: "FK_AnswerSubmitted_TestPublications_TestPublicationId",
                table: "AnswerSubmitted");

            migrationBuilder.DropForeignKey(
                name: "FK_TestSubmission_AspNetUsers_UserId1",
                table: "TestSubmission");

            migrationBuilder.DropTable(
                name: "ManualGradingResults");

            migrationBuilder.DropIndex(
                name: "IX_AnswerSubmitted_TestPublicationId",
                table: "AnswerSubmitted");

            migrationBuilder.DropColumn(
                name: "SubmissionDate",
                table: "TestSubmission");

            migrationBuilder.DropColumn(
                name: "IsManuallyGraded",
                table: "AnswerSubmitted");

            migrationBuilder.DropColumn(
                name: "TestPublicationId",
                table: "AnswerSubmitted");

            migrationBuilder.AlterColumn<string>(
                name: "UserId1",
                table: "TestSubmission",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerSubmitted_Questions_QuestionId",
                table: "AnswerSubmitted",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TestSubmission_AspNetUsers_UserId1",
                table: "TestSubmission",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
