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
            Assert.AreEqual(list.Count, 2);
            Assert.AreEqual(list[0], "banana");
            Assert.AreEqual(list[1], "apple");
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
            Assert.AreEqual(list.Count, 3);
            Assert.AreEqual(list[0], "banana");
            Assert.AreEqual(list[1], "apple");
            Assert.AreEqual(list[2], "pear");
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
            Assert.AreEqual(list.Count, 2);
            Assert.AreEqual(list[0], "banana");
            Assert.AreEqual(list[1], "Apple");
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
            Assert.AreEqual(list.Count, 3);
            Assert.AreEqual(list[0], "banana");
            Assert.AreEqual(list[1], "Apple");
            Assert.AreEqual(list[2], "apple");
        }
    }
}
