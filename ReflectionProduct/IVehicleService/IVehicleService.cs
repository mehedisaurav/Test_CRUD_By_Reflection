using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionProduct.IVehicleService
{
    public interface IVehicleService<T>
    {
        Dictionary<string, List<T>> Add(T model, Dictionary<string, List<T>> list, List<object> listObject, string name);
        Dictionary<string,List<T>> Edit(int Id, Dictionary<string,List<T>> list, T model, string name);
        Dictionary<string, List<T>> Delete(List<T> list, Dictionary<string, List<T>> prodList, T model, string name);
    }
}
