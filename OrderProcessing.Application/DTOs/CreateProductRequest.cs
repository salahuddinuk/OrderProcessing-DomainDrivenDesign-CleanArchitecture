using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessing.Application.DTOs
{
    public class CreateProductRequest
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }    
    }
}
