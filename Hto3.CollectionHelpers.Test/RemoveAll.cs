using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hto3.CollectionHelpers;

namespace Hto3.CollectionHelpers.Test
{
    [TestClass]
    public class RemoveAll
    {

        [TestMethod]
        public void NormalUseCondition1()
        {
            //Prepare
            var collection = new ObservableCollection<Int32>();
            collection.Add(1);
            collection.Add(2);
            collection.Add(55);
            collection.Add(100);

            //Act
            collection.RemoveAll(i => i > 10);

            //Assert
            Assert.AreEqual(collection.Count, 2);
            Assert.AreEqual(collection[0], 1);
            Assert.AreEqual(collection[1], 2);
        }

        [TestMethod]
        public void NormalUseCondition2()
        {
            //Prepare
            var collection = new ObservableCollection<Int32>();
            collection.Add(1);
            collection.Add(2);
            collection.Add(55);
            collection.Add(100);

            //Act
            collection.RemoveAll(i => i == 1 || i == 55);

            //Assert
            Assert.AreEqual(collection.Count, 2);
            Assert.AreEqual(collection[0], 2);
            Assert.AreEqual(collection[1], 100);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullCollection()
        {
            //Prepare
            var collection = default(ObservableCollection<Int32>);

            //Act
            collection.RemoveAll(i => i == 1 || i == 55);

            //Assert
            Assert.Fail();
        }
    }
}
