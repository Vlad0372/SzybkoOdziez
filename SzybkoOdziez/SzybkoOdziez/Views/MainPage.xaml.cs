using Android.Media;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using SzybkoOdziez.Models;
using Xamarin.Forms;
using static Android.Provider.ContactsContract.CommonDataKinds;
using static Java.Util.Jar.Attributes;

namespace SzybkoOdziez.Views
{
    public partial class MainPage : ContentPage
    {
        private List<Product> productsList = new List<Product>();
        private Product _currentProduct { get; set; }

        private string ItemCategory = "Wszystkie";
        public MainPage()
        {
            InitializeComponent();
            //InitProductInfoList();
            InitProductInfoListFromDB();

            _currentProduct = GetRandProductInfo();
            SetCurrentProduct(_currentProduct);
        }

        private void ShowMore(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ItemDescriptionPage(_currentProduct));
        }
        private async void FilterButtonClick(object sender, EventArgs e)
        {
             ItemCategory = await DisplayActionSheet("Filtruj", "Anuluj", null, "Wszystkie","Bielizna", "Bluza", "Buty", "Czapka", "Koszula", "Kurtka", "Marynarka", "Paski", "Rękawice", "Spodnie", "Spódnice", "Szalik", "T-shirt");
        }
        private async void OnDislikeClicked(object sender, EventArgs args)
        {
            _currentProduct = GetRandProductInfo();
            SetCurrentProduct(_currentProduct);

            await dislikeButton.ScaleTo(0.75, 100);
            await dislikeButton.ScaleTo(1, 100);        
        }

        private async void OnLikeClicked(object sender, EventArgs args)
        {
            AddProductToUserObserved(99, _currentProduct.Id);

            _currentProduct = GetRandProductInfo();
            SetCurrentProduct(_currentProduct);

            await likeButton.ScaleTo(0.75, 100);
            await likeButton.ScaleTo(1, 100);         
        }

        private void AddProductToUserObserved(int user_id, int item_id)
        {
            string ConnectionString = "Data Source=(DESCRIPTION=" +
           "(ADDRESS=(PROTOCOL=TCP)(HOST=217.173.198.135)(PORT=1521))" +
           "(CONNECT_DATA=(SERVICE_NAME=tpdb)));" + "" +
           "User Id=s100824;Password=Sddb2023;";
            OracleConnection connection = new OracleConnection(ConnectionString);
            connection.Open();

            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                var query = "INSERT INTO observed (user_user_id, item_item_id) VALUES (:user_user_id, :item_item_id)";

                using (OracleCommand cmdInsert = new OracleCommand(query, conn))
                {
                    OracleCommand command = new OracleCommand(query, connection);
                    command.Parameters.Add(new OracleParameter("user_user_id", user_id));
                    command.Parameters.Add(new OracleParameter("item_item_id", item_id));

                    try
                    {
                        conn.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        conn.Close();

                    }
                    catch (OracleException ex)
                    {
                        // wystąpił błąd Oracle - wyświetl komunikat o błędzie
                        DisplayAlert("UPS", "Cos poszlo nie tak", "Spróbuj ponownie", ex.Message);

                    }
                }
            }
        }

        private void SetCurrentProduct(Product product)
        {
            productUrl.Source = product.ImageUrl;
            productName.Text = product.Name;
            productDesc.Text = product.Description;
            productPrice.Text = product.Price.ToString() + " zł";
        }

        //
        // PONIZEJ SA DWIE NIEUZYWANE METODY (InitProductInfoList() i GetRandImgPath() NIE WIEM CZY USUNAC uwu
        //
        //private void InitProductInfoList()
        //{
        //    List<string> imgsNameList = DependencyService.Get<IImgArrayGetterService>().GetImgArrayStreamAsync();
            
        //    for (int i = 0; i < imgsNameList.Count; i++)
        //    {
        //        var currentProduct = new Product();

        //        currentProduct.Id = i;
        //        currentProduct.Name = "prod_name_" + i;
        //        currentProduct.Description = "disc_" + i;
        //        currentProduct.ImageUrl = "@drawable/" + imgsNameList[i] + ".jpg";
        //        //currentProduct.Price = ((i + 1 * 100 % 15) * 10).ToString() + ",00 zł";
        //        currentProduct.Price = (i + 1 * 100 % 15) * 10;

        //        productsList.Add(currentProduct);
        //    }
        //}
        //
        //private string GetRandImgPath()
        //{
        //    var random = new System.Random();
            
        //    List<string> imgsNameList = DependencyService.Get<IImgArrayGetterService>().GetImgArrayStreamAsync();

        //    int index = random.Next(imgsNameList.Count);
            
        //    return  "@drawable/" + imgsNameList[index] + ".jpg";
        //}

        private Product GetRandProductInfo()
        {
            var random = new System.Random();

            int index = random.Next(productsList.Count);
            if(ItemCategory == "Wszystkie")
            {
                return productsList[index];
            }
            else
            {
                while (productsList[index].Category != ItemCategory)
                {
                    index = random.Next(productsList.Count);
                }
                return productsList[index];
            }
            
        }

        private void InitProductInfoListFromDB()
        {
            productsList = DependencyService.Get<IImgArrayGetterService>().GetProductListFromDBStreamAsync();
        }
    }
}