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
            Assert.AreEqual(1, result.ElementAt(0));
            Assert.AreEqual(4, result.ElementAt(1));
            Assert.AreEqual(5, result.ElementAt(2));
            Assert.AreEqual(88, result.ElementAt(3));
            Assert.AreEqual(122, result.ElementAt(4));
        }

        [TestMethod]
        public void NormalUseWithString()
        {
            //Prepare
            var delimitedString = "banana;apple;juice;lemon";

            //Act
            var result = delimitedString.BuildCollectionFromString<String>(";");

            //Assert
            Assert.AreEqual("banana", result.ElementAt(0));
            Assert.AreEqual("apple", result.ElementAt(1));
            Assert.AreEqual("juice", result.ElementAt(2));
            Assert.AreEqual("lemon", result.ElementAt(3));
        }

        [TestMethod]
        public void NormalUseWithStringSingleItem()
        {
            //Prepare
            var delimitedString = "banana";

            //Act
            var result = delimitedString.BuildCollectionFromString<String>(";");

            //Assert
            Assert.AreEqual("banana", result.ElementAt(0));
        }
    }
}
