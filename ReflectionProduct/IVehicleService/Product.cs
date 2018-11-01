using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionProduct.IVehicleService
{
    public abstract class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

    }
}
