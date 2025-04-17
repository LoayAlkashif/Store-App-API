using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Entities
{
    public class Product : BaseEntity<int>
    {

        public Product()
        {
            
        }

        public Product(string name, string description, string pictureUrl, decimal price, int? brandId, ProductBrand brand, int? typeId, ProductType type)
        {
            Name = name;
            Description = description;
            PictureUrl = pictureUrl;
            Price = price;
            BrandId = brandId;
            Brand = brand;
            TypeId = typeId;
            Type = type;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }

        public int? BrandId { get; set; } // FK
        public ProductBrand Brand { get; set; }


        public int? TypeId { get; set; } //FK
        public ProductType Type { get; set; }



    }
}
