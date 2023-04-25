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
    public partial class ProblemApplicationPage : ContentPage
    {
        public ProblemApplicationPage()
        {
            InitializeComponent();
        }

        private void send_message_problem_application_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new GetMessageAboutProblemApplicationPage());
            Navigation.RemovePage(this);
        }
    }
}