using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Szl.Tests
{
    [TestFixture]
    public class ExtensionsTests
    {
        [Test]
        public void NameValueCollection_Extension_ToRouteValueDictionary_ResultsIn_RouteValueDictionary()
        {
            var testNameValueCollection = new NameValueCollection()
            {
                { "name1", "value1" },
                { "name2", "value2" }
            };

            var routeValues = testNameValueCollection.ToRouteValueDictionary();

            foreach (var key in testNameValueCollection.Keys)
            {
                Assert.That(testNameValueCollection[key.ToString()], Is.EqualTo(routeValues[key.ToString()]));
            }
        }

        [Test]
        public void NameValueCollection_Extension_ToRouteValueDictionary_ExcludesNulls()
        {
            var testNameValueCollection = new NameValueCollection()
            {
                { "name1", "value1" },
                { "name2", "value2" },
                { null, "a terrible null key" }
            };

            var routeValues = testNameValueCollection.ToRouteValueDictionary();

            Assert.That(routeValues.Count, Is.EqualTo(2));
            Assert.That(routeValues.ContainsValue("a terrible null key"), Is.False);
        }
    }
}