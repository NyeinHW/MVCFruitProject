using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCFruitStore.Models
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }

        [Required]
        [MaxLength(128)]
        public string Name { get; set; }

        public string LastName { get; set; }

        [MaxLength(128)]
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string SessionId { get; set; }
    }
}
