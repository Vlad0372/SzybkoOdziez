using Java.Util;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using SzybkoOdziez.Models;
using Xamarin.Forms;

namespace SzybkoOdziez.ViewModels
{
    public class ItemCarouselViewModel : BaseViewModel
    {
        public ObservableCollection<string> imageSources { get; set; }
        public int itemId = 0;
        public ItemCarouselViewModel()
        {
            imageSources = new ObservableCollection<string>();
            makeMockList();
        }

        public ItemCarouselViewModel(Product product)
        {
            imageSources = new ObservableCollection<string>();
            itemId = product.Id;
            LoadItemImageSources();
        }

        public void LoadItemImageSources()
        {
            this.imageSources.Clear();
            string placeholderImageName = "imagenotfound";
            List<string> imageSources = getItemImageSourcesFromDB(itemId);
            if(imageSources.Count < 3)
            {
                for (int i = imageSources.Count - 1; i < 3; i++)
                {
                    imageSources.Add(placeholderImageName);
                }
            }
            foreach(var item in imageSources)
            {
                this.imageSources.Add(item);
            }
        }

        public List<string> getItemImageSourcesFromDB(int item_id)
        {
            List<string> imageSources = new List<string>();
            var app = (App)Application.Current;
            var ConnectionString = app.connectionString;
            string queryString = "SELECT image_source FROM item_images WHERE item_id = :item_id";

            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                using (OracleCommand cmd = new OracleCommand(queryString, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("item_id", item_id));

                    try
                    {
                        conn.Open();
                        OracleDataReader reader = null;
                        reader = cmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                imageSources.Add(reader.GetString(0));
                            }
                        }
                        else
                        {
                            return new List<string>();
                        }
                        conn.Close();
                    }
                    catch (OracleException ex)
                    {
                        // wystąpił błąd Oracle - wyświetl komunikat o błędzie
                        Console.WriteLine(ex.ToString());
                        if (conn.State == System.Data.ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }

                }
            }
            return imageSources;
        }

        public void makeMockList()
        {
            this.imageSources.Clear();
            var list = new List<string> { "m_boots_item_1", "m_boots_item_2", "m_boots_item_3" };
            foreach (var item in list)
            {
                this.imageSources.Add(item);
            }
        }
    }
}
