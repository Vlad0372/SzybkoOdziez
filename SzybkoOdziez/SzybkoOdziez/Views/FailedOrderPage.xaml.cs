using Android.Icu.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SzybkoOdziez.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FailedOrderPage : ContentPage
    {
        public FailedOrderPage()
        {
            InitializeComponent();
        }

        public void RepordOrderProblem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new GetMessageAboutProblemApplicationPage());
            Navigation.RemovePage(this);

            editor.Text = "";
            orderNumberEntry.Text = "";

            OrderProblem1.IsChecked = false;
            OrderProblem2.IsChecked = false;
            OrderProblem3.IsChecked = false;            
        }
    }
}