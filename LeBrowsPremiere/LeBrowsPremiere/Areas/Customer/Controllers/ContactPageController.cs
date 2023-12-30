using LeBrowsPremiere.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;

namespace LeBrowsPremiere.Areas.Customer.Controllers
{
    public class ContactPageController : Controller
    {
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost()]
        public async Task<IActionResult> Contact(ContactFormViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    MailMessage msg = new MailMessage();

                    // Sender and recipient address
                    msg.From = new MailAddress("email", "LeBrows Premiere");
                    msg.To.Add(new MailAddress(viewModel.ContactEmail, viewModel.ContactFirstName + " " + viewModel.ContactLastName));

                    // Message subject and body
                    msg.Subject = "Contact Us";
                    msg.Body = "First Name: " + viewModel.ContactFirstName;
                    msg.Body += "Last Name: " + viewModel.ContactLastName;
                    msg.Body += "Email: " + viewModel.ContactEmail;
                    msg.Body += "Comments: " + viewModel.Message;
                    //msg.IsBodyHtml = true;


                    // SMTP server settings
                    SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                    smtp.EnableSsl = true;
                    smtp.Credentials = new NetworkCredential("email", "password"); //valid email account & acount password

                    await smtp.SendMailAsync(msg);

                    return RedirectToAction("Contact");
                }
                catch (Exception)
                {
                    return View("Error");
                }


            }
            else
            {
                ModelState.AddModelError("", "There are errors in the form!");
                return View(viewModel);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
