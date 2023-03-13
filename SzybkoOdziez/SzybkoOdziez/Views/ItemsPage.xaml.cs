using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SzybkoOdziez.Models;
using SzybkoOdziez.ViewModels;
using SzybkoOdziez.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SzybkoOdziez.Views
{
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel _viewModel;

        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new ItemsViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}