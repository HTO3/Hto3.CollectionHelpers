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
            Assert.AreEqual(3, collection.Count);
            Assert.AreEqual("banana", collection[0]);
            Assert.AreEqual("strawberry", collection[1]);
            Assert.AreEqual("pinapple", collection[2]);
        }
    }
}
