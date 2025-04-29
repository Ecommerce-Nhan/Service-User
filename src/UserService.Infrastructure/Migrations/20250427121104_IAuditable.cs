using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IAuditable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Roles");

            migrationBuilder.RenameColumn(
                name: "UpdateDate",
                table: "Users",
                newName: "IsDelete");

            migrationBuilder.RenameColumn(
                name: "DeleteBy",
                table: "Users",
                newName: "LastModifiedBy");

            migrationBuilder.RenameColumn(
                name: "CreateBy",
                table: "Users",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "Roles",
                newName: "CreatedOn");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "Roles",
                newName: "LastModifiedBy");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedOn",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Roles",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedOn",
                table: "Roles",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "RoleClaims",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "RoleClaims",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "RoleClaims",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Group",
                table: "RoleClaims",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "RoleClaims",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedOn",
                table: "RoleClaims",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastModifiedOn",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "LastModifiedOn",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "RoleClaims");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "RoleClaims");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "RoleClaims");

            migrationBuilder.DropColumn(
                name: "Group",
                table: "RoleClaims");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "RoleClaims");

            migrationBuilder.DropColumn(
                name: "LastModifiedOn",
                table: "RoleClaims");

            migrationBuilder.RenameColumn(
                name: "LastModifiedBy",
                table: "Users",
                newName: "DeleteBy");

            migrationBuilder.RenameColumn(
                name: "IsDelete",
                table: "Users",
                newName: "UpdateDate");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Users",
                newName: "CreateBy");

            migrationBuilder.RenameColumn(
                name: "LastModifiedBy",
                table: "Roles",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "Roles",
                newName: "UpdatedDate");

            migrationBuilder.AddColumn<bool>(
                name: "CreateDate",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Roles",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Roles",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
