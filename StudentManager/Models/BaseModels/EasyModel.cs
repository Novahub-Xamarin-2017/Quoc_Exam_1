using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace StudentManager.Models.BaseModels
{
    public class IgnoreInputAttribute : Attribute
    {
        
    }
    public class PrompDisplayAttribute : Attribute
    {
        public string Display { get; set; }

        public PrompDisplayAttribute(string display)
        {
            Display = display;
        }
    }

    public class EasyModel
    {
        private readonly List<PropertyInfo> propertyInfos;

        public EasyModel()
        {
            propertyInfos = GetType().GetProperties().ToList();
        }

        public virtual void Input()
        {
            foreach (var propertyInfo in propertyInfos.Where(x=>x.CanWrite))
            {
                var prompDisplay = GetAttributeValue<PrompDisplayAttribute>(propertyInfo);

                var ignoreInput = GetAttributeValue<IgnoreInputAttribute>(propertyInfo);

                if (ignoreInput == null)
                {
                    Console.Write($"\tEnter {prompDisplay?.Display ?? propertyInfo.Name}: ");

                    switch (Type.GetTypeCode(propertyInfo.PropertyType))
                    {
                        case TypeCode.String:
                            propertyInfo.SetValue(this, Console.ReadLine());
                            break;
                        case TypeCode.Int16:
                            propertyInfo.SetValue(this, Convert.ToInt16(Console.ReadLine()));
                            break;
                        case TypeCode.Int32:
                            propertyInfo.SetValue(this, Convert.ToInt32(Console.ReadLine()));
                            break;
                        case TypeCode.DateTime:
                            propertyInfo.SetValue(this, DateTime.Parse(Console.ReadLine()));
                            break;
                        case TypeCode.Double:
                            propertyInfo.SetValue(this, Convert.ToDouble(Console.ReadLine()));
                            break;
                    }
                }
                
            }
        }

        public override string ToString()
        {
            return string.Join(", ", propertyInfos
                .Where(x => x.CanRead)
                .Select(x => $"{x.Name}: {x.GetValue(this)}"));
        }

        private static T GetAttributeValue<T>(PropertyInfo propertyInfo) where T: Attribute
        {

            return propertyInfo.GetCustomAttributes(typeof(T), true).FirstOrDefault() as T;

        }
    }
}
