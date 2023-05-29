using Prog7311.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Prog7311.Controllers
{
    public class RoleController : Controller
    {
        ApplicationDbContext context;
        public RoleController()
        {
            context = new ApplicationDbContext();

        }
        UserController UC = new UserController();
        //----------------------------------------------------------------------------------------------------\\
        // GET: Role
        public ActionResult Index()
        {


            if (User.Identity.IsAuthenticated)
            {


                if (!UC.isAdminUser())
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            var Roles = context.Roles.ToList();
            return View(Roles);

        }
        //----------------------------------------------------------------------------------------------------\\
    }
}
//------------------------------------------------------End of File-------------------------------------------------------\\
