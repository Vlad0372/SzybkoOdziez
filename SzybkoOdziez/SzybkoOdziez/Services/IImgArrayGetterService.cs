using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SzybkoOdziez
{
    public interface IImgArrayGetterService
    {
        List<string> GetImgArrayStreamAsync();
    }
}
