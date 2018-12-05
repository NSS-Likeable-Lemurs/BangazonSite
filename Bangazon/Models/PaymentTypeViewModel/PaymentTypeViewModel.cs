using Bangazon.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Bangazon.Models.PaymentTypeViewModel
{
    public class PaymentTypeViewModel
    {

        public List<SelectListItem> PaymentTypeListItems { get; set; }
        public PaymentType PaymentType { get; set; }

        public PaymentTypeViewModel() { }

        public PaymentTypeViewModel(List<PaymentType> paymentTypes)
        {

            PaymentTypeListItems = paymentTypes.Select(li => new SelectListItem
                {
                  Text = li.Description,
                  Value = li.PaymentTypeId.ToString()
                }).ToList();
        }
    }
}
