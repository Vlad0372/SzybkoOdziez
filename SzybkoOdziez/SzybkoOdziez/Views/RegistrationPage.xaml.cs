using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using SzybkoOdziez.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SzybkoOdziez.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrationPage : ContentPage
    {
        public RegistrationPage()
        {
            InitializeComponent();
        }
    }
}