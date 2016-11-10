namespace StartIdea.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CpfNotUnique : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Usuario", "UK_Usuario_CPF");
        }
        
        public override void Down()
        {
            CreateIndex("dbo.Usuario", "CPF", unique: true, name: "UK_Usuario_CPF");
        }
    }
}
