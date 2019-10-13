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
    public class BuildCollectionFromString
    {
        [TestMethod]
        public void NormalUseWithInt()
        {
            //Prepare
            var delimitedString = "1,4,5,88,122";

            //Act
            var result = delimitedString.BuildCollectionFromString<Int32>(",");

            //Assert
            Assert.AreEqual(result.ElementAt(0), 1);
            Assert.AreEqual(result.ElementAt(1), 4);
            Assert.AreEqual(result.ElementAt(2), 5);
            Assert.AreEqual(result.ElementAt(3), 88);
            Assert.AreEqual(result.ElementAt(4), 122);
        }

        [TestMethod]
        public void NormalUseWithString()
        {
            //Prepare
            var delimitedString = "banana;apple;juice;lemon";

            //Act
            var result = delimitedString.BuildCollectionFromString<String>(";");

            //Assert
            Assert.AreEqual(result.ElementAt(0), "banana");
            Assert.AreEqual(result.ElementAt(1), "apple");
            Assert.AreEqual(result.ElementAt(2), "juice");
            Assert.AreEqual(result.ElementAt(3), "lemon");
        }

        [TestMethod]
        public void NormalUseWithStringSingleItem()
        {
            //Prepare
            var delimitedString = "banana";

            //Act
            var result = delimitedString.BuildCollectionFromString<String>(";");

            //Assert
            Assert.AreEqual(result.ElementAt(0), "banana");
        }
    }
}
