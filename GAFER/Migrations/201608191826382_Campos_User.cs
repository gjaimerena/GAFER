namespace GAFER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Campos_User : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "CodigoColegio", c => c.String(nullable: false, maxLength: 5));
            AddColumn("dbo.AspNetUsers", "Denominacion", c => c.String(maxLength: 40));
            AddColumn("dbo.AspNetUsers", "CondicionIVA", c => c.Int());
            AddColumn("dbo.AspNetUsers", "CUIT", c => c.String());
            AddColumn("dbo.AspNetUsers", "Mail", c => c.String());
            AddColumn("dbo.AspNetUsers", "Contacto", c => c.String());
            AddColumn("dbo.AspNetUsers", "Direccion", c => c.String());
            AddColumn("dbo.AspNetUsers", "CantidadVencimientos", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "Observaciones", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Observaciones");
            DropColumn("dbo.AspNetUsers", "CantidadVencimientos");
            DropColumn("dbo.AspNetUsers", "Direccion");
            DropColumn("dbo.AspNetUsers", "Contacto");
            DropColumn("dbo.AspNetUsers", "Mail");
            DropColumn("dbo.AspNetUsers", "CUIT");
            DropColumn("dbo.AspNetUsers", "CondicionIVA");
            DropColumn("dbo.AspNetUsers", "Denominacion");
            DropColumn("dbo.AspNetUsers", "CodigoColegio");
        }
    }
}
