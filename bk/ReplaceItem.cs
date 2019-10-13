using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hto3.CollectionHelpers;
using System.Collections.ObjectModel;

namespace Hto3.CollectionHelpers.Test
{
    [TestClass]
    public class ReplaceItem
    {
        [TestMethod]
        public void NormalUse()
        {
            //Prepare
            var collection = new ObservableCollection<String>();
            collection.Add("banana");
            collection.Add("apple");
            collection.Add("pinapple");

            //Act
            collection.ReplaceItem("apple", "strawberry");

            //Assert
            Assert.AreEqual(collection.Count, 3);
            Assert.AreEqual(collection[0], "banana");
            Assert.AreEqual(collection[1], "strawberry");
            Assert.AreEqual(collection[2], "pinapple");
        }
    }
}
