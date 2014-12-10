﻿using System;
using System.Collections.Generic;

namespace Atomia.Store.Core
{
    public class Cart
    {
        private readonly ICartRepository cartRepository;
        private readonly ICartPricingProvider cartPricingProvider;
        
        private List<CartItem> cartItems = new List<CartItem>();
        private string campaignCode = String.Empty;
        private decimal subTotal;
        private decimal tax;
        private decimal total;
        private int itemNoCounter = 1;

        public Cart(ICartRepository cartRepository, ICartPricingProvider cartPricingProvider)
        {
            if (cartRepository == null)
            {
                throw new ArgumentNullException("cartRepository");
            }

            if (cartPricingProvider == null)
            {
                throw new ArgumentNullException("cartPricingProvider");
            }

            this.cartRepository = cartRepository;
            this.cartPricingProvider = cartPricingProvider;
        }

        public ICollection<CartItem> CartItems { get { return this.cartItems; } }

        public string CampaignCode { get { return campaignCode; }}

        public decimal SubTotal { get { return subTotal; } }

        public decimal Tax { get { return tax; } }

        public decimal Total { get { return total; } }

        public void SetPricing(decimal subTotal, decimal tax, decimal total)
        {
            if (subTotal < 0)
            {
                throw new ArgumentOutOfRangeException("subTotal");
            }

            if (tax < 0)
            {
                throw new ArgumentOutOfRangeException("tax");
            }

            if (total < 0)
            {
                throw new ArgumentOutOfRangeException("total");
            }

            this.subTotal = subTotal;
            this.tax = tax;
            this.total = total;
        }

        public void AddItem(CartItem cartItem)
        {
            if (cartItem == null)
            {
                throw new ArgumentNullException("cartItem");
            }

            cartItem.Id = itemNoCounter++;
            this.cartItems.Add(cartItem);

            RecalculatePricingAndSave();
        }

        public void RemoveItem(int itemId)
        {
            var cartItem = this.cartItems.Find(x => x.Id == itemId);

            if (cartItem == default(CartItem))
            {
                throw new ArgumentException("itemId");
            }

            this.cartItems.Remove(cartItem);

            RecalculatePricingAndSave();
        }

        public void SetCampaignCode(string campaignCode)
        {
            if (campaignCode == null)
            {
                throw new ArgumentNullException("campaignCode");
            }

            this.campaignCode = campaignCode;

            RecalculatePricingAndSave();
        }

        public void RemoveCampaignCode()
        {
            this.campaignCode = string.Empty;

            RecalculatePricingAndSave();
        }

        public void ChangeQuantity(int itemId, decimal newQuantity)
        {
            if (newQuantity < 0)
            {
                throw new ArgumentOutOfRangeException("newQuantity");
            }

            var cartItem = this.cartItems.Find(x => x.Id == itemId);

            if (cartItem == default(CartItem))
            {
                throw new ArgumentException("itemId");
            }

            cartItem.Quantity = newQuantity;

            RecalculatePricingAndSave();
        }

        public void Clear()
        {
            this.cartItems.Clear();
            this.campaignCode = string.Empty;
            this.SetPricing(0m, 0m, 0m);

            cartRepository.SaveCart(this);
        }

        public bool IsEmpty()
        {
            return cartItems.Count == 0;
        }

        private void RecalculatePricingAndSave()
        {
            var updatedCart = cartPricingProvider.CalculatePricing(this);
            cartRepository.SaveCart(updatedCart);
        }
    }
}
