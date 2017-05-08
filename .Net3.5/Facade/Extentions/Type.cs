using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Facade.Extentions.Type
{
    internal static class Type
    {
        public static bool IsSubclassOfGeneric(this System.Type generic, System.Type toCheck)
        {
            if (generic == toCheck) return true;
            if (generic.IsSubclassOf(toCheck)) return true;

            System.Type t = generic;

            while (t.BaseType != null)
            {
                if (t.Name == toCheck.Name && t.Namespace == toCheck.Namespace)
                    return true;
                t = t.BaseType;
            }

            return false;
        }

        public static string FullNormalName(this System.Type t)
        {
            System.Type[] generic = t.GetGenericArguments();
            if (t.FullName != null)
                if (generic.Length == 0)
                    if (t.FullName == null)
                        return t.Name.Replace("+", ".");
                    else
                        return t.FullName.Replace("+", ".");
            if (t.FullName == null)
                if (t.IsGenericParameter)
                    return getFullNormalName(t.Name, generic);
                else
                    return getFullNormalName((t.Namespace != null ? t.Namespace + '.' : "") + t.Name, generic);

            return getFullNormalName(t.FullName, generic);
        }

        private static string getFullNormalName(string name, System.Type[] generic)
        {
            if (generic.Length == 0)
                return name.Split('`')[0].Replace("+", ".");
            else
                return name.Split('`')[0].Replace("+", ".") + "<" + string.Join(",", generic.Select(x => FullNormalName(x)).ToArray()) + ">";
        }
    }
}
