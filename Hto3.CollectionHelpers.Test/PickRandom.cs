using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hto3.CollectionHelpers.Test
{
    [TestClass]
    public class PickRandom
    {
        [TestMethod]
        public void NormalUse()
        {
            //Arrange
            var COLLECTION = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            const Int32 DEFAULT_NOT_EXPECTED = default(Int32);

            //Act
            var result = CollectionHelpers.PickRandom(COLLECTION);

            //Assert
            Assert.AreNotEqual(DEFAULT_NOT_EXPECTED, result);
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void NormalUseEmptyCollection()
        {
            //Arrange
            var COLLECTION = new Int32[0];

            //Act
            var result = CollectionHelpers.PickRandom(COLLECTION);

            //Assert
            Assert.Fail();
        }
    }
}
