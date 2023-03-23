using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SzybkoOdziez.Views.koszykK
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Koszyk : ContentPage
    {
        public ObservableCollection<string> Items { get; set; }

        public Koszyk()
        {
            InitializeComponent();
          

        }

        private void Kliknienie_zamowienia(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Zatwierdzenie_zamowienie());
        }
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            TappedEventArgs tappedEventArgs = (TappedEventArgs)e;
            Produkt_informacje produkt_informacje = ((Informacje_daneym_produkcie)BindingContext).listaProduktu.
                Where(prod => prod.ProduktId == (int)tappedEventArgs.Parameter).FirstOrDefault();

            ((Informacje_daneym_produkcie)BindingContext).listaProduktu.Remove(produkt_informacje);
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}
