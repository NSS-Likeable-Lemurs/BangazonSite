using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bangazon.Models.ProductViewModels
{
    public class ProductTypeViewModel
    {
        public int ProductId { get; set; }

        public IEnumerable<GroupedProducts> GroupedProducts { get; set; }

        public IEnumerable<Product> Products { get; set; }

        public IEnumerable<ProductType> ProductType { get; set; }

        
    }
}
