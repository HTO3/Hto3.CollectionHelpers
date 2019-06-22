using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hto3.CollectionHelpers.Test
{
    [TestClass]
    public class Describe
    {
        [TestMethod]
        public void EmptyCollection()
        {
            //Prepare
            var collection = new List<String>();

            //Act
            var result = collection.Describe();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result, String.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void InvalidFormat()
        {
            //Prepare
            var collection = new List<String>();

            //Act
            var result = collection.Describe("ddddd");

            //Assert
            Assert.Fail();
        }

        [TestMethod]
        public void NoSeparator()
        {
            //Prepare
            var collection = new List<String>();
            collection.Add("apple");
            collection.Add("banana");

            //Act
            var result = collection.Describe(separator: null);

            //Assert
            Assert.AreEqual(result, "applebanana");
        }

        [TestMethod]
        public void CustomFormat()
        {
            //Prepare
            var collection = new List<DateTime>();
            collection.Add(new DateTime(2018, 1, 22));
            collection.Add(new DateTime(2018, 1, 21));

            //Act
            var result = collection.Describe(format: "{0:dd/MM/yyyy}");

            //Assert
            Assert.AreEqual(result, "22/01/2018, 21/01/2018");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullCollection()
        {
            //Prepare
            var collection = default(IEnumerable<Int32>);

            //Act
            var result = collection.Describe();

            //Assert
            Assert.Fail();
        }

        [TestMethod]
        public void NormalUse()
        {
            //Prepare
            var collection = new List<Int32>();
            collection.Add(1);
            collection.Add(2);
            collection.Add(3);

            //Act
            var result = collection.Describe();

            //Assert
            Assert.AreEqual(result, "1, 2, 3");
        }
    }
}
