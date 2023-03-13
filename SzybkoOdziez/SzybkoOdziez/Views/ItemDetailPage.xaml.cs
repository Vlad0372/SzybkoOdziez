using System.ComponentModel;
using SzybkoOdziez.ViewModels;
using Xamarin.Forms;

namespace SzybkoOdziez.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}