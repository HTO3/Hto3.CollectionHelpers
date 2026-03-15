using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hto3.CollectionHelpers;

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
            Assert.AreEqual(String.Empty, result);
        }

        [TestMethod]
        public void InvalidFormat()
        {
            //Prepare
            var collection = new List<String>();

            //Act & Assert
            TestAssert.Throws<FormatException>(() => collection.Describe("ddddd"));
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
            Assert.AreEqual("applebanana", result);
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
            Assert.AreEqual("22/01/2018, 21/01/2018", result);
        }

        [TestMethod]
        public void NullCollection()
        {
            //Prepare
            var collection = default(IEnumerable<Int32>);

            //Act & Assert
            TestAssert.Throws<ArgumentNullException>(() => collection.Describe());
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
            Assert.AreEqual("1, 2, 3", result);
        }
    }
}
