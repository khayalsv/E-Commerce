using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Image { get; set; }
        public string Type { get; set; }
        public string ProductColor { get; set; }
        public bool IsAvailable { get; set; }
        public int ProductTypeId { get; set; }
        [ForeignKey("ProductTypeId")]
        public virtual ProductTypes ProductTypes { get; set; }
    }
}
