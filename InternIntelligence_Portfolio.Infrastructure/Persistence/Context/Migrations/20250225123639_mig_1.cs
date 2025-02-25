using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternIntelligence_Portfolio.Infrastructure.Persistence.Context.Migrations
{
    /// <inheritdoc />
    public partial class mig_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationFiles_Projects_ProjectId",
                table: "ApplicationFiles");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_Name",
                table: "Skills",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationFiles_Projects_ProjectId",
                table: "ApplicationFiles",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationFiles_Projects_ProjectId",
                table: "ApplicationFiles");

            migrationBuilder.DropIndex(
                name: "IX_Skills_Name",
                table: "Skills");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationFiles_Projects_ProjectId",
                table: "ApplicationFiles",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");
        }
    }
}
