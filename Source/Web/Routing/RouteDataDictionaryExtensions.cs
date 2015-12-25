using System;
using System.Web.Routing;

namespace ReusableLibrary.Web.Routing
{
    public static class RouteDataDictionaryExtensions
    {
        public static void AddRange(this RouteValueDictionary routeValues, RouteValueDictionary that)
        {
            if (that == null)
            {
                throw new ArgumentNullException("that");
            }

            foreach (var pair in that)
            {
                routeValues.Add(pair.Key, pair.Value);
            }
        }

        public static void Merge(this RouteValueDictionary routeValues, RouteValueDictionary that)
        {
            if (that == null)
            {
                throw new ArgumentNullException("that");
            }

            foreach (var pair in that)
            {
                routeValues[pair.Key] = pair.Value;
            }
        }
    }
}
