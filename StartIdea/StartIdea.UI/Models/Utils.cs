using System.Collections.Generic;
using System.Linq;

namespace StartIdea.UI.Models
{
    public static class Utils
    {
        public static bool IsEmpty<T>(this IEnumerable<T> list)
        {
            if (list is ICollection<T>)
                return ((ICollection<T>)list).Count == 0;

            return !list.Any();
        }
    }
}