using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hto3.CollectionHelpers;
using System.Collections;

namespace Hto3.CollectionHelpers.Test
{
    [TestClass]
    public class EmptyIfNull
    {
        [TestMethod]
        public void WhenGenericCollectionIsNotNullGeneric()
        {
            //Prepare
            var collection = default(IEnumerable<String>);
            collection = new List<String>();

            //Act
            var result = collection.EmptyIfNull();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(collection, result);
        }

        [TestMethod]
        public void WhenGenericCollectionIsNull()
        {
            //Prepare
            var collection = default(IEnumerable<String>);

            //Act
            var result = collection.EmptyIfNull();

            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void WhenNonGenericCollectionIsNotNullGeneric()
        {
            //Prepare
            var collection = default(IEnumerable);
            collection = new ArrayList();

            //Act
            var result = collection.EmptyIfNull();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(collection, result);
        }

        [TestMethod]
        public void WhenNonGenericCollectionIsNull()
        {
            //Prepare
            var collection = default(IEnumerable);

            //Act
            var result = collection.EmptyIfNull();

            //Assert
            Assert.IsNotNull(result);
        }
    }
}
