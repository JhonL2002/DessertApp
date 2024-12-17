using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DessertApp.Infraestructure.ConfigurationServices
{
    public static class VerifyCastingEntity<TInterface, T> where T : class
    {
        public static T VerifyObject(TInterface elementToCast)
        {
            if (elementToCast is not T)
            {
                throw new InvalidCastException($"The provided {elementToCast} is not a type of T");
            }
            var castObject = elementToCast as T;
            return castObject!;
        }
    }
}
