using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using SzybkoOdziez.Models;
using SzybkoOdziez.ViewModels;
using Xamarin.Forms;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SzybkoOdziez.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderCompletionPage : ContentPage
    {
        private OrderCompletionPage _viewModel;


       
        public OrderCompletionPage()
        {
            InitializeComponent();
            Random rnd = new Random();
            int orderNumber = rnd.Next(10000, 99999); // generuje losową liczbę z przedziału od 10000 do 99999

            // Przypisanie wartości do właściwości kontrolki w pliku .xaml
            orderNumberLabel.Text = orderNumber.ToString(); // zakładając, że kontrolka ma nazwę "orderNumberLabel"
        }

        private async void main_page_Clicked(object sender, EventArgs e)
        {
            //Application.Current.MainPage.Navigation.PushModalAsync(new MainPage(), true);
            //Navigation.PushAsync(new MainPage());
             await Shell.Current.GoToAsync("//MainPage");
             Navigation.RemovePage(this);

        }
        

    }
}