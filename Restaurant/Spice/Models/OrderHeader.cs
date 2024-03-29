﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Spice.Models
{
    public class OrderHeader
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        [Required]
        public DateTime OderDate { get; set; }

        [Required]
        public double OrderTotalOriginal { get; set; }

        [Required]
        [DisplayFormat(DataFormatString ="{0:C}")] //for currency
        [Display(Name ="Order Total")]
        public double OrderTotal { get; set; } //total after any coupons applied

        [Required]
        [Display(Name ="Pickup Time")]
        public DateTime PickupTime { get; set; }

        [Required]
        [NotMapped]
        public DateTime PickUpDate { get; set; }  //to tweak the view

        [Display(Name ="Coupon Code")]
        public string CouponCode { get; set; }

        public double CouponCodeDiscount { get; set; }
        public string Status { get; set; }
        public string PaymentStatus { get; set; }
        public string Comments { get; set; }

        [Display(Name ="Pickup Name")]
        public string PickUpName { get; set; }

        [Display(Name ="Phone Number")]
        public string PhoneNumber { get; set; }
        public string TransactionId { get; set; }
    }

}
