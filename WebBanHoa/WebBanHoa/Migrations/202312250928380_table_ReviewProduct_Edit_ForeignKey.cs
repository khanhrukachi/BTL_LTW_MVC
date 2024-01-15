namespace WebBanHoa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class table_ReviewProduct_Edit_ForeignKey : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.tb_ReviewProduct", "ProductId");
            AddForeignKey("dbo.tb_ReviewProduct", "ProductId", "dbo.tb_Product", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tb_ReviewProduct", "ProductId", "dbo.tb_Product");
            DropIndex("dbo.tb_ReviewProduct", new[] { "ProductId" });
        }
    }
}
