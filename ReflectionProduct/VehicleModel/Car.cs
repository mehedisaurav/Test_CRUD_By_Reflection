using ReflectionProduct.IVehicleService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionProduct.VehicleModel
{
    public class Car : Product
    {
        public string Engine { get; set; }
        public string Model { get; set; }
        public string Category { get; set; }
        

    }
}
