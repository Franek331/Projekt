using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeeSharp.Egzaminer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAnswerSubmittedModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<Guid>(
                name: "TestPublicationId",
                table: "AnswerSubmitted",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnswerSubmitted_TestPublicationId",
                table: "AnswerSubmitted",
                column: "TestPublicationId");

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
                name: "FK_AnswerSubmitted_TestPublications_TestPublicationId",
                table: "AnswerSubmitted");

            migrationBuilder.DropForeignKey(
                name: "FK_TestSubmission_AspNetUsers_UserId1",
                table: "TestSubmission");

            migrationBuilder.DropIndex(
                name: "IX_AnswerSubmitted_TestPublicationId",
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
                name: "FK_TestSubmission_AspNetUsers_UserId1",
                table: "TestSubmission",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
