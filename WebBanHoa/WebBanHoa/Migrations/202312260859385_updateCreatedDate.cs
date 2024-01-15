namespace WebBanHoa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateCreatedDate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tb_Wishlist", "CreatedDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tb_Wishlist", "CreatedDate", c => c.String());
        }
    }
}
