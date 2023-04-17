using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SzybkoOdziez.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SzybkoOdziez.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemDescriptionPage : ContentPage
    {
        private Product _product = new Product();

        public ItemDescriptionPage()
        {
            InitializeComponent();
        }
        public ItemDescriptionPage(Product product)
        {
            InitializeComponent();
            _product = product;
        }

        public void OnLabelCommentsTapped(object sender, EventArgs e)
        {
            //Navigation.PushAsync(new ProductCommentsPage(_product));
            //Navigation.PushAsync(new TESTPAGE(_product));
            Navigation.PushAsync(new TESTPAGE(_product));
        }
    }
}