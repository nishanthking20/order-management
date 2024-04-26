using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Order_Management.Data;
using Order_Management.Models;
using Order_Management.Services;
using System.Security.Cryptography;
using System.Text;

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
            // Generate OTP (Example: 6-digit random number)
            var otp = new Random().Next(100000, 999999);

            // Store OTP along with user's email in the database
            user.VerificationCode = otp;
            user.IsEmailVerified = false; // Set email verification status to false
            _context.User.Add(user);
            _context.SaveChanges();

            // Send email with OTP to user's email address
            _emailService.SendEmail(user.Email, "Email Verification", $"Your OTP is: {otp}");

            // Redirect to the email verification page
            return RedirectToAction("VerifyEmail", new { email = user.Email });
        }


        [HttpGet]
        public IActionResult VerifyEmail()
        {
            return View();
        }

        [HttpPost]
        public IActionResult VerifyEmail(string otp)
        {
            // Retrieve OTP and user's email from session or database
            var sessionOTP = HttpContext.Session.GetString("OTP");
            var userEmail = HttpContext.Session.GetString("UserEmail");

            // Verify the entered OTP
            if (otp == sessionOTP)
            {
                // OTP is correct, proceed with registration
                // You can register the user here or redirect to the registration success page
                return RedirectToAction("Dashboard","Shared");
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