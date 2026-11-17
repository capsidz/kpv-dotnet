using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApprovalService.Migrations
{
    /// <inheritdoc />
    public partial class AddAiAnalysisFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "ApprovalRequests",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Priority",
                table: "ApprovalRequests",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Summary",
                table: "ApprovalRequests",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "ApprovalRequests");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "ApprovalRequests");

            migrationBuilder.DropColumn(
                name: "Summary",
                table: "ApprovalRequests");
        }
    }
}
