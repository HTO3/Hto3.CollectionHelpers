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
            Assert.AreEqual(2, collection.Count);
            Assert.AreEqual(1, collection[0]);
            Assert.AreEqual(2, collection[1]);
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
            Assert.AreEqual(2, collection.Count);
            Assert.AreEqual(2, collection[0]);
            Assert.AreEqual(100, collection[1]);
        }

        [TestMethod]
        public void NullCollection()
        {
            //Prepare
            var collection = default(ObservableCollection<Int32>);

            //Act & Assert
            TestAssert.Throws<ArgumentNullException>(() => collection.RemoveAll(i => i == 1 || i == 55));
        }
    }
}
