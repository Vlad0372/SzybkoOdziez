using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SzybkoOdziez.Models;
using SzybkoOdziez.Services;
using SzybkoOdziez.ViewModels;

namespace SzybkoOdziez.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderReturnPage : ContentPage
    {
        private Order _currentOrder;

        public OrderReturnPage()
        {
            InitializeComponent();
        }
        public OrderReturnPage(Order order)
        {
            InitializeComponent();
            
        }

        private void ButtonZwrot_Clicked(object sender, EventArgs e)
        {

        }
    }
}