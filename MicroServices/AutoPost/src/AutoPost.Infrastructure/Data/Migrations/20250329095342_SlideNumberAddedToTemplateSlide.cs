using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SMP.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class SlideNumberAddedToTemplateSlide : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SlideNumber",
                table: "TemplateSlides",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SlideNumber",
                table: "TemplateSlides");
        }
    }
}
