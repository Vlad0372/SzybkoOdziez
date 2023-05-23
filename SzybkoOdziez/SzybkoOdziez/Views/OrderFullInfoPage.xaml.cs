using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Android.Media.TV;
using SzybkoOdziez.Models;
using System.Collections.ObjectModel;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace SzybkoOdziez.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderFullInfoPage : ContentPage
    {
        public ObservableCollection<Product> Products { get; set; }
        public OrderFullInfoPage(string orderId)
        {           
            Products = new ObservableCollection<Product>();

            ShowOrderProducts(orderId);
            InitializeComponent();
            PrintOrderData(orderId);

            orderProductsListView.ItemsSource = Products;
        }
        private void PrintOrderData(string orderId)
        {
            string connStr = "Data Source=(DESCRIPTION=" +
           "(ADDRESS=(PROTOCOL=TCP)(HOST=217.173.198.135)(PORT=1521))" +
           "(CONNECT_DATA=(SERVICE_NAME=tpdb)));" + "" +
           "User Id=s100824;Password=Sddb2023;";

            using (OracleConnection conn = new OracleConnection(connStr))
            {
                conn.Open();

                OracleCommand command = new OracleCommand();

                command.Connection = conn;               
                command.CommandText = "select * from \"order\" where order_id = " + orderId;
               
                OracleDataReader data = command.ExecuteReader();

                if (data.HasRows)
                {
                    data.Read();

                    f1.Text = data["order_id"].ToString();
                    f2.Text = data["total_price"].ToString() + " zł.";
                    f3.Text = data["payment_method"].ToString();
                    f4.Text = data["delivery_option"].ToString();
                    f5.Text = data["date"].ToString();
                    f6.Text = data["order_status"].ToString();                 
                }            
                else
                {
                    DisplayAlert("UPS...!", "Coś poszło nie tak", "Spróbuj ponownie");
                }

                conn.Close();
            }
        }
        private void ShowOrderProducts(string orderId)
        {
            var productsList = DependencyService.Get<IImgArrayGetterService>().GetProductListFromDBStreamAsync();

            string connStr = "Data Source=(DESCRIPTION=" +
           "(ADDRESS=(PROTOCOL=TCP)(HOST=217.173.198.135)(PORT=1521))" +
           "(CONNECT_DATA=(SERVICE_NAME=tpdb)));" + "" +
           "User Id=s100824;Password=Sddb2023;";

            using (OracleConnection conn = new OracleConnection(connStr))
            {
                conn.Open();
                OracleCommand subCommand = new OracleCommand();

                subCommand.Connection = conn;
                subCommand.CommandText = "select item_item_id from item_order where order_order_id = '" + orderId + "'";

                OracleDataReader subData = subCommand.ExecuteReader();

                var currentProducts = new ObservableCollection<Product>();

                if (subData.HasRows)
                {
                    while (subData.Read())
                    {
                        var currentProdInfo = productsList.Find(i => i.Id == Convert.ToInt32(subData["item_item_id"].ToString()));

                        var currentProd = new Product()
                        {
                            Id = currentProdInfo.Id,
                            Name = currentProdInfo.Name,
                            Description = currentProdInfo.Description,
                            ImageUrl = currentProdInfo.ImageUrl,
                            Price = currentProdInfo.Price,
                            TotalPrice = 0,
                            Comments = new List<Comment>(),
                        };

                        currentProducts.Add(currentProd);
                        Products.Add(currentProd);
                    }
                }
            }
        }
        private async void ReturnOrder_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Zwrot", "Dokonano zwrotu tego zamówienia", "Ok");
        }
    }
}