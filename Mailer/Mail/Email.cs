using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SendGrid;
using System.Net.Mail;

namespace Mailer.Mail
{
    class Email
    {
        private bool issfs = true;
        public void send()
        {
#if Debug

#endif
            if (issfs)
            {
                var myMessage = new SendGridMessage();
                // Add the message properties.
                myMessage.From = new MailAddress("paul.salvi@returnonweb.com");

                // Add multiple addresses to the To field.
                List<String> recipients = new List<String>
                {
                         @"Jeff Smith <plslv1991@gmail.com>",
                         @"Anna Lidman <anupama.kamble@returnonweb.com>"
                };

                myMessage.AddTo(recipients);

                myMessage.Subject = "Testing the SendGrid Library";

                //Add the HTML and Text bodies
                myMessage.Html = "<p>Hello World!</p>";
                myMessage.Text = "Hello World plain text!";


                var transportWeb = new Web("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");
                transportWeb.DeliverAsync(myMessage);
                issfs = false;
            }


        }

    }
}
