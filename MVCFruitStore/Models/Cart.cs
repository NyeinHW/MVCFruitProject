using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MVCFruitStore.Models
{
        public class Cart
        {
            [Key]
            public string CartId { get; set; }

            public string UserID { get; set; }

            [Required]
            public string ProductId { get; set; }

            public string SessionId { get; set; }

            [Required]
            [MaxLength(3)]
            public int Qty { get; set; }
            public virtual Product CartProduct { get; set; }
            public virtual User CartUser { get; set; }

        }
    }

