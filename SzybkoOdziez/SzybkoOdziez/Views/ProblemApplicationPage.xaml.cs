using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using MimeKit;
using MailKit.Net.Smtp;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Acr.UserDialogs;
using Org.Apache.Http;

namespace SzybkoOdziez.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProblemApplicationPage : ContentPage
    {
        bool guestMode = true;

        public ProblemApplicationPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var app = (App)Application.Current;
            guestMode = app.guestMode;
        }

        private void send_message_problem_application_Clicked(object sender, EventArgs e)
        {
            //SendEmail("szybkoodziezgetreports@gmail.com", getSelectedRadioButtonContent(), entryReportBody.Text);
            //SendEmailMK("szybkoodziezgetreports@gmail.com", getSelectedRadioButtonContent(), entryReportBody.Text);
            //if (guestMode)
            //{
            //    DisplayAlert("Nie wysłano zgłoszenia!", "By wysłać zgłoszenie, trzeba się zalogować!", "Ok");
            //}
            Navigation.PushAsync(new GetMessageAboutProblemApplicationPage());
            Navigation.RemovePage(this);

        }

        private void ClearSelection_clicked(object sender, EventArgs e)
        {
            AppProblem1.IsChecked = false;
            AppProblem2.IsChecked = false;
            AppProblem3.IsChecked = false;
            AppProblem4.IsChecked = false;
        }

        private string getSelectedRadioButtonContent()
        {
            string radioContent = "";
            if (AppProblem1.IsChecked)
            {
                radioContent = AppProblem1.Content.ToString();
            }
            else if (AppProblem2.IsChecked)
            {
                radioContent = AppProblem2.Content.ToString();
            }
            else if (AppProblem3.IsChecked)
            {
                radioContent = AppProblem3.Content.ToString();
            }
            else if (AppProblem4.IsChecked)
            {
                radioContent = AppProblem4.Content.ToString();
            }
            return radioContent;
        }

        //private void SendEmail(string recipientEmail, string subject, string body)
        //{
        //    try
        //    {
        //        // Create a new SmtpClient with Gmail's SMTP server settings
        //        SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
        //        smtpClient.EnableSsl = true;
        //        smtpClient.UseDefaultCredentials = false;
        //        smtpClient.Credentials = new NetworkCredential("szybkoodziezreporterrors@gmail.com", "report123!");

        //        // Create a new MailMessage with the sender, recipient, subject, and body
        //        MailMessage mailMessage = new MailMessage();
        //        mailMessage.From = new MailAddress("szybkoodziezreporterrors@gmail.com");
        //        mailMessage.To.Add(recipientEmail);
        //        mailMessage.Subject = subject;
        //        mailMessage.Body = body;

        //        // Send the email
        //        smtpClient.Send(mailMessage);

        //        // Display a success message
        //        UserDialogs.Instance.Toast("Zgłoszenie wysłano pomyślnie!", TimeSpan.FromSeconds(2));
        //    }
        //    catch (SmtpException ex)
        //    {
        //        // Handle any SMTP-related exceptions
        //        DisplayAlert("Error sending the email", ex.Message, "Ok");
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle any other exceptions
        //        DisplayAlert("Error sending the email", ex.Message, "Ok");
        //    }
        //}

        private void SendEmailMK(string recipientEmail, string subject, string body)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("", "szybkoodziezreports@outlook.com"));
                message.To.Add(new MailboxAddress("", recipientEmail));
                message.Subject = subject;

                var bodyBuilder = new BodyBuilder();
                bodyBuilder.TextBody = body;

                message.Body = bodyBuilder.ToMessageBody();

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    //client.Connect("smtp.gmail.com", 587, true);

                    //client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                    //client.Connect("smtp.office365.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                    client.Connect("smtp-mail.outlook.com", 587, MailKit.Security.SecureSocketOptions.StartTls);

                    //Use your Gmail account credentials
                    //client.Authenticate("szybkoodziezreporterrors@gmail.com", "report123!");
                    client.Authenticate("szybkoodziezreports@outlook.com", "report123!");

                    client.Send(message);
                    client.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending email: " + ex.Message);
            }
        }
    }
}