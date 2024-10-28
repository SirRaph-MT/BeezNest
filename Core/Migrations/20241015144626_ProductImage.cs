using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class ProductImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductImage_UploadProducts_ProductId",
                table: "ProductImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductImage",
                table: "ProductImage");

            migrationBuilder.RenameTable(
                name: "ProductImage",
                newName: "ProductImages");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "ProductImages",
                newName: "UploadProductId");

            migrationBuilder.RenameColumn(
                name: "ImageId",
                table: "ProductImages",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_ProductImage_ProductId",
                table: "ProductImages",
                newName: "IX_ProductImages_UploadProductId");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "ProductImages",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductImages",
                table: "ProductImages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImages_UploadProducts_UploadProductId",
                table: "ProductImages",
                column: "UploadProductId",
                principalTable: "UploadProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductImages_UploadProducts_UploadProductId",
                table: "ProductImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductImages",
                table: "ProductImages");

            migrationBuilder.RenameTable(
                name: "ProductImages",
                newName: "ProductImage");

            migrationBuilder.RenameColumn(
                name: "UploadProductId",
                table: "ProductImage",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ProductImage",
                newName: "ImageId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductImages_UploadProductId",
                table: "ProductImage",
                newName: "IX_ProductImage_ProductId");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "ProductImage",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductImage",
                table: "ProductImage",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImage_UploadProducts_ProductId",
                table: "ProductImage",
                column: "ProductId",
                principalTable: "UploadProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
