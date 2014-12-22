﻿using Atomia.Store.Core;
using System.Collections.Generic;
using System.Linq;

namespace Atomia.Store.Fakes.Adapters
{
    public class AddonProductsProvider : IProductsProvider
    {
        private readonly IProductsProvider defaultProvider;
        private readonly ICartProvider cartProvider;

        public AddonProductsProvider(IProductsProvider productsProvider, ICartProvider cartProvider)
        {
            defaultProvider = productsProvider;
            this.cartProvider = cartProvider;
        }

        public IEnumerable<Product> GetProducts(ProductSearchQuery searchQuery)
        {
            var products = defaultProvider.GetProducts(searchQuery);
            var cart = cartProvider.GetCart();

            return products.Where(
                p => p.CustomAttributes.Any(ca => ca.Name == "AddonFor" && cart.CartItems.Select(ci => ci.ArticleNumber).Contains(ca.Value)));
        }
    }
}
