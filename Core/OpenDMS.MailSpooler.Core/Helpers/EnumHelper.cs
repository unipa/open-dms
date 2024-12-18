using System.ComponentModel;
using System.Reflection;

namespace OpenDMS.MailSpooler.Core.Helpers
{
    public static class EnumHelper<T>
    {
        public static T GetDescriptionEnum(string description)
        {
            Type enumType = typeof(T);

            // Can't use generic type constraints on value types,
            // so have to do check like this
            if (enumType.BaseType != typeof(Enum))
                throw new ArgumentException("T must be of type System.Enum");

            Array enumValArray = Enum.GetValues(enumType);

            foreach (int val in enumValArray)
            {

                MemberInfo mi = enumType.GetMember(Enum.Parse(enumType, val.ToString()).ToString()).FirstOrDefault();
                DescriptionAttribute enumAttribute = mi.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;

                if (String.Compare(enumAttribute.Description, description, true) == 0)
                    return ((T)Enum.Parse(enumType, val.ToString()));
            }

            return ((T)Enum.Parse(enumType, enumValArray.GetValue(0).ToString())); ;
        }

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }
    }
}
