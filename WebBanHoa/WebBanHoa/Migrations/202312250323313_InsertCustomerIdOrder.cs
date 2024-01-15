namespace WebBanHoa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InsertCustomerIdOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Order", "CustomerID", c => c.String());
            AddColumn("dbo.tb_Order", "Status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tb_Order", "Status");
            DropColumn("dbo.tb_Order", "CustomerID");
        }
    }
}
