using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.Mvc;

namespace Szl.MvcDemo
{
    public static class Extensions
    {
        public static IDictionary<string, object> ToSimpleDictionary(this NameValueCollection nameValueCollection)
        {
            var simpleDictionary = new Dictionary<string, object>();
            nameValueCollection.CopyTo(simpleDictionary);
            return simpleDictionary;
        }
    }
}