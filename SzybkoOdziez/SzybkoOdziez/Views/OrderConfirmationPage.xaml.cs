﻿using System;
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
using Android.Locations;
using static Android.Provider.ContactsContract.CommonDataKinds;
using static Java.Util.Jar.Attributes;
using Xamarin.Essentials;
using Android.Icu.Util;

namespace SzybkoOdziez.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderConfirmationPage : ContentPage
    {
        INotificationManager notificationManager;

        ShoppingCartViewModel _viewModel;
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

            //Vladek tu powiadominia robi
            notificationManager = DependencyService.Get<INotificationManager>();
            notificationManager.NotificationReceived += (sender, eventArgs) =>
            {
                var evtData = (NotificationEventArgs)eventArgs;
                ShowNotification(evtData.Title, evtData.Message);
            };
            //Vladek tu powiadominia robi

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
                CreatedDate = DateTime.Now.ToString("dd/MM/yyyy"),
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
            var place = placeTxt.Text;
            var street = streetTxt.Text;
            var postalCode = postalCodeTxt.Text;
            var voivodship = voivodeshipTxt.Text;


            if (!paymentRadioBtn1.IsChecked && !paymentRadioBtn2.IsChecked && !paymentRadioBtn3.IsChecked)
            {
                DisplayAlert("Błąd", "Proszę zaznaczyć jedną z opcji płatności!.", "OK");
                return;
            }

            else if (!deliveryRadioBtn1.IsChecked && !deliveryRadioBtn2.IsChecked && !deliveryRadioBtn3.IsChecked)
            {
                DisplayAlert("Błąd", "Proszę zaznaczyć jedną z opcji dostawy!.", "OK");
                return;
            }

            else if (string.IsNullOrEmpty(place) || string.IsNullOrEmpty(street) || string.IsNullOrEmpty(postalCode) || string.IsNullOrEmpty(voivodship))
            {
                // Wyświetl komunikat o błędzie
                DisplayAlert("Błąd", "Wprowadź wymagane dane.", "OK");
                return; // Zatrzymaj dalsze przetwarzanie
            }

            var app = (App)Application.Current;
            var orderHistoryDataStore = app.orderHistoryDataStore;

            //Vladek tu powiadominia robi
            try
            {
                string title = "Otrzymaliśmy Twoje zamówienie";
                string message = "Za niegługo otrzymasz dodatkową informacje dotyczącą czasu dostawy :)";
                notificationManager.SendNotification(title, message, DateTime.Now.AddSeconds(3));
            }
            catch (Exception ex)
            {
                DisplayAlert("Powiadomienie", "Wygląda na to, że nie zezwoliłeś aplikacji na wysyłania wiadomości, " +
                    "zmień to w ustawieniach urządzenia i spróbuj ponownie, jak problem się powtórzy, zgłoś o problemie poprzez" +
                    " formularz zgłoszeniowy", "OK");
            }
            //Vladek tu powiadominia robi

            //Navigation.PushAsync(new OrderCompletionPage());
            Navigation.PushAsync(new OrderCompletionPage(currentOrder));
            //AddOrderToDB(currentOrder);
            Navigation.RemovePage(this);

            int lastDeliveryDestinationId = GetLastID("delivery_destination", "destination_id");
            lastDeliveryDestinationId++;

            DeliveryDestination currentDeliveryDestination = new DeliveryDestination
            {
                Id = lastDeliveryDestinationId,
                City = place,
                PostalCode = postalCode,
                Street = street,
                Voivodship = voivodship,
                UserId = app.userId
            };

            //IsertDevileryDestinationToDB(currentDeliveryDestination);
            IsertOrderToDB(currentOrder);
            int lastOrderId = GetLastID("\"order\"", "order_id");
            InsertItemOrderToDb(lastOrderId, currentOrder);
        }

        private void IsertDevileryDestinationToDB(DeliveryDestination deliveryDestination)
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    conn.Open();

                    var query = "INSERT INTO delivery_destination (destination_id, city, street, postal_code, voivoidship, user_user_id) VALUES (:destination_id, :city, :street, :postal_code, :voivoidship, :user_user_id)";

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        cmd.Parameters.Add(new OracleParameter("destination_id", deliveryDestination.Id));
                        cmd.Parameters.Add(new OracleParameter("city", deliveryDestination.City));
                        cmd.Parameters.Add(new OracleParameter("street", deliveryDestination.Street));
                        cmd.Parameters.Add(new OracleParameter("postal_code", deliveryDestination.PostalCode));
                        cmd.Parameters.Add(new OracleParameter("voivoidship", deliveryDestination.Voivodship));
                        cmd.Parameters.Add(new OracleParameter("user_user_id", deliveryDestination.UserId));

                        int rowsAffected = cmd.ExecuteNonQuery();
                        conn.Close();

                        if (rowsAffected < 0)
                        {
                            DisplayAlert("UPS", "KOKOJAMBA!", "Spróbuj ponownie!");
                        }
                    }
                }
            }
            catch (OracleException ex)
            {
                DisplayAlert("UPS", "KOKOJAMBA!", "Spróbuj ponownie");
            }
        }
        private void IsertOrderToDB(Order order)
        {
            try
            {
                var app = (App)Application.Current;

                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    conn.Open();

                    var query = "INSERT INTO \"order\" (order_id, total_price, payment_method, delivery_option, delivery_destination_id, user_user_id, \"date\", order_status) VALUES (:order_id, :total_price, :payment_method, :delivery_option, :delivery_destination_id, :user_user_id, :\"date\", :order_status)";
                    
                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        int order_id = GetLastID("\"order\"", "order_id");
                        order_id++;
                        string total_price = order.TotalPrice.ToString();
                        string payment_method = "BLIK";
                        string delivery_option = "kurier poczta polska";
                        int delivery_destination_id = 2;
                        int user_user_id = app.userId;
                        string order_status = "realizowane";

                        cmd.Parameters.Add(new OracleParameter("order_id", order_id));
                        cmd.Parameters.Add(new OracleParameter("total_price", total_price));
                        cmd.Parameters.Add(new OracleParameter("payment_method", payment_method));
                        cmd.Parameters.Add(new OracleParameter("delivery_option", delivery_option));
                        cmd.Parameters.Add(new OracleParameter("delivery_destination_id", delivery_destination_id));
                        cmd.Parameters.Add(new OracleParameter("user_user_id", user_user_id));
                        cmd.Parameters.Add(new OracleParameter("\"date\"", OracleDbType.Date));
                        cmd.Parameters["\"date\""].Value = DateTime.Now;
                        cmd.Parameters.Add(new OracleParameter("order_status", order_status)); 

                        int rowsAffected = cmd.ExecuteNonQuery();
                        conn.Close();

                        if (rowsAffected < 0)
                        {
                            DisplayAlert("UPS", "KOKOJAMBA!", "Spróbuj ponownie!");
                        }
                    }
                }
            }
            catch (OracleException ex)
            {
                DisplayAlert("UPS", "KOKOJAMBA!", "Spróbuj ponownie");
            }       
        }
        private void InsertItemOrderToDb(int lastOrderId, Order order)
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    conn.Open();

                    var query = "INSERT INTO item_order (item_item_id, order_order_id) VALUES (:item_item_id, :order_order_id)";
                    
                    for (int i = 0; i < order.Products.Count; i++)
                    {
                        using (OracleCommand cmd = new OracleCommand(query, conn))
                        {   
                       
                            int item_item_id = order.Products[i].Id;

                            cmd.Parameters.Add(new OracleParameter("item_item_id", item_item_id));
                            cmd.Parameters.Add(new OracleParameter("order_order_id", lastOrderId));

                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected < 0)
                            {
                                DisplayAlert("UPS", "KOKOJAMBA!", "Spróbuj ponownie!");
                            }
                        
                         
                        
                        }
                    }
                    conn.Close();
                }
            }
            catch (OracleException ex)
            {
                DisplayAlert("UPS", "KOKOJAMBA!", "Spróbuj ponownie");
            }
        }
        private int GetLastID(string tableName, string idFieldName)
        {
            string lastID = "0";

            OracleConnection connection = new OracleConnection(connectionString);
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

        void ShowNotification(string title, string message)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Shell.Current.GoToAsync($"//MainPage//NotificationPage?title={title}&message={message}");
            });
        }
    }
}
