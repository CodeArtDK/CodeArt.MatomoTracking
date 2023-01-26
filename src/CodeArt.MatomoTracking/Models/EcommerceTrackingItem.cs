using CodeArt.MatomoTracking.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeArt.MatomoTracking.Models
{
    public class EcommerceTrackingItem : BaseTrackingItem
    {
        /// <summary>
        /// The unique string identifier for the ecommerce order (required when tracking an ecommerce order)
        /// </summary>
        [QueryParameter("ec_id")]
        public string OrderId { get; set; }

        /// <summary>
        ///  Items in the Ecommerce order. 
        /// </summary>
        public List<EcommerceTrackingOrderItem> Items { get; set; }

        /// <summary>
        /// The sub total of the order; excludes shipping
        /// </summary>
        [QueryParameter("ec_st")]
        public decimal? Subtotal { get; set; }

        /// <summary>
        /// Tax Amount of the order
        /// </summary>
        [QueryParameter("ec_tx")]
        public decimal? Tax { get; set; }

        /// <summary>
        /// Shipping cost of the Order
        /// </summary>
        [QueryParameter("ec_sh")]
        public decimal? ShippingCost { get; set; }

        /// <summary>
        /// Discount offered
        /// </summary>
        [QueryParameter("ec_dt")]
        public decimal? Discount { get; set; }


        public EcommerceTrackingItem()
        {
            this.IDGoal = 0;
            Items = new List<EcommerceTrackingOrderItem>();
        }
    }

    public class EcommerceTrackingOrderItem
    {
        public string SKU { get; set; }
        public string Name { get; set; } = "";
        public string Category { get; set; } = "";
        public decimal Price { get; set; } = 0;
        public int Quantity { get; set; } = 0;
    }
}
