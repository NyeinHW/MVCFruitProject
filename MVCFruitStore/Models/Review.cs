using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCFruitStore.Models
{
    public class Review
    {
        public string Id { get; set; }

        [Required]
        public string ReviewText { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string ProductId { get; set; }

        public virtual Product ReviewProduct { get; set; }
        public virtual User ReviewUser { get; set; }
    }
}
