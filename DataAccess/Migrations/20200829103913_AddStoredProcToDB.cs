using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class AddStoredProcToDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                                    CREATE PROC SP_GETALLCOVERTYPES
                                    AS
                                    BEGIN
                                    SELECT * FROM DBO.COVERTYPES
                                    END
                                   ");

            migrationBuilder.Sql(@"
                                    CREATE PROC SP_GETCOVERTYPE
                                    @id int
                                    AS
                                    BEGIN
                                    SELECT * FROM DBO.COVERTYPES where CoverTypeId=@id
                                    END
                                   ");
            migrationBuilder.Sql(@"
                                    CREATE PROC SP_INSERTCOVERTYPE
                                    @NAME VARCHAR(100)
                                    AS
                                    BEGIN
                                    INSERT INTO DBO.COVERTYPES (CoverTypeName) VALUES( @NAME )
                                    END
                                   ");
            migrationBuilder.Sql(@"
                                    CREATE PROC SP_UPDATECOVERTYPE
                                    @id int,
                                    @name varchar(100)
                                    AS
                                    BEGIN
                                    UPDATE DBO.COVERTYPES SET CoverTypeName=@name where CoverTypeId=@id
                                    END
                                   ");
            migrationBuilder.Sql(@"
                                    CREATE PROC SP_DELETECOVERTYPE
                                    @id int
                                    AS
                                    BEGIN
                                    DELETE FROM DBO.COVERTYPES where CoverTypeId=@id
                                    END
                                   ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROC SP_GETALLCOVERTYPES");
            migrationBuilder.Sql(@"DROP PROC SP_GETCOVERTYPE");
            migrationBuilder.Sql(@"DROP PROC SP_INSERTCOVERTYPE");
            migrationBuilder.Sql(@"DROP PROC SP_UPDATECOVERTYPE");
            migrationBuilder.Sql(@"DROP PROC SP_DELETECOVERTYPE");

        }
    }
}

