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
    public class AddRange
    {
        [TestMethod]
        public void NormalUse()
        {
            //Arrange
            var ORIGINAL_LIST = new ArrayList();
            ORIGINAL_LIST.Add(1);
            var LIST_ADD = new ArrayList();
            LIST_ADD.Add(2);
            LIST_ADD.Add(3);
            const Int32 EXPECTED_COUNT = 3;

            //Act
            CollectionHelpers.AddRange(ORIGINAL_LIST, LIST_ADD);

            //Assert
            Assert.AreEqual(EXPECTED_COUNT, ORIGINAL_LIST.Count);
        }
    }
}
