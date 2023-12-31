﻿using System.ComponentModel.DataAnnotations;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Authentication_Service.Models
{
	public class User
	{
        [Required(ErrorMessage = "UserId is required")]
        public string UserID { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        public string? ResetPasswordToken { get; set; }
        
        public DateTime ResetPasswordTokenExpiry { get; set; }

        [Required(ErrorMessage = "User must always have a role")]
        public int RoleID { get; set; }

        public Role? Role { get; set; }

        public User()
        {
            
        }

        public User(string userId, string email, string password, int roleID)
        {
            UserID = userId;
            Email = email;
            Password = password;
            RoleID = roleID;
        }


        public bool ConfirmPasswords(string inputPassword, string inputConfirmPassword)
        {
            return inputPassword == inputConfirmPassword;
        }

        public string HashPassword(string inputPassword)
        {
            if (string.IsNullOrEmpty(inputPassword))
            {
                throw new ArgumentException("Input password cannot be null or empty.");
            }

            string encryptedPassword = BCrypt.Net.BCrypt.HashPassword(inputPassword);
            Password = encryptedPassword; 

            return encryptedPassword;
        }


        public bool VerifyPassword(string enteredPassword, string hashedPassword)
        {
            if (string.IsNullOrEmpty(enteredPassword) || string.IsNullOrEmpty(hashedPassword))
            {
                // Handle invalid input (e.g., log, throw an exception, etc.)
                return false;
            }
            // Compare the entered password with the stored hashed password
            return BCrypt.Net.BCrypt.Verify(enteredPassword, hashedPassword);
        }

     
        public string GeneratePasswordResetToken()
        {
            // Generate a unique token for resetting the password 
            return Guid.NewGuid().ToString();
        }

        // Helper method to send a password reset email (use your own logic here)
        async public Task SendPasswordResetEmail(string userEmail, string token)
        {
            try
            {
                var apiKey = Environment.GetEnvironmentVariable("EMAIL_PROVIDER_KEY");
                var client = new SendGridClient(apiKey);

                var from = new EmailAddress("narvaezfb4@hotmail.com");
                var subject = "Sending with SendGrid is Fun";
                var to = new EmailAddress(userEmail);
                var plainTextContent = "and easy to do anywhere, even with C#";
                var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";

                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = await client.SendEmailAsync(msg);

                if (response.StatusCode != System.Net.HttpStatusCode.OK && response.StatusCode != System.Net.HttpStatusCode.Accepted)
                {
                    // Handle error or log unsuccessful email sending
                    // You may want to throw an exception or log this for further investigation
                    // For example:
                    Console.WriteLine($"Email sending failed with status codee: {response.StatusCode}");
                }
                else
                {
                    Console.WriteLine(response.StatusCode);
                }
            }
            catch(Exception ex)
            {
                // Handle exceptions during email sending
                // For example:
                Console.WriteLine($"Error sending email: {ex.Message}");
                throw; // Propagate the exception up the call stack
            }

        }
    }
}

