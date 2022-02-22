using BookALookProject.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BookALookProject.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db;
        public HomeController()
        {

        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            db = HttpContext.GetOwinContext().Get<ApplicationDbContext>();

            base.OnActionExecuting(filterContext);
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SearchSalons(int? Category, string City, string Search)
        {
            if (Category == null) Category = 0;
            if (City == null) City = "";
            if (Search == null) Search = "";

            Category category = db.Categories.Where(c => c.CategoryId == Category).FirstOrDefault();
            //List<Salon> salons = db.Salons
            //.Where(c => c.Categories.Any(cat => cat.CategoryId == Category) && c.City.Contains(City) && c.Name.ToLower().Contains(Search.ToLower().Trim()))
            //.Include(c => c.Categories).ToList();

            List<Salon> salons = db.Salons.Include(s => s.Categories).ToList();
            List<Salon> filtered = new List<Salon>();
            foreach(var salon in salons)
            {
                if (salon.Categories.Any(c => c.CategoryId == Category || Category == 0) && salon.City.Contains(City) && salon.Name.ToLower().Contains(Search.ToLower().Trim()))
                    filtered.Add(salon);
            }
            
            return View("Salons", filtered);
        }
        public ActionResult ListSalons()
        {
            List<Salon> salons = db.Salons.Include(s => s.Categories).ToList();
            ViewBag.title = "ALL SALONS";
            return View("Salons", salons);
        }

        public ActionResult Salons (string category)
        {
            Category cat = db.Categories.Where(c => c.CategoryName == category).Include(c => c.Salons).FirstOrDefault();
            List<Salon> salons = cat.Salons.ToList();
            if(category != null)
                ViewBag.title = category.ToUpper();
            return View(salons);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Appointments()
        {
            string Role = "User";
            UserWithAppointments uwa;
            ApplicationUser user = db.Users.Where(u => u.UserName == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault();
            if (System.Web.HttpContext.Current.User.IsInRole("Salon"))
            {
                Role = "Salon";
                Salon s = db.Salons.Where(sal => sal.Name == user.Name).FirstOrDefault();
                List<Appointment> app = db.Appointments.Where(a => a.Salon.SalonId == s.SalonId).Include(a => a.Client).Include(a => a.Salon).ToList();
                uwa = new UserWithAppointments { Appointments = app, User = user, UserRole = Role };

            }
            else
            {
                uwa = new UserWithAppointments { Appointments = db.Appointments.Where(a => a.Client.Name == user.Name).Include(a => a.Salon).ToList(), User = user, UserRole = Role };
            }

            return View(uwa);
            
        }

        [HttpPost]
        [Authorize]
        public ActionResult Appointments(AppointmentViewModel model) 
        {
            ApplicationUser user = db.Users.Where(u => u.UserName == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault();
            Salon salon = db.Salons.Where(s => s.Name == model.SalonName).FirstOrDefault();

            Appointment app = new Appointment { AppointmentCategory = model.AppointmentCategory, AppointmentService = model.AppointmentService, Client = user, Salon = salon, Date = model.Date, Time = model.Time };
            ViewBag.Success = true;
            db.Appointments.Add(app);
            db.SaveChanges();

            return RedirectToAction("Salon", "Home", new { SalonId = salon.SalonId });
        }

        public ActionResult RemoveAppointment(int AppId)
        {
            Appointment app = db.Appointments.Where(a => a.AppointmentId == AppId).FirstOrDefault();
            db.Appointments.Remove(app);
            db.SaveChanges();

            return RedirectToAction("Appointments", "Home");
        }


        [HttpGet]
        [Authorize]
        public ActionResult Edit()
        {
            Salon salon = db.Salons.Where(s => s.Email == System.Web.HttpContext.Current.User.Identity.Name).Include(s => s.Categories).FirstOrDefault();
            List<SalonService> salonServices = db.SalonServices.Where(s => s.Salon.SalonId == salon.SalonId).ToList();
            SalonWithServices sws = new SalonWithServices { Salon = salon, SalonServices = salonServices };
            return View(sws);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Edit(RegisterSalonViewModel model)
        {
            Salon instance = db.Salons.Where(s => s.Email == System.Web.HttpContext.Current.User.Identity.Name).Include(s => s.Categories).FirstOrDefault();
           
            List<Category> categories = new List<Category>();
            foreach (string cat in model.Categories)
            {
                Category category = db.Categories.Where(c => c.CategoryName == cat).FirstOrDefault();
                instance.Categories.Remove(category);
                instance.Categories.Add(category);
            }

            instance.Name = model.FullName;
            instance.PhoneNum = model.PhoneNum;
            instance.Address = model.Address;
            
            instance.City = model.City;
            instance.WorkingFrom = model.WorkingFrom;
            instance.WorkingTo = model.WorkingTo;
            instance.Email = model.Email;
            instance.Description = model.SalonDescription;
            db.SaveChanges();

            if (model.File != null)
            {
                string pic = instance.SalonId + System.IO.Path.GetExtension(model.File.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/Content/images/"), pic);
                instance.ImagePath = "/Content/images/" + pic;
                db.SaveChanges();
                // file is uploaded
                model.File.SaveAs(path);
            }


            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize]
        public ActionResult Service()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Service(SalonServiceViewModel service)
        {
            Salon salon = db.Salons.Where(s => s.Email == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault();
            Category category = db.Categories.Where(c => c.CategoryName == service.CategoryName).FirstOrDefault();

            db.SalonServices.Add(new SalonService { Category = category, Name = service.Name, Price = service.Price, Salon = salon });
            db.SaveChanges();

            return RedirectToAction("Edit", "Home");
        }

        public ActionResult Salon(int SalonId)
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated && System.Web.HttpContext.Current.User.IsInRole("User"))
            {
                ViewBag.LoggedIn = true;
            }
            else
            {
                ViewBag.LoggedIn = false;
            }

            Salon salon = db.Salons.Where(s => s.SalonId == SalonId).Include(s => s.Categories).FirstOrDefault();
            List<SalonService> services = db.SalonServices.Where(sr => sr.Salon.SalonId == salon.SalonId).ToList();
            SalonWithServices sws = new SalonWithServices { Salon = salon, SalonServices = services };

            return View(sws);
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public ActionResult Salon(AppointmentViewModel model)
        {
            ApplicationUser client = db.Users.Where(u => u.UserName == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault();
            Salon salon = db.Salons.Where(s => s.Name == model.SalonName).Include(s => s.Categories).FirstOrDefault();
            List<SalonService> services = db.SalonServices.Where(sr => sr.Salon.SalonId == salon.SalonId).ToList();
            SalonWithServices sws = new SalonWithServices { Salon = salon, SalonServices = services };

            Appointment app = new Appointment
            {
                AppointmentCategory = model.AppointmentCategory,
                AppointmentService = model.AppointmentService,
                Client = client,
                Salon = salon,
                Date = model.Date,
                Time = model.Time
            };

            db.Appointments.Add(app);
            db.SaveChanges();

            return RedirectToAction("Salon", "Home", new { SalonId = salon.SalonId });
        }
    }
}