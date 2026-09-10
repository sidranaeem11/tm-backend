using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddB2BFieldsToModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "ALTER TABLE \"Products\" ADD COLUMN IF NOT EXISTS \"IsB2B\" boolean NOT NULL DEFAULT false;");

            migrationBuilder.Sql(
                "ALTER TABLE \"Products\" ADD COLUMN IF NOT EXISTS \"IsB2C\" boolean NOT NULL DEFAULT false;");

            migrationBuilder.Sql(
                "ALTER TABLE \"Products\" ADD COLUMN IF NOT EXISTS \"MinOrderQuantity\" integer;");

            migrationBuilder.Sql(
                "ALTER TABLE \"Products\" ADD COLUMN IF NOT EXISTS \"WholesalePrice\" numeric;");

            migrationBuilder.Sql(
                "ALTER TABLE \"Products\" ADD COLUMN IF NOT EXISTS \"BulkPrice\" numeric;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "ALTER TABLE \"Products\" DROP COLUMN IF EXISTS \"IsB2B\";");

            migrationBuilder.Sql(
                "ALTER TABLE \"Products\" DROP COLUMN IF EXISTS \"IsB2C\";");

            migrationBuilder.Sql(
                "ALTER TABLE \"Products\" DROP COLUMN IF EXISTS \"MinOrderQuantity\";");

            migrationBuilder.Sql(
                "ALTER TABLE \"Products\" DROP COLUMN IF EXISTS \"WholesalePrice\";");

            migrationBuilder.Sql(
                "ALTER TABLE \"Products\" DROP COLUMN IF EXISTS \"BulkPrice\";");
        }
    }
}