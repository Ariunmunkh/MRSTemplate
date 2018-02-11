namespace DXWebMRCS.Migrations
{
    using DXWebMRCS.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Security;
    using WebMatrix.WebData;

    internal sealed class Configuration : DbMigrationsConfiguration<DXWebMRCS.Models.UsersContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DXWebMRCS.Models.UsersContext context)
        {
            //this.AddUserAndRoles();
        }
    }
}
