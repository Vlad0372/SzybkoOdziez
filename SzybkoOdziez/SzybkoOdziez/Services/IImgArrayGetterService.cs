using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using SzybkoOdziez.Models;

namespace SzybkoOdziez
{
    public interface IImgArrayGetterService
    {
        List<string> GetImgArrayStreamAsync();
        List<Product> GetProductListFromDBStreamAsync();
    }
}
