using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using SzybkoOdziez.Models;

namespace SzybkoOdziez.Services
{
    public class OrderHistoryDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate SingleItemOrder { get; set; }
        public DataTemplate MultipleItemsOrder { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return ((Order)item).Products.Count > 1 ? MultipleItemsOrder : SingleItemOrder;
        }
    }
}
