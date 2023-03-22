using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace SzybkoOdziez
{
    public class Informacje_daneym_produkcie
    {
        public ObservableCollection<Produkt_informacje> listaProduktu { get; set; }

        public Informacje_daneym_produkcie()
        {
            listaProduktu = new ObservableCollection<Produkt_informacje>();
            listaProduktu.Add(new Produkt_informacje() { ProduktId = 1, NazwaProduktu = "Bluza", Cena = "250 zł", ImageUrl = "bluza.jpg" });
            listaProduktu.Add(new Produkt_informacje() { ProduktId = 2, NazwaProduktu = "Czapka", Cena = "15 zł", ImageUrl = "czapka.jpg" });
            listaProduktu.Add(new Produkt_informacje() { ProduktId = 3, NazwaProduktu = "Buty", Cena = "100 zł", ImageUrl = "buty.jpg" });
            listaProduktu.Add(new Produkt_informacje() { ProduktId = 4, NazwaProduktu = "Dresy", Cena = "200 zł", ImageUrl = "dresy.jpg" });
            listaProduktu.Add(new Produkt_informacje() { ProduktId = 5, NazwaProduktu = "Kurtka", Cena = "500 zł", ImageUrl = "kurtka.jpg" });
        }
    }
}
