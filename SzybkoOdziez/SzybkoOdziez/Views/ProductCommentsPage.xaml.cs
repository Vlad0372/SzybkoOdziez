using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SzybkoOdziez.Services;
using SzybkoOdziez.Models;
using SzybkoOdziez.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Android.Provider;
using System.Windows;
using Acr.UserDialogs;
using Xamarin.Essentials;
using Android.Graphics;
using System.IO;
using static Java.Util.Jar.Attributes;
using Oracle.ManagedDataAccess.Client;
using Java.Nio.Channels;
using static Java.Text.Normalizer;

namespace SzybkoOdziez.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductCommentsPage : ContentPage
    {
        private ProductCommentsViewModel _viewModel;
        private Product _product;
        private string _fileFullPath;
        int user_id;
        bool guestMode = true;
        //private ObservableCollection<Comment> _comments;


        public ProductCommentsPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new ViewModels.ProductCommentsViewModel();
        }
        public ProductCommentsPage(Product product)
        {
            InitializeComponent();
            BindingContext = _viewModel = new ViewModels.ProductCommentsViewModel();

            if (product.Comments == null) { product.Comments = new List<Comment>(); }
            _product = product;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var app = (App)Application.Current;
            user_id = app.userId;
            guestMode = app.guestMode;
            _viewModel.OnProductCommentsOpen(_product);
        }

        public async void OnAddCommentButtonClicked(object sender, EventArgs args)
        {
            if (guestMode)
            {
                await DisplayAlert("Nie dodano komentarza!", "Proszę się zalogować, by móc dodawać komentarze!", "Ok");
            }
            else
            {
                Comment comment = new Comment();
                comment.Title = CommentTitleEntry.Text;
                comment.Content = CommentTextEditor.Text;
                int newcommentDBID = AddCommentToDB(comment);
                comment.DBId = newcommentDBID;
                comment.UserId = user_id;
                comment.ProductId = _product.Id;

                _viewModel.Comments.Add(comment);
            }
            
            //if(_fullPath != null)
            //{
            //    comment.CommentImageSource = _fullPath;
            //}

            //if (CheckIfCommentUnique(comment))
            //{
            //    _product.Comments.Add(comment);
            //    _viewModel.OnProductCommentsOpen(_product);
            //    _fullPath = null;
            //}
            //else
            //{
            //    await DisplayAlert("Duplikat!", "Komentarz o takim tytule już istnieje!", "Ok");
            //}
        }

        public bool CheckIfCommentUnique(Comment comment)
        {
            bool isUnique = true;
            foreach (Comment productcomment in _product.Comments)
            {
                if (productcomment.Title == comment.Title)
                {
                    isUnique = false;
                }
            }
            return isUnique;
        }

        public int AddCommentToDB(Comment comment)
        {
            var app = (App)Application.Current;
            string ConnectionString = app.connectionString;
            string queryStringInsert = "";
            string queryStringSelectMaxID = "SELECT MAX(comment_id) FROM comments";
            byte[] imageData = null; 
            if (_fileFullPath == null)
            {
                queryStringInsert = "INSERT INTO comments (comment_id, title, content, item_item_id, user_user_id)"
                                        + "VALUES (:db_id, :title, :content, :item_id, :user_id)";
            }
            else
            {
                queryStringInsert = "INSERT INTO comments (comment_id, title, content, item_item_id, user_user_id, comment_image)"
                                    + "VALUES (:db_id, :title, :content, :item_id, :user_id, :comment_image)";
                imageData = File.ReadAllBytes(_fileFullPath);
            }

            int newcommentDBID = new int();
            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                using (OracleCommand cmd = new OracleCommand(queryStringSelectMaxID, conn))
                {
                    try
                    {
                        conn.Open();
                        OracleDataReader reader = cmd.ExecuteReader();
                        reader.Read();
                        newcommentDBID = reader.GetInt32(0) + 1;
                        conn.Close();
                   
                    }
                    catch(OracleException ex)
                    {
                        Console.WriteLine(ex.ToString());
                        if (conn.State == System.Data.ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }
                }
            }
            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                using (OracleCommand cmd = new OracleCommand(queryStringInsert, conn))
                {
                    try
                    {
                        cmd.Parameters.Add("db_id", newcommentDBID);
                        cmd.Parameters.Add("title", comment.Title);
                        cmd.Parameters.Add("content", comment.Content);
                        cmd.Parameters.Add("item_id", _product.Id);
                        cmd.Parameters.Add("user_id", comment.UserId);
                        if (_fileFullPath != null)
                        {
                            cmd.Parameters.Add(":comment_image", OracleDbType.Blob).Value = imageData;
                        }

                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        conn.Close();

                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("Insert succesful");
                        }
                        else { Console.WriteLine("Insert unsuccesful!"); }
                    }
                    catch(OracleException ex)
                    {
                        Console.WriteLine(ex.ToString());
                        if(conn.State == System.Data.ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }
                }
            }
            return newcommentDBID;
        }



        public async void ButtonSelectImage_Clicked(object sender, EventArgs args)
        {
            //zdjecia komentarzy nie sa nigdzie zapisywane, jedynie zapisywana jest pelna sciezka do zdjecia na urzadzeniu
            try
            {
                var result = await MediaPicker.PickPhotoAsync();
                if (result != null)
                {
                    var stream = await result.OpenReadAsync();
                    ImageSource imageSource = ImageSource.FromStream(() => stream);
                    _fileFullPath = result.FullPath;
                    
                    //SaveFile(stream);
                }
            }
            catch (FeatureNotSupportedException)
            {
                await DisplayAlert("Error", "Feature not supported exception", "Ok");
            }
            catch (PermissionException)
            {
                await DisplayAlert("Error", "Permission exception", "Ok");
            }
            catch (Exception)
            {
                await DisplayAlert("Error", "Exception", "Ok");
            }
        }

    }
}