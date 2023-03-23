using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SzybkoOdziez.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Zatwierdzenie_zamowienie : ContentPage
    {
        public ObservableCollection<string> Items { get; set; }

        public Zatwierdzenie_zamowienie()
        {
            InitializeComponent();

        }
    }
}
