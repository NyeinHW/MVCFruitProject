using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCFruitStore.Models
{
    public class AllItems
    {
        public string ProductId { get; set; }
        public string ProductImage { get; set; }
        public string ProductName { get; set; }
        public string ProductDesc { get; set; }
        public double ProductPrice { get; set; }
        public int ProductQty { get; set; }
        public DateTime DateAndTime { get; set; }
    }
}
