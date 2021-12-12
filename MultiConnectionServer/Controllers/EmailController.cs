using Microsoft.AspNetCore.Mvc;
using MultiConnectionServer.Entities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace MultiConnectionServer.Controllers
{
     [ApiController]
     [Route("[controller]")]
     public class EmailController : ControllerBase
     {

          [HttpPost("/smtp/gmail")]
          public HttpStatusCode SendSMTPEmail(Email email)
          {

               try
               {
                    MailMessage message = new(email.From, email.To);

                    message.Subject = email.Subject;
                    message.Body = email.Message;
                    message.BodyEncoding = Encoding.UTF8;
                    message.IsBodyHtml = true;
                    SmtpClient client = new("smtp.gmail.com", 587); //Gmail smtp    
                    client.EnableSsl = true;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(email.From, email.Password);

                    client.Send(message);
                    return HttpStatusCode.OK;
               }
               catch(Exception e)
               {
                    HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return HttpStatusCode.BadRequest;
               }
          }
     }

}
