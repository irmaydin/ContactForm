using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;


namespace ArgeyaContactForm.Pages
{
    public class IndexModel : PageModel
    {

        public ContactInformation ContactInformation { get; set; }
        public string successMessage { get; set; }
        public string ErrorMessage { get; set; }


        public void OnGet()
        {

        }
        public void OnPost()
        {
            ErrorMessage = "";

            // Create new ContactInformation object with values from Form
            ContactInformation ContactInformation = new()
            {
                Date = DateTime.Now,
                FirstName = Request.Form["UserName"],
                LastName = Request.Form["UserSurname"],
                PhoneNumber = Request.Form["PhoneNumber"],
                Email = Request.Form["Email"],
                Department = Request.Form["Department"],
                UserMessage = Request.Form["UserMessage"],

            };

            try
            {
                // Check is there any null value
                if (
                    string.IsNullOrEmpty(ContactInformation.FirstName) ||
                    string.IsNullOrEmpty(ContactInformation.LastName) ||
                    string.IsNullOrEmpty(ContactInformation.PhoneNumber) ||
                    string.IsNullOrEmpty(ContactInformation.Email) ||
                    string.IsNullOrEmpty(ContactInformation.Department) ||
                    string.IsNullOrEmpty(ContactInformation.UserMessage))
                {
                    ErrorMessage = "You must fill in all fields";
                    return;
                }

                // Connect Database
                string connectionString = "Data Source=.;Initial Catalog=ContactForm;Integrated Security=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    connection.Open();

                    // Use Stored Procedure for insert values to the Database Table
                    using (SqlCommand command = new SqlCommand("SubmitForm", connection))
                    {

                        command.CommandType = CommandType.StoredProcedure;
                        // Set the parameters for the stored procedure
                        command.Parameters.AddWithValue("@Date", ContactInformation.Date);
                        command.Parameters.AddWithValue("@FirstName", ContactInformation.FirstName);
                        command.Parameters.AddWithValue("@LastName", ContactInformation.LastName);
                        command.Parameters.AddWithValue("@PhoneNumber", ContactInformation.PhoneNumber);
                        command.Parameters.AddWithValue("@Email", ContactInformation.Email);
                        command.Parameters.AddWithValue("@Department", ContactInformation.Department);
                        command.Parameters.AddWithValue("@UserMessage", ContactInformation.UserMessage);

                        // Execute the stored procedure
                        command.ExecuteNonQuery();

                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                // Log or print the exception message for debugging purposes
                Console.WriteLine(ex.Message);
            }

            // Clean attributes
            ContactInformation.Date = DateTime.Now;
            ContactInformation.FirstName = "";
            ContactInformation.LastName = "";
            ContactInformation.PhoneNumber = "";
            ContactInformation.Email = "";
            ContactInformation.Department = "";
            ContactInformation.UserMessage = "";


            successMessage = "Your Message Is Sended Successfully";
        }
    }

    // Class for keep ContactInformation table values
    public class ContactInformation
    {
        public required DateTime Date;
        public required String FirstName;
        public required String LastName;
        public required String PhoneNumber;
        public required String Email;
        public required String Department;
        public required String UserMessage;

        public ContactInformation()
        {

        }
    }
}
