using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Order_Management.Data;
using Order_Management.Models;
using Order_Management.Services;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Order_Management.Controllers {

    public class RegisterController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService; // Implement IEmailService to send emails

        public RegisterController(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [HttpPost]
        public IActionResult RegisterUser(User user)
        {

            Console.WriteLine(user.ToString());

            // Check if the model is valid based on data annotations and constraints
            if (!ModelState.IsValid)
            {
                // If the model is not valid, return the registration view with validation errors
                return RedirectToAction("Register","Home");
            }

            // Generate OTP and store it in session
            var otp = new Random().Next(100000, 999999);
             
            user.VerificationCode = otp;  // Store OTP along with user's email in the database
            user.IsEmailVerified = false; // Set email verification status to false
            HttpContext.Session.SetInt32("OTP", otp);
            HttpContext.Session.SetString("UserEmail", user.Email);


           
                
            // Send email with OTP to user's email address
            _emailService.SendEmail(user.Email, "Email Verification", $"Your OTP is: {otp}");

            // Redirect to the email verification page
            return RedirectToAction("VerifyEmail");
        }
    


        [HttpGet]
        public IActionResult VerifyEmail()
        {
            return View();
        }

        [HttpPost]
        public IActionResult VerifyEmail(User user,string otp)
        {
            // Retrieve OTP and user's email from session
            var sessionOTP = HttpContext.Session.GetInt32("OTP");
            var userEmail = HttpContext.Session.GetString("UserEmail");

            // Verify OTP
            if (sessionOTP.HasValue && otp == sessionOTP.Value.ToString())
            {
                // OTP is correct, proceed with registration
                _context.User.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Dashboard", "Shared");
            }
            else
            {
                // OTP is incorrect, display error message
                ModelState.AddModelError("otp", "Invalid OTP. Please try again.");
                return View();
            }
        }

    }

}