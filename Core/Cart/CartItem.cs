﻿using System;
using System.Collections.Generic;

namespace Atomia.Store.Core
{
    public sealed class CartItem
    {
        private decimal quantity = 1;
        private int id;
        private decimal price;
        private decimal discount;
        private decimal taxAmount;

        public string ArticleNumber { get; set;}

        public decimal Quantity
        {
            get { return quantity; }
            set 
            { 
                if (quantity < 1)
                {
                    throw new ArgumentException("Quantity must be greater than 0.");
                }

                quantity = value;
            }
        }

        public RenewalPeriod RenewalPeriod { get; set; }

        public Dictionary<string, string> CustomAttributes { get; set; }

        public int Id { 
            get 
            {
                return this.id;
            }
            set
            {
                if (id == 0)
                {
                    id = value;
                }
                else
                {
                    throw new InvalidOperationException("ItemId has already been set for cart item.");
                }
            }
        }

        public decimal Price { get { return price; } }

        public decimal Discount { get { return discount; } }

        public decimal TaxAmount { get { return taxAmount; } }
        
        public decimal Total
        {
            get { return (Price - Discount) * Quantity; }
        }

        public void SetPricing(decimal price, decimal discount, decimal taxAmount)
        {
            if (price < 0)
            {
                throw new ArgumentOutOfRangeException("price");
            }

            if (discount < 0)
            {
                throw new ArgumentOutOfRangeException("discount");
            }

            if (taxAmount < 0)
            {
                throw new ArgumentOutOfRangeException("taxAmount");
            }

            this.price = price;
            this.discount = discount;
            this.taxAmount = taxAmount;
        }
    }
}
