using Spice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spice.Utility
{
    public static class SD
    {
        public static string DefaultFoodImage = "default_food.png";
        public const string ManagerUser = "Manager";
        public const string KitchenUser = "Kitchen";
        public const string FrontDeskUser = "FrontDesk";
        public const string CustomerEndUser = "Customer";

        public const string ssShoppingCartCount = "ssCartCount";
		public const string ssCouponCode = "ssCouponCode";

		//statuses for orders and payments
		public const string StatusSubmitted = "Submitted";
		public const string StatusInProcess = "Being Prepared";
		public const string StatusReady = "Ready for Pickup";
		public const string Statuscompleted = "Completed";
		public const string StatusCancelled = "Cancelled";

		public const string PaymentStatusPending = "Pending";
		public const string PaymentStatusApproved = "Approved";
		public const string PaymentStatusRejected = "Rejected";

		// to convert Html to Raw Html
		public static string ConvertToRawHtml(string source)
		{
			char[] array = new char[source.Length];
			int arrayIndex = 0;
			bool inside = false;

			for (int i = 0; i < source.Length; i++)
			{
				char let = source[i];
				if (let == '<') //looking for these signs and convert them
				{
					inside = true;
					continue;
				}
				if (let == '>')
				{
					inside = false;
					continue;
				}
				if (!inside)
				{
					array[arrayIndex] = let;
					arrayIndex++;
				}
			}
			return new string(array, 0, arrayIndex);
		}
		//171
		public static double DiscountedPrice(Coupon couponFromDb,double OriginalOrderTotal)
        {
            if (couponFromDb == null)
            {
				return OriginalOrderTotal;
            }
            else
            {
                if (couponFromDb.MinimumAmount > OriginalOrderTotal)
                {
					return OriginalOrderTotal;
                }
                else
                {
                    //everything is valid
                    if (Convert.ToInt32(couponFromDb.CouponType) == (int)Coupon.ECouponType.Dollar)
                    {
						return Math.Round(OriginalOrderTotal - couponFromDb.Discount, 2);
                    }
                    else
                    {
						if (Convert.ToInt32(couponFromDb.CouponType) == (int)Coupon.ECouponType.Percent)
						{
							return Math.Round(OriginalOrderTotal -(OriginalOrderTotal* couponFromDb.Discount/100), 2);
						}

					}
                }				
            }
			return OriginalOrderTotal;
		}

	}

}
