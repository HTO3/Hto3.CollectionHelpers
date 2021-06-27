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
    public class AddRangeIfNotExists
    {
        [TestMethod]
        public void NormalUse()
        {
            //Arrange
            var listOriginal = new List<Int32>(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
            var listToAdd = new List<Int32>(new[] { 1, 2, 3, 4, 5 });
            const Int32 EXPECTED_COUNT = 10;

            //Act
            CollectionHelpers.AddRangeIfNotExists(listOriginal, listToAdd);

            //Assert
            Assert.AreEqual(EXPECTED_COUNT, listOriginal.Count);
        }

        [TestMethod]
        public void NormalUsePredicate()
        {
            //Arrange
            var listOriginal = new List<Int32>(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
            var listToAdd = new List<Int32>(new[] { 1, 2, 3, 4, 5 });
            const Int32 EXPECTED_COUNT = 10;

            //Act
            CollectionHelpers.AddRangeIfNotExists(listOriginal, (alreadyPresent, toAdd) => alreadyPresent == toAdd, listToAdd);

            //Assert
            Assert.AreEqual(EXPECTED_COUNT, listOriginal.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullListParameterUsePredicate()
        {
            //Arrange
            var listOriginal = default(List<Int32>);
            var listToAdd = new List<Int32>();

            //Act
            CollectionHelpers.AddRangeIfNotExists(listOriginal, (alreadyPresent, toAdd) => alreadyPresent == toAdd, listToAdd);

            //Assert
            Assert.Fail();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullPredicateParameterUsePredicate()
        {
            //Arrange
            var listOriginal = new List<Int32>();
            var listToAdd = new List<Int32>();

            //Act
            CollectionHelpers.AddRangeIfNotExists(listOriginal, null, listToAdd);

            //Assert
            Assert.Fail();
        }
    }
}
