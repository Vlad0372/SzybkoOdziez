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
    public partial class GetMessageAboutProblemApplicationPage : ContentPage
    {
        public GetMessageAboutProblemApplicationPage()
        {
            InitializeComponent();
        }

        private async void return_main_page_clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//MainPage");
            Navigation.RemovePage(this);
        }
    }
}