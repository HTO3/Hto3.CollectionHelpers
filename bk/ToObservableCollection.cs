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
    public class ToObservableCollection
    {
        [TestMethod]
        public void NormalUse()
        {
            //Prepare
            var anArray = new Int32[] { 1, 2, 3 };

            //Act
            var result = anArray.ToObservableCollection();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ObservableCollection<Int32>));
            Assert.AreEqual(anArray.Length, result.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullCase()
        {
            //Prepare
            var nullCollection = default(IEnumerable<String>);

            //Act
            nullCollection.ToObservableCollection();

            //Assert
            Assert.Fail();
        }
    }
}
