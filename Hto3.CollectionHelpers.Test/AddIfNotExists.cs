using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hto3.CollectionHelpers;

namespace Hto3.CollectionHelpers.Test
{
    [TestClass]
    public class AddIfNotExists
    {
        [TestMethod]
        public void AddWhenExistsNonGeneric()
        {
            //Prepare
            var list = new ArrayList();
            list.Add("banana");
            list.Add("apple");

            //Act
            list.AddIfNotExists("apple");

            //Assert
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual("banana", list[0]);
            Assert.AreEqual("apple", list[1]);
        }

        [TestMethod]
        public void AddWhenNotExistsNonGeneric()
        {
            //Prepare
            var list = new ArrayList();
            list.Add("banana");
            list.Add("apple");

            //Act
            list.AddIfNotExists("pear");

            //Assert
            Assert.AreEqual(3, list.Count);
            Assert.AreEqual("banana", list[0]);
            Assert.AreEqual("apple", list[1]);
            Assert.AreEqual("pear", list[2]);
        }

        [TestMethod]
        public void AddWhenExistsWithPredicate()
        {
            //Prepare
            var list = new List<String>();
            list.Add("banana");
            list.Add("Apple");

            //Act
            list.AddIfNotExists((i) => String.Compare("apple", i, true) == 0, "apple");

            //Assert
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual("banana", list[0]);
            Assert.AreEqual("Apple", list[1]);
        }

        [TestMethod]
        public void AddWhenNotExistsWithPredicate()
        {
            //Prepare
            var list = new List<String>();
            list.Add("banana");
            list.Add("Apple");

            //Act
            list.AddIfNotExists((i) => i == "apple" , "apple");

            //Assert
            Assert.AreEqual(3, list.Count);
            Assert.AreEqual("banana", list[0]);
            Assert.AreEqual("Apple", list[1]);
            Assert.AreEqual("apple", list[2]);
        }
    }
}
