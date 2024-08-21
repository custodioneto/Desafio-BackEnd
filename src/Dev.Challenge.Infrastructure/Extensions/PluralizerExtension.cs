using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Challenge.Infrastructure.Extensions
{
    public static class PluralizerExtension
    {
        public static string Pluralize(this string name)
        {
            if (name.EndsWith("y"))
            {
                name = name.Substring(0, name.Length - 1) + "ies";
            }
            else
            {
                name = name + "s";
            }
            return char.ToLowerInvariant(name[0]) + name.Substring(1);
        }
    }
}
