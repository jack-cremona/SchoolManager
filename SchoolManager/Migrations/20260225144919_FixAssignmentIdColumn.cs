using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManager.Migrations
{
    /// <inheritdoc />
    public partial class FixAssignmentIdColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Modules_ModulesModuleId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Teachers_TeachersTeacherId",
                table: "Assignments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Assignments",
                table: "Assignments");

            migrationBuilder.RenameColumn(
                name: "TeachersTeacherId",
                table: "Assignments",
                newName: "TeacherId");

            migrationBuilder.RenameColumn(
                name: "ModulesModuleId",
                table: "Assignments",
                newName: "ModuleId");

            migrationBuilder.RenameIndex(
                name: "IX_Assignments_TeachersTeacherId",
                table: "Assignments",
                newName: "IX_Assignments_TeacherId");

            migrationBuilder.AddColumn<int>(
                name: "AssignmentId",
                table: "Assignments",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Assignments",
                table: "Assignments",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_ModuleId",
                table: "Assignments",
                column: "ModuleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Modules_ModuleId",
                table: "Assignments",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Teachers_TeacherId",
                table: "Assignments",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "TeacherId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Modules_ModuleId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Teachers_TeacherId",
                table: "Assignments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Assignments",
                table: "Assignments");

            migrationBuilder.DropIndex(
                name: "IX_Assignments_ModuleId",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "AssignmentId",
                table: "Assignments");

            migrationBuilder.RenameColumn(
                name: "TeacherId",
                table: "Assignments",
                newName: "TeachersTeacherId");

            migrationBuilder.RenameColumn(
                name: "ModuleId",
                table: "Assignments",
                newName: "ModulesModuleId");

            migrationBuilder.RenameIndex(
                name: "IX_Assignments_TeacherId",
                table: "Assignments",
                newName: "IX_Assignments_TeachersTeacherId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Assignments",
                table: "Assignments",
                columns: new[] { "ModulesModuleId", "TeachersTeacherId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Modules_ModulesModuleId",
                table: "Assignments",
                column: "ModulesModuleId",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Teachers_TeachersTeacherId",
                table: "Assignments",
                column: "TeachersTeacherId",
                principalTable: "Teachers",
                principalColumn: "TeacherId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
