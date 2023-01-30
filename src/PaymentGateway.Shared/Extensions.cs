using System.ComponentModel;

namespace PaymentGateway.Shared
{
    public static class Extensions
    {
        public static string JoinToStringBy(this ICollection<string> list, string seperator = " ")
        {
            return string.Join(seperator, list);
        }

        public static string GetDescription(this Enum value)
        {
            Type type = value.GetType();
            var name = Enum.GetName(type, value);
            if (name != null)
            {
                var field = type.GetField(name);
                if (field != null)
                {
                    if (Attribute.GetCustomAttribute(field,
                            typeof(DescriptionAttribute)) is DescriptionAttribute attr)
                    {
                        return attr.Description;
                    }
                    else
                    {
                        return value.ToString();
                    }
                }
            }
            return string.Empty;
        }
    }
}