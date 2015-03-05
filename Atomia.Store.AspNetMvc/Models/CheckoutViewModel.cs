﻿using Atomia.Store.AspNetMvc.Helpers;
using Atomia.Store.AspNetMvc.Ports;
using Atomia.Store.Core;
using Atomia.Web.Plugin.Validation.ValidationAttributes;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Atomia.Store.AspNetMvc.Models
{
    public abstract class CheckoutViewModel
    {
        public abstract PaymentMethodGuiPlugin SelectedPaymentMethod { get; }

        public abstract IList<TermsOfServiceConfirmationModel> TermsOfService { get; set; }
    }


    public class DefaultCheckoutViewModel : CheckoutViewModel
    {
        private readonly ITermsOfServiceProvider termsOfServiceProvider = DependencyResolver.Current.GetService<ITermsOfServiceProvider>();
        private readonly PaymentMethodGuiPluginsProvider paymentPluginsProvider = new PaymentMethodGuiPluginsProvider();

        private IList<TermsOfServiceConfirmationModel> termsOfServiceConfirmationModels = null;

        public DefaultCheckoutViewModel()
        {
            this.PaymentMethodGuiPlugins = paymentPluginsProvider.AvailablePaymentMethodGuiPlugins;
            this.SelectedPaymentMethodId = paymentPluginsProvider.DefaultPaymentMethodId;
        }

        public virtual IEnumerable<PaymentMethodGuiPlugin> PaymentMethodGuiPlugins { get; set; }

        [AtomiaRequired("ValidationErrors,ErrorEmptyField")]
        public string SelectedPaymentMethodId { get; set; }

        public override PaymentMethodGuiPlugin SelectedPaymentMethod
        {
            get { return PaymentMethodGuiPlugins.Where(x => x.Id == SelectedPaymentMethodId).FirstOrDefault();}
        }

        public override IList<TermsOfServiceConfirmationModel> TermsOfService
        {
            get
            {
                if (termsOfServiceConfirmationModels == null)
                {
                    return termsOfServiceProvider.GetTermsOfService().Select(tos =>
                        new TermsOfServiceConfirmationModel
                        {
                            Id = tos.Id,
                            Name = tos.Name,
                        }).ToList();
                }

                return termsOfServiceConfirmationModels;
            }
            set
            {
                var termsOfServiceData = termsOfServiceProvider.GetTermsOfService();
                
                foreach(var val in value)
                {
                    var data = termsOfServiceData.First(tos => tos.Id == val.Id);
                    val.Name = data.Name;
                }

                termsOfServiceConfirmationModels = value;
            }
        }
    }
}
