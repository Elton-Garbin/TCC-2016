namespace StartIdea.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TamanhoCampoUserName : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Usuario", "UK_Usuario_UserName");
            AlterColumn("dbo.Usuario", "UserName", c => c.String(nullable: false, maxLength: 20));
            CreateIndex("dbo.Usuario", "UserName", unique: true, name: "UK_Usuario_UserName");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Usuario", "UK_Usuario_UserName");
            AlterColumn("dbo.Usuario", "UserName", c => c.String(nullable: false, maxLength: 256));
            CreateIndex("dbo.Usuario", "UserName", unique: true, name: "UK_Usuario_UserName");
        }
    }
}
