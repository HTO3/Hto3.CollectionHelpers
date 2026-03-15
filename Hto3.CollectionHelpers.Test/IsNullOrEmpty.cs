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
    public class IsNullOrEmpty
    {
        [TestMethod]
        public void WhenGenericCollectionIsNull()
        {
            // Prepare
            var collection = default(IEnumerable<String>);

            // Act
            var result = collection.IsNullOrEmpty();

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void WhenGenericCollectionIsEmpty()
        {
            // Prepare
            var collection = new List<String>();

            // Act
            var result = collection.IsNullOrEmpty();

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void WhenGenericCollectionHasItems()
        {
            // Prepare
            var collection = new List<String> { "one", "two" };

            // Act
            var result = collection.IsNullOrEmpty();

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void WhenGenericCollectionHasSingleNullItem_CountsAsNotEmpty()
        {
            // Prepare
            var collection = new List<String> { null };

            // Act
            var result = collection.IsNullOrEmpty();

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void WhenUsingEnumerableEmpty_ReturnsTrue()
        {
            // Prepare
            var collection = Enumerable.Empty<Int32>();

            // Act
            var result = collection.IsNullOrEmpty();

            // Assert
            Assert.IsTrue(result);
        }

    }
}
