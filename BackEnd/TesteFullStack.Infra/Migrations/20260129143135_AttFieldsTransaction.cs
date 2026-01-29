using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TesteFullStack.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AttFieldsTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_transactions_CategoryId",
                table: "transactions",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_PersonId",
                table: "transactions",
                column: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_categories_CategoryId",
                table: "transactions",
                column: "CategoryId",
                principalTable: "categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_persons_PersonId",
                table: "transactions",
                column: "PersonId",
                principalTable: "persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_transactions_categories_CategoryId",
                table: "transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_transactions_persons_PersonId",
                table: "transactions");

            migrationBuilder.DropIndex(
                name: "IX_transactions_CategoryId",
                table: "transactions");

            migrationBuilder.DropIndex(
                name: "IX_transactions_PersonId",
                table: "transactions");
        }
    }
}
