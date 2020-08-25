using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using WeddingPlanner.Models;

namespace WeddingPlanner.Controllers
{
    public class HomeController : Controller
    {
        private MyContext _context;

    

        public HomeController(MyContext context)
            {
            _context = context;
            }
        [HttpGet("")]
        public IActionResult Index ()
        {
            return View ("Index");
        }

        [HttpPost("/new")]
        public IActionResult CreateUser (IndexWrapper Form)
        {
            if(ModelState.IsValid)
            {
                // Initializing a PasswordHasher object, providing our User class as its type
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                Form.FormModel.Password = Hasher.HashPassword(Form.FormModel, Form.FormModel.Password);
                _context.Add(Form.FormModel);
                _context.SaveChanges();
                HttpContext.Session.SetInt32("UserId", Form.FormModel.UserId);
                return Dashboard();
            }   else
            {
                return Index ();
            }
                //Save your user object to the database
            
        }
      
        

        [HttpPost("/processlogin")]
        public IActionResult ProcessLogin(IndexWrapper userSubmission)
        {
            Console.WriteLine("hello world");
            if(ModelState.IsValid)
            {
            // If inital ModelState is valid, query for a user with provided email
                var userInDb = _context.Users.FirstOrDefault(u => u.Email == userSubmission.UserLogin.Email);
                // If no user exists with provided email
                if(userInDb == null)
                {   Console.WriteLine("got null for email");
                    // Add an error to ModelState and return to View!
                    ModelState.AddModelError("Email", "Invalid Email/Password");
                    return View("Index");
                }
             
                // Initialize hasher object
                var hasher = new PasswordHasher<UserLogin>();
                Console.WriteLine("made it to hasher");
            
                // verify provided password against hash stored in db
                var result = hasher.VerifyHashedPassword(userSubmission.UserLogin, userInDb.Password, userSubmission.UserLogin.Password);
            
                // result can be compared to 0 for failure
                if(result == 0)
                {   Console.WriteLine("result was null");
                    return View ("Index");
                }
                else 
                {
                    HttpContext.Session.SetInt32("UserId", userInDb.UserId);
                    return Dashboard ();
                }
            }
                else {

            return View("Index");
        }

        }
        [HttpGet("dashboard")]
        public IActionResult Dashboard ()
        {   int? uId = HttpContext.Session.GetInt32("UserId");
            if(uId == null)
            {
                return RedirectToAction("Index");
            }
            IndexWrapper Wrap = new IndexWrapper();
            User ActiveUser = _context.Users.FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("UserId"));
            if (ActiveUser == null)
            {
                return RedirectToAction("Index");
            }
            Wrap.User = ActiveUser;
            Wrap.AllWeddings = _context.Weddings.Include(w => w.Guests).ThenInclude(g => g.User).Where(w => w.WeddingDate > DateTime.Now).ToList();
            Wrap.CurrentUser =(int) uId;
            return View("Dashboard", Wrap);
        }
        
       [HttpGet("/new/wedding")]
       public IActionResult AddWedding ()
       {    int? uId = HttpContext.Session.GetInt32("UserId");
            if(uId == null)
            {
                return RedirectToAction("LogReg");
            }
           return View ("New");
       }
       [HttpPost("/new/wedding")]
       public IActionResult AddWedding(IndexWrapper Form)
       {
       int? uId = HttpContext.Session.GetInt32("UserId");
            if(uId == null)
            {
                return RedirectToAction("LogReg");
            }
       
            if(ModelState.IsValid)
                {
                Form.OneWedding.WeddingId= (int)uId;
                _context.Add(Form.OneWedding);
                _context.SaveChanges();
                return Dashboard ();
                }   
            else
            {
                return View("New");
            }
            
        }
        [HttpGet("/wedding/{id}")]
        public IActionResult Detail (int id)
        {
            IndexWrapper Wmod = new IndexWrapper();
            Wmod.OneWedding = _context.Weddings
            .Include(w => w.Guests)
            .ThenInclude(g => g.User)
            .FirstOrDefault(w =>w.WeddingId == id);

            return View ("Detail", Wmod);

        }
    
        [HttpPost("/delete/{id}")]
        public IActionResult Delete (int id)
        {   int? uId = HttpContext.Session.GetInt32("UserId");
            if(uId == null)
            {
                return RedirectToAction("Index");
            }

            Wedding ToDelete = _context.Weddings.FirstOrDefault(i =>i.WeddingId == (int) id);
            if(ToDelete == null )
                {return RedirectToAction("Dashboard");
                }
            _context.Remove(ToDelete);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }
        [HttpPost("/rsvp/{id}")]
        public IActionResult RSVP (int id, IndexWrapper Form)
        {
            int? uId = HttpContext.Session.GetInt32("UserId");
            if(uId == null)
            {
                return RedirectToAction("Index");
            }

            if(ModelState.IsValid)
                {
                   WeddingGuest ToAdd =  new WeddingGuest {
                       UserId = (int) uId, 
                       WeddingId = id
                   };
                    _context.Add(ToAdd);
                    _context.SaveChanges();
                    return RedirectToAction("Dashboard"); 
                   
                }  
            else {
                return Dashboard ();
            }

        }
        [HttpPost("/unrsvp/{id}")]
        public IActionResult UNRSVP (int id)
        {
            int? uId = HttpContext.Session.GetInt32("UserId");
            if(uId == null)
            {
                return RedirectToAction("Index");
            }
            WeddingGuest ToDelete = _context.WeddingGuests.FirstOrDefault(i =>i.WeddingId == (int) id);
                if(ToDelete == null )
                    {   
                    return RedirectToAction("Dashboard");
                    }
                _context.Remove(ToDelete);
                _context.SaveChanges();
                return RedirectToAction("Dashboard"); 
                   
                
        }
        [HttpGet("/logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }

}