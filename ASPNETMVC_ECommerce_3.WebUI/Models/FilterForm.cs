using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPNETMVC_ECommerce_3.WebUI.Models
{
    public class FilterForm
    {
        public enum OrderBy
        {
            sortDesc
            , sortAsc
        }

        public enum SortParam
        {
            sortTitle,
            sortCategory,
            sortPrice,
            sortQuantity
        }

        public int[] categories { get; set; }
        public OrderBy sort { get; set; }
        public SortParam sortParam { get; set; }
    }
}