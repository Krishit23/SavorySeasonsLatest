using Microsoft.AspNetCore.Mvc;
using SavorySeasons.Backend.Models;
using System.Net.Mail;
using System.Net;
using SavorySeasons.Controllers;

namespace SavorySeasons.Backend.Controllers
{
    public class ContactController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ContactController(
            IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        [Route("api/contact")]
        public async Task<ActionResult> Post([FromBody] ContactUs contact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var html = await System.IO.File.ReadAllTextAsync(Path.Combine(_webHostEnvironment.WebRootPath, "Template/ContactUs.html"));

            // Replace placeholders in the HTML template
            html = html.Replace("##YouTube##", $"{Request.Scheme}://{Request.Host}/common/youtube.png");
            html = html.Replace("##Whatsapp##", $"{Request.Scheme}://{Request.Host}/common/whatsapp.png");
            html = html.Replace("##Twitter##", $"{Request.Scheme}://{Request.Host}/common/twitter.png");
            html = html.Replace("##Linkedin##", $"{Request.Scheme}://{Request.Host}/common/linkedin.png");
            html = html.Replace("##CompanyLogo##", $"{Request.Scheme}://{Request.Host}/common/email-logo.png");

            html = html.Replace("##Name##", $"{contact.FirstName} {contact.LastName}");
            html = html.Replace("##Email##", contact.Email);
            html = html.Replace("##MobileNumber##", $"{contact.MobileNumber}");
            html = html.Replace("##Message##", contact.Message);

            string senderEmail = "savoryseasons06@gmail.com";
            string senderPassword = "njebdmrvrilxzevp";
            string receiverEmail = "savoryseasons06@gmail.com";

            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(senderEmail);
                    mail.To.Add(receiverEmail);
                    mail.Subject = "Contact Us Form Submission";
                    mail.Body = html;
                    mail.IsBodyHtml = true;

                    using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.EnableSsl = true;
                        smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
                        await smtpClient.SendMailAsync(mail);
                    }
                }

                return Ok("Email sent successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}