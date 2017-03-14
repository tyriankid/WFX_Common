namespace Hidistro.Entities
{
    using System;
    using System.ComponentModel;

    public class EnumDescription
    {
        public static string GetEnumDescription(Enum enumSubitem)
        {
            string name = enumSubitem.ToString();
            object[] customAttributes = enumSubitem.GetType().GetField(name).GetCustomAttributes(typeof(DescriptionAttribute), false);
            if ((customAttributes == null) || (customAttributes.Length == 0))
            {
                return name;
            }
            DescriptionAttribute attribute = (DescriptionAttribute) customAttributes[0];
            return attribute.Description;
        }
    }
}

