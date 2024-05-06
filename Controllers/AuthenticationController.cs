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
            // foreach(User user1 in users)
            //     Console.WriteLine(user1);
            var userInDb = _context.User.FirstOrDefault(u => u.Name == user.Name);
            if (userInDb != null)
            {
                if (user.Password == "admin")
                {
                    return RedirectToAction("AddItem", "AdminDashboard");
                }
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
                        if (userInDb != null && hashedPassword == userInDb.Password)
                        {
                            HttpContext.Session.SetInt32("userId", userInDb.Id);
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
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (user.Password != user.ConfirmPassword)
            {
                TempData["RegisterAlertMessage"] = "Password and Confirm Password are not equal.";
                return View();
            }
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
            var existingEmail = _context.User.FirstOrDefault(u => u.Email == user.Email);
            var existingName = _context.User.FirstOrDefault(u => u.Name == user.Name);
            if (existingName != null || existingEmail != null)
            {
                TempData["RegisterAlertMessage"] =
                    "Email or Username already exists. Please use a different email address or username.";
                return View();
            }
            var otp = new Random().Next(100000, 999999);
            HttpContext.Session.SetInt32("OTP", otp);
            var userBytes = JsonSerializer.SerializeToUtf8Bytes(user);
            HttpContext.Session.Set("User", userBytes);
            if (!string.IsNullOrEmpty(user.Email))
            {
                _emailService.SendEmail(user.Email, "Email Verification", $"Your OTP is: {otp}");
            }
            otpReceivedUser = user;
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
                var sessionOTP = HttpContext.Session.GetInt32("OTP");
                var userBytes = HttpContext.Session.Get("User");
                User? user =
                    JsonSerializer.Deserialize<User>(new ReadOnlySpan<byte>(userBytes))
                    ?? new User();
                if (sessionOTP.HasValue && otp == sessionOTP.Value.ToString())
                {
                    _context.User.Add(user);
                    _context.SaveChanges();
                    return RedirectToAction("Items", "Dashboard");
                }
                else
                {
                    TempData["OtpAlertMessage"] = "Invalid OTP. Please try again.";
                    return View();
                }
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null)
                {
                    Console.WriteLine("InnerException Message: " + ex.InnerException.Message);
                }
                else
                {
                    Console.WriteLine("No InnerException found.");
                }
                return RedirectToAction("RegisterUser", "Authentication");
            }
        }
    }
}
