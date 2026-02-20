using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManager.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCompetence : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Modules_ModulesId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Competences_Subjects_SubjectsSubjectId",
                table: "Competences");

            migrationBuilder.DropForeignKey(
                name: "FK_Competences_Teachers_TeachersTeacherId",
                table: "Competences");

            migrationBuilder.RenameColumn(
                name: "TeachersTeacherId",
                table: "Competences",
                newName: "SubjectId");

            migrationBuilder.RenameColumn(
                name: "SubjectsSubjectId",
                table: "Competences",
                newName: "TeacherId");

            migrationBuilder.RenameIndex(
                name: "IX_Competences_TeachersTeacherId",
                table: "Competences",
                newName: "IX_Competences_SubjectId");

            migrationBuilder.RenameColumn(
                name: "ModulesId",
                table: "Assignments",
                newName: "ModulesModuleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Modules_ModulesModuleId",
                table: "Assignments",
                column: "ModulesModuleId",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Competences_Subjects_SubjectId",
                table: "Competences",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "SubjectId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Competences_Teachers_TeacherId",
                table: "Competences",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "TeacherId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Modules_ModulesModuleId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Competences_Subjects_SubjectId",
                table: "Competences");

            migrationBuilder.DropForeignKey(
                name: "FK_Competences_Teachers_TeacherId",
                table: "Competences");

            migrationBuilder.RenameColumn(
                name: "SubjectId",
                table: "Competences",
                newName: "TeachersTeacherId");

            migrationBuilder.RenameColumn(
                name: "TeacherId",
                table: "Competences",
                newName: "SubjectsSubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Competences_SubjectId",
                table: "Competences",
                newName: "IX_Competences_TeachersTeacherId");

            migrationBuilder.RenameColumn(
                name: "ModulesModuleId",
                table: "Assignments",
                newName: "ModulesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Modules_ModulesId",
                table: "Assignments",
                column: "ModulesId",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Competences_Subjects_SubjectsSubjectId",
                table: "Competences",
                column: "SubjectsSubjectId",
                principalTable: "Subjects",
                principalColumn: "SubjectId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Competences_Teachers_TeachersTeacherId",
                table: "Competences",
                column: "TeachersTeacherId",
                principalTable: "Teachers",
                principalColumn: "TeacherId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
