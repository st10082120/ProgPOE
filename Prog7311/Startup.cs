using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Owin;
using Prog7311.model;

[assembly: OwinStartupAttribute(typeof(Prog7311.Startup))]
namespace Prog7311
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRolesandUsers();
        }
        //-------------------------------------------------------------------------------------------------------------\\
        // creates default User roles and Employee user for login    
        private void CreateRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


           
            // creates an Employee super user who will maintain the website                  
            if (!roleManager.RoleExists("Employee"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Employee";
                roleManager.Create(role);

                var User = new ApplicationUser();
                User.UserName = "st10082120";
                User.Email = "dheilbron7@gmail.com";
                string userPWD = "password";

                var checkUser = UserManager.Create(User, userPWD);

                //Add default User to Role Employee    
                if (checkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(User.Id, "Employee");
                     
                }
            }
            // creates a Farmer User     
            if (!roleManager.RoleExists("Farmer"))
            {

                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Farmer";
              //  roleManager.Create(role);
               
                    var User = new ApplicationUser();
                    User.UserName = "FarmerGreen";
                    User.Email = "FarmerGreen@gmail.com";
                    string userPWD = "password";
                var checkUser = UserManager.Create(User, userPWD);
                if (checkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(User.Id, "Farmer");

                }
            }
        }

        }
    }

//----------------------------------------------------End of File---------------------------------------------------------------------\\
