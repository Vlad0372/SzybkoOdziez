using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SzybkoOdziez.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SzybkoOdziez.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class WatchlistPage : ContentPage
    {
        public WatchlistPage()
        {
            InitializeComponent();


            BindingContext = new ViewModels.WatchlistViewModel();
            
        }


        private void OnWishlistProductStackLayoutTapped(object sender, EventArgs e)
        {
            ChangePageToItemDescription();
        }


        private void ChangePageToItemDescription()
        {
            Navigation.PushAsync(new ItemDescriptionPage());
        }
    }


}