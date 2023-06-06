using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SzybkoOdziez.Models;
using SzybkoOdziez.ViewModels;
using SzybkoOdziez.Views;
using System.Collections.Generic;
using Acr.UserDialogs;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using static Android.App.DownloadManager;

namespace SzybkoOdziez.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderConfirmationPage : ContentPage
    {
        ShoppingCartViewModel _viewModel;

        INotificationManager notificationManager;
        public ObservableCollection<Product> Products { get; set; }
        private Order currentOrder { get; set; }
        int user_id;

        string connectionString = "Data Source=(DESCRIPTION=" +
               "(ADDRESS=(PROTOCOL=TCP)(HOST=217.173.198.135)(PORT=1521))" +
               "(CONNECT_DATA=(SERVICE_NAME=tpdb)));" + "" +
               "User Id=s100824;Password=Sddb2023;";

        public OrderConfirmationPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new ShoppingCartViewModel();

            currentOrder = new Order();

            notificationManager = DependencyService.Get<INotificationManager>();
            notificationManager.NotificationReceived += (sender, eventArgs) =>
            {
                var evtData = (NotificationEventArgs)eventArgs;
                ShowNotification(evtData.Title);
            };
            //List<Product> products = new List<Product>();//lista produktów
            //Products = new ObservableCollection<Product>(products);

            //// oblicz łączną wartość
            //decimal total = products.Sum(p => p.Price);

            //// ustaw tekst etykiety
            //TotalLabel.Text += total.ToString();

            //foreach (var item in products)
            //{
            //    Console.WriteLine("xD");
            //}
        }
        void ShowNotification(string title)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                DisplayAlert($"Informacja dotycząca zamówienia", "Poszło! Już mamy twoje zamówinie," +
                    " za niedługo dostaniesz szczegóły dotyczące terminu dostawy ;)", "OK");
            });
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnShoppingCartOpen();

            Products = _viewModel.Products;

            //=============== to Vladek dodal ===================== 
          
            var app = (App)Application.Current;
            var orderHistoryDataStore = app.orderHistoryDataStore;
            user_id = app.userId;

            int orderId = 0;
            Random rnd = new Random();

            if (orderHistoryDataStore != null && orderHistoryDataStore.Count() < 1) 
            {
                orderId = 0;
            }
            else
            {
                orderId = orderHistoryDataStore.GetLastItem().Id;
            }

            currentOrder = new Order
            {
                Id = orderId,
                Number = rnd.Next(10000, 99999).ToString(),
                CreatedDate = DateTime.Now.ToString("dd.MM.yyy"),
                TotalPrice = Products.Sum(p => p.Price),
                Products = Products
            };

            if (orderHistoryDataStore.CheckInDataStore(currentOrder))
            {
                await orderHistoryDataStore.AddItemAsync(currentOrder);  
            }
           
            //=============== to Vladek dodal ===================== 
            
            //// oblicz łączną wartość
  
            //// ustaw tekst etykiety
           totalPriceLabel.Text = currentOrder.TotalPrice.ToString() + " zł";
           

        }
        public ObservableCollection<string> Items { get; set; }

        private void complition_of_order_Clicked(object sender, EventArgs e)
        {
            var app = (App)Application.Current;
            var orderHistoryDataStore = app.orderHistoryDataStore;

            string title = $"Nowe powiadomenie od SzybkoOdzież";
            string message = $"Kliknij i zobacz, coś dla ciebie mamy!";
            notificationManager.SendNotification(title, message, DateTime.Now.AddSeconds(2));

            //Navigation.PushAsync(new OrderCompletionPage());
            Navigation.PushAsync(new OrderCompletionPage(currentOrder));
            //AddOrderToDB(currentOrder);
            Navigation.RemovePage(this);

        }
        private void paymentBLIK_CheckedChanged(object sender, EventArgs e)
        {
            if(BLIK_field.IsVisible == false)
            {
                BLIK_field.IsVisible = true;
            }
            else
            {
                BLIK_field.IsVisible = false;
            }
            
        }
        private void paymentCredit_Card_CheckedChanged(object sender, EventArgs e)
        {
            if (Credit_Card_field.IsVisible == false)
            {
                Credit_Card_field.IsVisible = true;
            }
            else
            {
                Credit_Card_field.IsVisible = false;
            }

        }
        private void AddOrderToDB(Order order)
        {
            var userId = 1;
            //================================
            var deliveryMethod = "";
            var paymentMethod = "";
            var totalPrice = totalPriceLabel.Text;
            var place = placeTxt.Text;
            var street = streetTxt.Text;
            var postalCode = postalCodeTxt.Text;
            var voivodship = voivodeshipTxt.Text;

            if (deliveryRadioBtn1.IsChecked)
            {
                deliveryMethod = deliveryRadioBtn1.Content.ToString();
            }
            else if(deliveryRadioBtn2.IsChecked)
            {
                deliveryMethod = deliveryRadioBtn2.Content.ToString();
            }
            else if(deliveryRadioBtn3.IsChecked)
            {
                deliveryMethod = deliveryRadioBtn2.Content.ToString();
            }

            if (paymentRadioBtn1.IsChecked)
            {
                paymentMethod = paymentRadioBtn1.Content.ToString();
            }
            else if (paymentRadioBtn2.IsChecked)
            {
                paymentMethod = paymentRadioBtn2.Content.ToString();
            }
            else if (paymentRadioBtn3.IsChecked)
            {
                paymentMethod = paymentRadioBtn3.Content.ToString();
            }
            //================================

            //int lastDeliveryDestId = GetLastID("DELIVERY_DESTINATION", "DESTINATION_ID", connectionString);

            //using (OracleConnection conn = new OracleConnection(connectionString))
            //{
            //    conn.Open();

            //    using (OracleCommand command = new OracleCommand())
            //    {
            //        command.Connection = conn;
            //        //INSERT INTO DELIVERY_DESTINATION(DESTINATION_ID, CITY, STREET, POSTAL_CODE, VOIVOIDSHIP, USER_USER_ID) VALUES(1, 'Katowice', 'Wrzosowa 7', 27-937, 'Śląsk', 1);
            //        //command.CommandText = "INSERT INTO observed (user_user_id, item_item_id) VALUES (:user_user_id, :item_item_id)";
            //        command.CommandText = "INSERT INTO DELIVERY_DESTINATION(DESTINATION_ID, CITY, STREET, POSTAL_CODE, VOIVOIDSHIP, USER_USER_ID) VALUES(:DESTINATION_ID, :CITY, :STREET, :POSTAL_CODE, :VOIVOIDSHIP, :USER_USER_ID)";



            //        string bruh = "bruh";
            //        lastDeliveryDestId++;

            //        //int defaultId = lastOrderId;
            //        //int customId = Convert.ToInt32(order.Number.ToString() + lastOrderId.ToString());

            //        command.Parameters.Add(new OracleParameter("DESTINATION_ID", 44));
            //        command.Parameters.Add(new OracleParameter("CITY", place));
            //        command.Parameters.Add(new OracleParameter("STREET", street));
            //        command.Parameters.Add(new OracleParameter("POSTAL_CODE", postalCode));
            //        command.Parameters.Add(new OracleParameter("VOIVOIDSHIP", voivodship));
            //        command.Parameters.Add(new OracleParameter("USER_USER_ID", 10424));


            //        //command.Parameters.Add(new OracleParameter("\"date\"", OracleDbType.Date));
            //        try
            //        {
            //            //conn.Open();
            //            int rowsAffected = command.ExecuteNonQuery();
            //            //conn.Close();

            //        }
            //        catch (OracleException ex)
            //        {
            //            // wystąpił błąd Oracle - wyświetl komunikat o błędzie
            //            DisplayAlert("UPS", "Cos poszlo nie tak", "Spróbuj ponownie", ex.Message);

            //        }
            //    }
            //}

            int lastOrderId = GetLastID("\"order\"", "order_id", connectionString);

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();

                //OracleCommand command = new OracleCommand();



                //OracleDataReader data = command.ExecuteNonQuery();
                using (OracleCommand command = new OracleCommand())
                {
                    command.Connection = conn;
                    //command.CommandText = "INSERT INTO observed (user_user_id, item_item_id) VALUES (:user_user_id, :item_item_id)";
                    command.CommandText = "INSERT INTO \"order\" (ORDER_ID, TOTAL_PRICE, PAYMENT_METHOD, DELIVERY_OPTION, DELIVERY_DESTINATION_ID, USER_USER_ID, \"date\", ORDER_STATUS) VALUES(:ORDER_ID, :TOTAL_PRICE, :PAYMENT_METHOD, :DELIVERY_OPTION, :DELIVERY_DESTINATION_ID, :USER_USER_ID, :\"date\", :ORDER_STATUS)";
                    //OracleCommand command = new OracleCommand(query, conn);

                    DateTime currentDate = DateTime.Now;

                    //command.Parameters.Add(new OracleParameter("user_user_id", user_id));
                    //command.Parameters.Add(new OracleParameter("item_item_id", item_id));
                    string bruh = "bruh";
                    lastOrderId++;

                    int defaultId = lastOrderId;
                    //int customId = Convert.ToInt32(order.Number.ToString() + lastOrderId.ToString());

                    command.Parameters.Add(new OracleParameter("ORDER_ID", defaultId));
                    command.Parameters.Add(new OracleParameter("TOTAL_PRICE", order.TotalPrice));
                    command.Parameters.Add(new OracleParameter("PAYMENT_METHOD", bruh));
                    command.Parameters.Add(new OracleParameter("DELIVERY_OPTION", bruh));
                    command.Parameters.Add(new OracleParameter("DELIVERY_DESTINATION_ID", 44));
                    command.Parameters.Add(new OracleParameter("USER_USER_ID", user_id));
                    command.Parameters.Add(new OracleParameter("\"date\"", currentDate));
                    command.Parameters.Add(new OracleParameter("ORDER_STATUS", bruh));

                    //command.Parameters.Add(new OracleParameter("\"date\"", OracleDbType.Date));
                    try
                    {
                        //conn.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        //conn.Close();

                    }
                    catch (OracleException ex)
                    {
                        // wystąpił błąd Oracle - wyświetl komunikat o błędzie
                        DisplayAlert("UPS", "Cos poszlo nie tak", "Spróbuj ponownie", ex.Message);

                    }
                }
            }
        }
        private int GetLastID(string tableName, string idFieldName, string connStr)
        {
            string lastID = "0";

            OracleConnection connection = new OracleConnection(connStr);
            connection.Open();
            OracleCommand command = new OracleCommand();

            command.Connection = connection;
            command.CommandText = "select max(" + tableName + "." + idFieldName + ") from " + tableName;
            command.CommandType = CommandType.Text;

            object val = command.ExecuteScalar();

            if (val != null)
            {
                lastID = val.ToString();

                if (lastID == "") { lastID = "0"; }
            }
            else
            {
                lastID = "0";
            }

            connection.Dispose();

            return Convert.ToInt32(lastID);
        }

        private void paymentRadioBtn1_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {

        }

        
    }
}
