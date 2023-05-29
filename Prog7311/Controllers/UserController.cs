using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Prog7311.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;

namespace Prog7311.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;
        readonly ApplicationDbContext context;

        public UserController()
        {
            context = new ApplicationDbContext();

        }
        //----------------------------------------------------------------------------------------------------\\
        public UserController(ApplicationUserManager userManager)
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
        // [ValidateAntiForgeryToken]
        //[HttpPost]
        //[Authorize]
        // GET: User
        public ActionResult Index()
        {
            // Check user role and set ViewBag.displayMenu accordingly
            if (User.IsInRole("Employee") || User.IsInRole("Farmer"))
            {
                ViewBag.displayMenu = "Yes";
            }
            else
            {
                ViewBag.displayMenu = "No";
            }

            // Set ViewBag.Name with the user's name if desired

            return View();
        }
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
        //-------------------------------------------------------------------------------------------------------------\\
        //[ValidateAntiForgeryToken]
        //[Authorize]
        public ActionResult AccessUserFunctions()
        {
           
                if (User.IsInRole("Employee"))
                {
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    // Handle the case when the user is not in the "Employee" role
                    return RedirectToAction("AccessDenied", "Error");
                    // You can create an "Error" controller with an "AccessDenied" action
                    // that displays an appropriate error message or redirects to another page.
                }

            
        }
        //-------------------------------------------------------------------------------------------------------------\\
        public Boolean isAdminUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                //ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
                if (s[0].ToString() == "Employee")
                {
                    return true;
                }
                else 
                {
                    return false;
                }
            }
            return false;
        }
        public ActionResult AddProduct()
        {
            return View();
        }
        //----------------------------------------------------------------------------------------------------\\

        // POST: Product/AddProduct

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
        public ActionResult Products(String userName)
        {
            // Retrieve the farmer and their associated products by ID from the database
            Farmers Farmers;
            Farmers = db.Farmer.Find(userName);

            var Products = Farmers.Product;
             Farmers = db.Farmer.Include(Products).SingleOrDefault(f => f.FarmerId == userName);

            if (Farmers == null)
            {
                // Farmer not found, return appropriate response (e.g., error view or redirect)
            }

            // Pass the farmer and their products to the view
            ViewBag.FarmerName = Farmers.UserName;
            return View(Farmers.Product.ToList());
        }

        // GET: Product/SupplierProducts/{supplierId}
        public ActionResult ProductType(String Type)
        {
            // Get the products supplied by the specified supplier
            var products = db.Farmer.Where(p => p.ProductType == Type).ToList();

            return View(products);
        }
        //----------------------------------------------------------------------------------------------------\\
        //add errors
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        //----------------------------------------------------------------------------------------------------\\
        //filters products by ProductType
        public ActionResult FilterProducts(String userName, string productType)
        {
            var products = db.Farmer.Where(p => p.UserName == userName);

            if (!string.IsNullOrEmpty(productType) && productType != "All")
            {
                products = products.Where(p => p.ProductType == productType);
            }

            var farmer = db.Farmer.Find(userName);
            ViewBag.Farmer = farmer;
            ViewBag.ProductTypes = new SelectList(db.Farmer.Select(p => p.ProductType).Distinct());

            return View(products.ToList());
        }

        //----------------------------------------------------------------------------------------------------\\
    }
}
//---------------------------------------------------------End of File----------------------------------------------------\\
