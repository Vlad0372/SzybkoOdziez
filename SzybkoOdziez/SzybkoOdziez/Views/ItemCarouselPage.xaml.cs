using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SzybkoOdziez.Models;
using SzybkoOdziez.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SzybkoOdziez.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemCarouselPage : ContentPage
    {
        private ItemCarouselViewModel _viewModel;
        public ItemCarouselPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new ItemCarouselViewModel();
        }

        public ItemCarouselPage(Product product)
        {
            InitializeComponent();
            BindingContext = _viewModel = new ItemCarouselViewModel(product);
        }
    }
}