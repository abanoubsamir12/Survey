using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Survey.Migrations
{
    /// <inheritdoc />
    public partial class AddAnsweredSurveyCheck : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSurvey_Surveys_SurveyTId",
                table: "UserSurvey");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSurvey_Users_UserId",
                table: "UserSurvey");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSurvey",
                table: "UserSurvey");

            migrationBuilder.RenameTable(
                name: "UserSurvey",
                newName: "UserSurveys");

            migrationBuilder.RenameIndex(
                name: "IX_UserSurvey_SurveyTId",
                table: "UserSurveys",
                newName: "IX_UserSurveys_SurveyTId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSurveys",
                table: "UserSurveys",
                columns: new[] { "UserId", "SurveyTId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserSurveys_Surveys_SurveyTId",
                table: "UserSurveys",
                column: "SurveyTId",
                principalTable: "Surveys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSurveys_Users_UserId",
                table: "UserSurveys",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSurveys_Surveys_SurveyTId",
                table: "UserSurveys");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSurveys_Users_UserId",
                table: "UserSurveys");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSurveys",
                table: "UserSurveys");

            migrationBuilder.RenameTable(
                name: "UserSurveys",
                newName: "UserSurvey");

            migrationBuilder.RenameIndex(
                name: "IX_UserSurveys_SurveyTId",
                table: "UserSurvey",
                newName: "IX_UserSurvey_SurveyTId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSurvey",
                table: "UserSurvey",
                columns: new[] { "UserId", "SurveyTId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserSurvey_Surveys_SurveyTId",
                table: "UserSurvey",
                column: "SurveyTId",
                principalTable: "Surveys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSurvey_Users_UserId",
                table: "UserSurvey",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
