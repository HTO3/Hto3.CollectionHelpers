using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hto3.CollectionHelpers.Test
{
    [TestClass]
    public class Shuffle
    {
        [TestMethod]
        public void NormalUse()
        {
            //Prepare
            var collection = new List<String>();
            collection.Add("banana");
            collection.Add("apple");
            collection.Add("pinapple");

            //Act
            var scrambled = collection.Shuffle().ToArray();

            //Assert
            Assert.AreEqual(3, scrambled.Length);
        }
    }
}
