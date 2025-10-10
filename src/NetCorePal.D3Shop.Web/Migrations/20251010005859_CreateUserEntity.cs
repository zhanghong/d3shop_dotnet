using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetCorePal.D3Shop.Web.Migrations
{
    /// <inheritdoc />
    public partial class CreateUserEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, defaultValue: "", comment: "登录名")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    avatar = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false, defaultValue: "", comment: "用户头像")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    phone = table.Column<string>(type: "varchar(11)", maxLength: 11, nullable: false, defaultValue: "", comment: "手机号码")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, defaultValue: "", comment: "邮箱")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    password_hash = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, defaultValue: "", comment: "密码")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    password_salt = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: false, defaultValue: "", comment: "密码Salt")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    password_failed_times = table.Column<int>(type: "int", nullable: false, defaultValue: 0, comment: "登录失败次数"),
                    is_disabled = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false, comment: "是否禁用"),
                    disabled_at = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true, comment: "禁用时间"),
                    disabled_reason = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false, defaultValue: "", comment: "禁用原因")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    is_two_factor_enabled = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false, comment: "是否双认证"),
                    last_login_at = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true, comment: "最后登录时间"),
                    created_at = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false, comment: "创建时间"),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false, comment: "更新时间"),
                    is_deleted = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false, comment: "是否删除"),
                    deleted_at = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true, comment: "删除时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
