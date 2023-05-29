using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Prog7311.model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Antlr.Runtime;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;

namespace Prog7311.Controllers
{
    public class FarmersController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;
        readonly ApplicationDbContext context;

        public FarmersController()
        {
            context = new ApplicationDbContext();

        }
        //----------------------------------------------------------------------------------------------------\\
        public FarmersController(ApplicationUserManager userManager)
        {
            UserManager = userManager;

        }

        //----------------------------------------------------------------------------------------------------\\

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        //----------------------------------------------------------------------------------------------------\\

        // GET: Farmers/AddFarmer
        [AllowAnonymous]
        public ActionResult AddFarmer()
        {
            ViewBag.Name = new SelectList(context.Roles.Where(u => u.Name.Contains("Farmer"))
                                           .ToList(), "Name");
            return View();
        }
        //----------------------------------------------------------------------------------------------------\\

        // POST: Farmers/AddFarmer
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddFarmer(Farmers farmer)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = farmer.UserName, Id = farmer.FarmerId };
                var result = await UserManager.CreateAsync(user, farmer.Password);
                if (result.Succeeded)
                {
                    // var user = new ApplicationUser();
                    //string userPWD = "password";

                    farmer.FarmerId = User.Identity.GetUserId();
                    farmer.UserName = User.Identity.GetUserName();
                    farmer.UserRoles = "Farmer";


                    //var checkUser = UserManager.Create(User, userPWD);
                    // Save the farmer to the database
                    db.Farmer.Add(farmer);
                    db.SaveChanges();
                    await this.UserManager.AddToRoleAsync(user.Id, farmer.UserRoles);
                    //Ends Here     
                    return RedirectToAction("Index", "Users");
                }
                ViewBag.Name = new SelectList(context.Roles.Where(u => !u.Name.Contains("Farmer"))
                                           .ToList(), "Name");
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form    
            return View(farmer);
        }

            public ActionResult AddProduct()
        {
            return View();
        }
        //----------------------------------------------------------------------------------------------------\\

        // POST: Product/AddProduct
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult AddProduct(Farmers product)
        {
            if (ModelState.IsValid)
            {
                // Save the product to the database
                //var name = User.Identity;
                product.UserName = User.Identity.GetUserName();
                product.FarmerId = User.Identity.GetUserId();
                product.UserId = User.Identity.GetUserId();


                db.Farmer.Add(product);
                db.SaveChanges();

                return RedirectToAction("Index", "Home"); // Redirect to a success page or desired action
            }

            return View(product);
        }
        //----------------------------------------------------------------------------------------------------\\

        // GET: Product/SupplierProducts/{supplierId}
        public ActionResult ProductType(String Type)
        {
            // Get the products supplied by the specified supplier
            var products = db.Farmer.Where(p => p.ProductType == Type).ToList();

            return View(products);
        }


        // Other action methods as needed

        //----------------------------------------------------------------------------------------------------\\
        //add errors
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

    }
}