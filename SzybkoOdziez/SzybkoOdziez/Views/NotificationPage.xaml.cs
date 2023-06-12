using Org.BouncyCastle.Asn1.Crmf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;
using SzybkoOdziez.ViewModels;
using Xamarin.Forms;
using static Android.Icu.Text.CaseMap;
using static Android.Renderscripts.Sampler;
using static Java.Util.Jar.Attributes;

namespace SzybkoOdziez.Views
{
    [QueryProperty(nameof(Title), "title")]
    [QueryProperty(nameof(Message), "message")]
    public partial class NotificationPage : ContentPage
    {
        private string title;
        private string message;
        public string Title
        {
            set
            {
                title = value;
            }
        }
        public string Message
        {
            set
            {
                message = value;
            }
        }
        public NotificationPage()
        {
            InitializeComponent();
    
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            stackLayout.Children.Clear();

            string msgText = "";

            if (title == null || message == null)
            {
                msgText = "Brak powiadomień";
            }
            else
            {
                msgText = $"Otrzymane powiadomienie:\n\n{title}\n\n{message}";
            }

            var msg = new Label()
            {
                Text = msgText,
                FontSize = 18,
                TextColor = Color.Black
            };

            stackLayout.Children.Add(msg);
        }
    }
}