using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using SzybkoOdziez.Droid;
using System.Threading.Tasks;
using SzybkoOdziez.Services;
using System.IO;
using Android.Content.Res;
using Android;
using System.Reflection;
using Oracle.ManagedDataAccess.Client;
using SzybkoOdziez.Models;

[assembly: Dependency(typeof(ImgArrayGetterService))]
namespace SzybkoOdziez.Droid
{
    public class ImgArrayGetterService : IImgArrayGetterService
    {      
        public List<string> GetImgArrayStreamAsync()
        {
            FieldInfo[] resourceDrawableImgs = typeof(Resource.Drawable).GetFields();
            List<string> nameList = new List<string>();

            for (int i = 0; i< resourceDrawableImgs.Length; i++)
            {
                if ((resourceDrawableImgs[i].Name[0] == 'm' || resourceDrawableImgs[i].Name[0] == 'w') && resourceDrawableImgs[i].Name[1] == '_')
                {
                    nameList.Add(resourceDrawableImgs[i].Name);
                }              
            }

            return nameList;
        }

        public List<Product> GetProductListFromDBStreamAsync()
        {
            List<Product> productsList = new List<Product>();

            string connStr = "Data Source=(DESCRIPTION=" +
           "(ADDRESS=(PROTOCOL=TCP)(HOST=217.173.198.135)(PORT=1521))" +
           "(CONNECT_DATA=(SERVICE_NAME=tpdb)));" + "" +
           "User Id=s100824;Password=Sddb2023;";

            using (OracleConnection conn = new OracleConnection(connStr))
            {
                conn.Open();

                OracleCommand command = new OracleCommand();

                command.Connection = conn;
                command.CommandText = "select * from item";

                OracleDataReader data = command.ExecuteReader();

                if (data.HasRows)
                {
                    while (data.Read())
                    {
                        var currentProduct = new Product()
                        {
                            Id = Convert.ToInt32(data["item_id"]),
                            Name = data["name"].ToString(),
                            Description = data["description"].ToString(),
                            Price = Convert.ToDecimal(data["price"]),
                            ImageUrl = data["img_source"].ToString(),
                            Category = data["category"].ToString(),
                            Producer = data["producer"].ToString(),
                            Color = data["color"].ToString(),
                            Season = data["season"].ToString(),
                            Material = data["material"].ToString(),
                            Pattern = data["pattern"].ToString(),
                            Model = data["model"].ToString(),
                            Size = Convert.ToInt32(data["size"]),

                        };

                        productsList.Add(currentProduct);                                   
                    }
                }
                else
                {
                    List<string> imgsNameList = DependencyService.Get<IImgArrayGetterService>().GetImgArrayStreamAsync();

                    for (int i = 0; i < imgsNameList.Count; i++)
                    {
                        var currentProduct = new Product();

                        currentProduct.Id = i;
                        currentProduct.Name = "prod_name_" + i;
                        currentProduct.Description = "disc_" + i;
                        currentProduct.ImageUrl = "@drawable/" + imgsNameList[i] + ".jpg";
                        currentProduct.Price = (i + 1 * 100 % 15) * 10;

                        productsList.Add(currentProduct);
                    }
                }
            }

            return productsList;
        }
    }
}