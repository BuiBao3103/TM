using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TM.Migrations
{

    public partial class AddCodeToLocation : Migration
    {
        
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(name: "Code",table:"Location",nullable:false,defaultValue:"");
            migrationBuilder.Sql(@"
                UPDATE l 
                    SET l.Code = CASE
                    WHEN l.LocationName = 'Hanoi' THEN 'HAN'
                    WHEN l.LocationName = 'Ho Chi Minh city' THEN 'SGN'
                    WHEN l.LocationName = 'Da Nang' THEN 'DAD'
                    WHEN l.LocationName = 'Tokyo' THEN 'NRT'
                    WHEN l.LocationName = 'Osaka' THEN 'KIX'
                    WHEN l.LocationName = 'Kyoto' THEN 'KIX'
                    WHEN l.LocationName = 'Bangkok' THEN 'BKK'
                    WHEN l.LocationName = 'Chiang Mai' THEN 'CNX'
                    WHEN l.LocationName = 'Seoul' THEN 'ICN'
                    WHEN l.LocationName = 'Busan' THEN 'PUS'
                    WHEN l.LocationName = 'Singapore City' THEN 'SIN'
                    WHEN l.LocationName = 'Sentosa' THEN 'XSP'
                    ELSE ''
                END
                FROM Location l;");
        }

        
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "Code",table: "Location");
        }
    }
}
