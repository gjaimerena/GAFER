namespace GAFER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Campos_User1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "CondicionIVA", c => c.Int());
            AlterColumn("dbo.AspNetUsers", "CantidadVencimientos", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "CantidadVencimientos", c => c.String());
            AlterColumn("dbo.AspNetUsers", "CondicionIVA", c => c.String());
        }
    }
}
