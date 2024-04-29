using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Order_Management.Data;
using Order_Management.Models;
using Order_Management.Services;
using System.Text.Json;
namespace Order_Management.Controllers 
{
    public class RegisterController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService; // Implement IEmailService to send emails

        private User otpReceivedUser;

        public RegisterController(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [HttpPost]
        public IActionResult LoginUser(User user){

            var Name = _context.User.FirstOrDefault(u => u.Name == user.Name);
            var Password = _context.User.FirstOrDefault(u => u.Password == user.Password);
            if(Name!=null && Password!=null)
            {
                return RedirectToAction("Items","Dashboard");
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        


        [HttpPost]
        public IActionResult RegisterUser(User user)
        {
            // Check if the model is valid based on data annotations and constraints
            if (!ModelState.IsValid)
            {
                // If the model is not valid, return the registration view with validation errors
                return RedirectToAction("Register","Home");
            }

            var existingUser = _context.User.FirstOrDefault(u => u.Email == user.Email);
            if (existingUser != null)
            {
                // Add model error indicating that the email already exists
                ModelState.AddModelError("Email", "Email already exists. Please use a different email address.");
                return RedirectToAction("Register","Home");
            }
        
            // Generate OTP and store it in session
            var otp = new Random().Next(100000, 999999);
            HttpContext.Session.SetInt32("OTP", otp);
            var userBytes = JsonSerializer.SerializeToUtf8Bytes(user,
                    new JsonSerializerOptions { WriteIndented = false, IgnoreNullValues = true });
            HttpContext.Session.Set("User", userBytes);
                
            // Send email with OTP to user's email address
            _emailService.SendEmail(user.Email, "Email Verification", $"Your OTP is: {otp}");

            otpReceivedUser = user;

            // Redirect to the email verification page
            return RedirectToAction("VerifyEmail");
        }

        [HttpGet]
        public IActionResult VerifyEmail()
        {
            return View();
        }

        [HttpPost]
        public IActionResult VerifyEmail(string otp)
        {
            try
            {
                // Retrieve OTP and user's email from session
                var sessionOTP = HttpContext.Session.GetInt32("OTP");
                var userBytes = HttpContext.Session.Get("User");
                User user = JsonSerializer.Deserialize<User>(new ReadOnlySpan<byte>(userBytes));

                // Verify OTP
                if (sessionOTP.HasValue && otp == sessionOTP.Value.ToString())
                {
                    // OTP is correct, proceed with registration
                    _context.User.Add(user);
                    _context.SaveChanges();
                    return RedirectToAction("Items","Dashboard");
                }
                else
                {
                    // OTP is incorrect, display error message
                    ModelState.AddModelError("otp", "Invalid OTP. Please try again.");
                    return View();
                }
            }
            catch (DbUpdateException ex)
            {
                // Handle database update exception
                // Check if InnerException is not null before accessing it
                if (ex.InnerException != null)
                {
                    // Access InnerException properties or handle it further
                    Console.WriteLine("InnerException Message: " + ex.InnerException.Message);
                }
                else
                {
                    Console.WriteLine("No InnerException found.");
                }
                
                // Redirect to a login page or return an error view
                return RedirectToAction("Register");
            }
        }

    }
}

