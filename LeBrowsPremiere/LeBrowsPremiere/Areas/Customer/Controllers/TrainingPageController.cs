using LeBrowsPremiere.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;

namespace LeBrowsPremiere.Areas.Customer.Controllers
{
    public class TrainingPageController : Controller
    {
        public IActionResult Training()
        {
            return View();
        }

        [HttpPost()]
        public async Task<IActionResult> Training(TrainingViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    MailMessage msg = new MailMessage();

                    // Sender and recipient address
                    msg.From = new MailAddress("email", "LeBrows Premiere");
                    msg.To.Add(new MailAddress(viewModel.Email, viewModel.FirstName + " " + viewModel.LastName + " "+viewModel.CountryofResidence));

                    // Message subject and body
                    msg.Subject = $"Le Brow's Training";
                    msg.Body = "First Name: " + viewModel.FirstName;
                    msg.Body += "Last Name: " + viewModel.LastName;
                    msg.Body += "Email: " + viewModel.Email;
                    msg.Body += "Country of Residence" + viewModel.CountryofResidence;

                    //msg.IsBodyHtml = true;


                    // SMTP server settings
                    SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                    smtp.EnableSsl = true;
                    smtp.Credentials = new NetworkCredential("email", "password"); //valid email account & acount password

                    await smtp.SendMailAsync(msg);

                    TempData["success"] = "Thank you for your interest! We will get back to you shortly!";

                    return RedirectToAction("Training");
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
    }
}
