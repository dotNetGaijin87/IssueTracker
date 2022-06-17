using System.Reflection;

namespace IssueTracker.Application.Utils;
public static class DynamicUtils
{
    public static T2 UpdateObjectWithFieldMask<T1, T2>(T1 objectWithNewValues, List<string> fieldMask, T2 oldObject)
    {
        T2 updatedObject = oldObject;
        foreach (var field in fieldMask)
        {
            var propertyToBeUpdated = GetProperty(updatedObject, field);

            var value = GetPropertyValue(objectWithNewValues, field);

            if (propertyToBeUpdated?.Name.ToLower() == field.ToLower())
            {
                propertyToBeUpdated.SetValue(updatedObject, value, null);
            }
        }

        return updatedObject;
    }

    private static PropertyInfo GetProperty(object src, string propName)
    {
        if (src == null) throw new ArgumentException("Value cannot be null.", "src");
        if (propName == null) throw new ArgumentException("Value cannot be null.", "propName");

        if (propName.Contains("."))
        {
            var props = propName.Split(new char[] { '.' }, 2);
            var prop = GetProperty(GetProperty(src, props[0]), props[1]);

            return prop;
        }
        else
        {
            var prop = src.GetType().GetProperty(propName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            return prop;
        }
    }

    private static object GetPropertyValue(object src, string propName)
    {
        if (src == null) throw new ArgumentException("Value cannot be null.", "src");
        if (propName == null) throw new ArgumentException("Value cannot be null.", "propName");

        if (propName.Contains(".")) 
        {
            var props = propName.Split(new char[] { '.' }, 2);
            return GetPropertyValue(GetPropertyValue(src, props[0]), props[1]);
        }
        else
        {
            var prop = src.GetType().GetProperty(propName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            return prop != null ? prop.GetValue(src, null) : null;
        }
    }


}