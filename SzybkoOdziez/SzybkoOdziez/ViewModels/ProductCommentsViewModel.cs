using Android.App;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using SzybkoOdziez.Models;
using Oracle.ManagedDataAccess.Client;
using Xamarin.Forms;
using static Android.Telephony.CarrierConfigManager;
using System.Collections.Generic;
using System;
using System.IO;

namespace SzybkoOdziez.ViewModels
{
    public class ProductCommentsViewModel : BaseViewModel
    {
        public ObservableCollection<Comment> Comments { get; set; }

        public ProductCommentsViewModel()
        {
            Comments = new ObservableCollection<Comment>();
        }

        public ProductCommentsViewModel(ObservableCollection<Comment> comments)
        {
            Comments = comments;
        }

        //public async void OnProductCommentsOpen(Product product)
        //{
        //    await LoadCommentsAsync(product);
        //}

        private async Task LoadCommentsAsync(Product product)
        {
            Comments.Clear();
            foreach (var comment in product.Comments)
            {
                Comments.Add(comment);
            }
        }

        public async void OnProductCommentsOpen(Product product)
        {
            InitializeCommentsFromDB(product);
        }

        public void InitializeCommentsFromDB(Product product)
        {
            Comments.Clear();

            string querySelectProductComments = "SELECT * FROM COMMENTS WHERE(item_item_id = :item_id)";

            var app = (App)Xamarin.Forms.Application.Current;
            string ConnectionString = app.connectionString;
            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                using (OracleCommand cmd = new OracleCommand(querySelectProductComments, conn))
                {
                    cmd.Parameters.Add("item_id", product.Id);
                    try
                    {
                        conn.Open();
                        OracleDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                if (reader.IsDBNull(reader.GetOrdinal("comment_image")))
                                {
                                    Comment sqlcomment = new Comment()
                                    {
                                        DBId = reader.GetInt32(0),
                                        Title = reader.GetString(1),
                                        Content = reader.GetString(2),
                                        ProductId = reader.GetInt32(3),
                                        UserId = reader.GetInt32(4),
                                    };
                                    Comments.Add(sqlcomment);
                                }
                                else
                                {
                                    string localFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                                    string localPath = Path.Combine(localFolderPath, "comment_image_" + reader.GetInt32(0));
                                    var oracleBlob = reader.GetOracleBlob(5);
                                    byte[] byteData;
                                    using (MemoryStream ms = new MemoryStream())
                                    {
                                        oracleBlob.CopyTo(ms);
                                        byteData = ms.ToArray();
                                    }
                                    using (FileStream fs = new FileStream(localPath, FileMode.Create, FileAccess.Write))
                                    {
                                        fs.Write(byteData, 0, byteData.Length);
                                    }
                                    Comment sqlcomment = new Comment()
                                    {
                                        DBId = reader.GetInt32(0),
                                        Title = reader.GetString(1),
                                        Content = reader.GetString(2),
                                        ProductId = reader.GetInt32(3),
                                        UserId = reader.GetInt32(4),
                                        CommentImageSource = localPath,
                                    };
                                    Comments.Add(sqlcomment);
                                }

                            }
                        }
                        conn.Close();
                    }
                    catch (OracleException ex)
                    {
                        //blad oracle 

                        Console.WriteLine(ex.ToString());
                        conn.Close();
                    }
                }
            }
        }

    }
}