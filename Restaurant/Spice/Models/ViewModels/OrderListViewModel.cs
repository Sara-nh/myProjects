using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spice.Models.ViewModels
{
    //197
    public class OrderListViewModel
    {
        public IList<OrderDetailsViewModel>Orders { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
