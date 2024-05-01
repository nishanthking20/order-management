using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Order_Management.Data;
using Order_Management.Models;
using Order_Management.Services;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Order_Management.Controllers
{
    public class RegisterController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService; // Implement IEmailService to send emails

        private User? otpReceivedUser; // Declare the field as nullable

        public RegisterController(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [HttpPost]
        public IActionResult LoginUser(User user)
        {
            var userInDb = _context.User.FirstOrDefault(u => u.Name == user.Name);

            if (userInDb != null)
            {
                // Hash the input password
                using (SHA256 sha256Hash = SHA256.Create())
                {
                    if (user.Password != null)
                    {
                        byte[] hashedPasswordBytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(user.Password));
                        StringBuilder hashedPasswordBuilder = new StringBuilder();
                        foreach (byte b in hashedPasswordBytes)
                        {
                            hashedPasswordBuilder.Append(b.ToString("x2"));
                        }

                        string hashedPassword = hashedPasswordBuilder.ToString();

                        // Compare the hashed password with the stored hashed password
                        if (userInDb != null && hashedPassword == userInDb.Password)
                        {
                            Console.WriteLine(userInDb.Password + " " + hashedPassword);
                            // Successful login, redirect to the dashboard
                            return RedirectToAction("Items", "Dashboard");
                        }
                    }
                }

            }

            // Either user not found or incorrect password, redirect to the login page
            return RedirectToAction("Login", "Home");
        }

        [HttpPost]
        public IActionResult RegisterUser(User user)
        {
            // Check if the model is valid based on data annotations and constraints
            if (!ModelState.IsValid)
            {
                // If the model is not valid, return the registration view with validation errors
                return RedirectToAction("Register", "Home");
            }

            // Hash the password
            using (SHA256 sha256Hash = SHA256.Create())
            {
                if (user.Password != null)
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

            var existingEmail = _context.User.FirstOrDefault(u => u.Email == user.Email);
            var existingName = _context.User.FirstOrDefault(u => u.Name ==user.Name);
            Console.WriteLine(existingEmail+" "+existingName);
            if (existingName != null || existingEmail != null)
            {
                // Add model error indicating that the email already exists
                ModelState.AddModelError("Email Or Name", "Email or Name already exists. Please use a different email address.");
                return RedirectToAction("Register", "Home");
            }

            // Generate OTP and store it in session
            var otp = new Random().Next(100000, 999999);
            HttpContext.Session.SetInt32("OTP", otp);

            // Serialize user object with default options
            var userBytes = JsonSerializer.SerializeToUtf8Bytes(user);
            HttpContext.Session.Set("User", userBytes);

            // Send email with OTP to user's email address
            if (user.Email != null)
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
                User? user = JsonSerializer.Deserialize<User>(new ReadOnlySpan<byte>(userBytes)) ?? new User();


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
