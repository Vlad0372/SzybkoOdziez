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
            if (_product != null)
            {
                priceLabel.Text = Convert.ToString(_product.Price)+" zł";
            }
        }

        public async void OnLabelCommentsTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProductCommentsPage(_product));  
        }
    }
}