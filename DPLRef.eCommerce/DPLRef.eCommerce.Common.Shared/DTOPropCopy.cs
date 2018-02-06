using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace DPLRef.eCommerce.Common.Shared
{
    public static class DTOPropCopy
    {
        public static void CopyProps(object source, object destination)
        {
            if (source != null && destination != null)
            {
                foreach (var sourceProp in GetProps(source))
                {
                    var destinationProp = destination.GetType().GetProperty(sourceProp.Name);
                    if (destinationProp != null)
                    {
                        if (sourceProp.PropertyType.Name == destinationProp.PropertyType.Name)
                        {
                            var value = sourceProp.GetValue(source, new object[] { });
                            destinationProp.SetValue(destination, value);
                        }
                    }
                }
            }
        }

        private static PropertyInfo[] GetProps(object o)
        {
            return o.GetType().GetProperties();
        }

        private static PropertyInfo GetProp(object o, string propName)
        {
            return o.GetType().GetProperty(propName);
        }
    }
}
