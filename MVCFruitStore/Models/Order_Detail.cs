using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MVCFruitStore.Models
{
    public class Order_Detail
    {
        public int Id { get; set; }

        [Required]
        public string OrderId { get; set; }

        [Required]
        public string ProductId { get; set; }

        [Required]
        [MaxLength(36)]
        public string ActivationCode { get; set; }

        public int PurchaseQty { get; set; }


        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
