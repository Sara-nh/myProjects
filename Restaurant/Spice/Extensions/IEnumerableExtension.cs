﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spice.Extensions
{
    public static class IEnumerableExtension
    {
        public static IEnumerable<SelectListItem> ToSelectedListItem<T>(this IEnumerable<T> items, int SelectedValue)            
        {
            return from item in items
                   select new SelectListItem
                   {
                       Text = item.GetPropertyValue("Name"),
                       Value=item.GetPropertyValue("Id"),
                       Selected=item.GetPropertyValue("Id").Equals(SelectedValue.ToString()),
                   };         
        }
    }
}
