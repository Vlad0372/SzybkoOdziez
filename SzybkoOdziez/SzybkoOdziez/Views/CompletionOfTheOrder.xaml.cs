using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SzybkoOdziez.Models;
using SzybkoOdziez.ViewModels;
using Xamarin.Forms;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SzybkoOdziez.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CompletionOfTheOrder : ContentPage
    {
        private CompletionOfTheOrder _viewModel;

        public CompletionOfTheOrder()
        {
            InitializeComponent();
           
        }

        private void main_page_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MainPage());
        }

        private void random_number_Clicked(object sender, EventArgs e)
        {
            Random random = new Random();
            int numerZamowienia = random.Next(1000, 10000);
            Console.WriteLine("Twój numer to: " + numerZamowienia);

        }
    }
}