using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseApp.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InstructorID1",
                table: "Courses",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Courses_InstructorID1",
                table: "Courses",
                column: "InstructorID1");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Instructors_InstructorID1",
                table: "Courses",
                column: "InstructorID1",
                principalTable: "Instructors",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Instructors_InstructorID1",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_InstructorID1",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "InstructorID1",
                table: "Courses");
        }
    }
}
