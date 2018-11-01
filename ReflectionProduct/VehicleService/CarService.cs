using ReflectionProduct.IVehicleService;
using ReflectionProduct.VehicleModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionProduct.VehicleService
{
    public class CarService : IVehicleService<object>
    {

        public Dictionary<string, List<object>> Add(object model, Dictionary<string, List<object>> carList, List<object> listObject, string name)
        {
            
            if (!carList.ContainsKey(name))
            {
                carList.Add(name, new List<object>());
            }
            
            carList[name].Add(model);

            return carList;
        }
        
        public Dictionary<string, List<object>> Edit(int Id, Dictionary<string ,List<object>> list, object car, string name)
        {
            var idProp = car.GetType().GetProperty("Id");
            object value;

            var newList = list.Where(x => x.Key == name).SelectMany(y => y.Value).ToList();
            //var C = list.Where(x => idProp.GetValue(x).Equals(Id)).FirstOrDefault();

            var C = newList.Where(x => idProp.GetValue(x).Equals(Id)).FirstOrDefault();

            foreach (var item in car.GetType().GetProperties())
            {
                value = car.GetType().GetProperty(item.Name).GetValue(car, null);
                list.Where(x => x.Key == name).SelectMany(y => y.Value).Where(x => idProp.GetValue(x).Equals(Id))
                    .FirstOrDefault().GetType().GetProperty(item.Name).SetValue(C, value);
            }
          
            return list;
        }

        public Dictionary<string, List<object>> Delete(List<object> carList, Dictionary<string, List<object>> list, object model, string name)
        {
            var idProp = model.GetType().GetProperty("Id");
            var Id = model.GetType().GetProperty("Id").GetValue(model, null);

            var newList = list.Where(x => x.Key == name).SelectMany(y => y.Value).ToList();
            //var C = list.Where(x => idProp.GetValue(x).Equals(Id)).FirstOrDefault();

            object C = newList.Where(x => idProp.GetValue(x).Equals(Id)).FirstOrDefault();

            if (C != null)
            {
                //carList.Remove(C);

                //var P =  list.Where(x => x.Key == name).SelectMany(c => c.Value).ToList().Remove(C);
                //foreach (var item in list.Where(x => x.Value==C).SelectMany(p => p.Key).ToList())
                //{
                //    list.Remove(item);
                //}
            //    foreach (KeyValuePair<string, List<string>> kvp in map){
            //    foreach (string value in kvp.Value)
            //    {
            //        Console.WriteLine("Key = {0}, Value = {1}", kvp.Key, value);
            //    }
            //}
            //P.ToList().Remove(C);    
        }

            return list;
        }
    }
}
