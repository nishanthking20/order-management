using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Order_Management.Data;
using Order_Management.Models;
using Order_Management.Services;

namespace Order_Management.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService; // Implement IEmailService to send emails

        private User? otpReceivedUser; // Declare the field as nullable

        public AuthenticationController(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [HttpGet]
        public IActionResult LoginUser()
        {
            // Console.WriteLine(id);
            return View();
        }

        [HttpGet]
        public IActionResult RegisterUser()
        {
            // Console.WriteLine(id);
            return View();
        }

        [HttpPost]
        public IActionResult LoginUser(User user)
        {
            var users = _context.User;
            foreach(User user1 in users)
                Console.WriteLine(user1);
            var userInDb = _context.User.FirstOrDefault(u => u.Name == user.Name);
            if (userInDb != null)
            {
                if (user.Password == "admin")
                {
                    return RedirectToAction("AddItem", "AdminDashboard");
                }
                // Hash the input password
                using (SHA256 sha256Hash = SHA256.Create())
                {
                    if (user.Password != null)
                    {
                        byte[] hashedPasswordBytes = sha256Hash.ComputeHash(
                            Encoding.UTF8.GetBytes(user.Password)
                        );
                        StringBuilder hashedPasswordBuilder = new StringBuilder();
                        foreach (byte b in hashedPasswordBytes)
                        {
                            hashedPasswordBuilder.Append(b.ToString("x2"));
                        }

                        string hashedPassword = hashedPasswordBuilder.ToString();

                        // Compare the hashed password with the stored hashed password
                        if (userInDb != null && hashedPassword == userInDb.Password)
                        {
                            // Successful login, redirect to the dashboard
                            return RedirectToAction("Items", "Dashboard");
                        }
                        else
                        {
                            TempData["LoginAlertMessage"] = "Password is Incorrect.";
                            return View();
                        }
                    }
                }
            }

            TempData["LoginAlertMessage"] = "Username is Incorrect.";
            return View();
        }

        [HttpPost]
        public IActionResult RegisterUser(User user)
        {
            // Check if the model is valid based on data annotations and constraints
            if (!ModelState.IsValid)
            {
                // If the model is not valid, return the registration view with validation errors
                return View();
            }

            // Check if password matches confirm password
            if (user.Password != user.ConfirmPassword)
            {
                TempData["RegisterAlertMessage"] = "Password and Confirm Password are not equal.";
                return View();
            }

            // Hash the password
            using (SHA256 sha256Hash = SHA256.Create())
            {
                if (!string.IsNullOrEmpty(user.Password))
                {
                    byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(user.Password));
                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        builder.Append(bytes[i].ToString("x2"));
                    }
                    user.Password = builder.ToString();
                }
            }

            // Check if email or username already exists
            var existingEmail = _context.User.FirstOrDefault(u => u.Email == user.Email);
            var existingName = _context.User.FirstOrDefault(u => u.Name == user.Name);
            if (existingName != null || existingEmail != null)
            {
                TempData["RegisterAlertMessage"] =
                    "Email or Username already exists. Please use a different email address or username.";
                return View();
            }

            // Generate OTP and store it in session
            var otp = new Random().Next(100000, 999999);
            HttpContext.Session.SetInt32("OTP", otp);

            // Serialize user object with default options
            var userBytes = JsonSerializer.SerializeToUtf8Bytes(user);
            HttpContext.Session.Set("User", userBytes);

            // Send email with OTP to user's email address
            if (!string.IsNullOrEmpty(user.Email))
            {
                _emailService.SendEmail(user.Email, "Email Verification", $"Your OTP is: {otp}");
            }

            // Assign user to otpReceivedUser
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
                User? user =
                    JsonSerializer.Deserialize<User>(new ReadOnlySpan<byte>(userBytes))
                    ?? new User();

                // Verify OTP
                if (sessionOTP.HasValue && otp == sessionOTP.Value.ToString())
                {
                    // OTP is correct, proceed with registration
                    _context.User.Add(user);
                    _context.SaveChanges();
                    return RedirectToAction("Items", "Dashboard");
                }
                else
                {
                    // OTP is incorrect, display error message
                    TempData["OtpAlertMessage"] = "Invalid OTP. Please try again.";
                    // ModelState.AddModelError("otp", "Invalid OTP. Please try again.");
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
                return RedirectToAction("RegisterUser", "Authentication");
            }
        }
    }
}
