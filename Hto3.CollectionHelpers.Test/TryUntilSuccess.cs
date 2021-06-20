using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hto3.CollectionHelpers.Test
{
    [TestClass]
    public class TryUntilSuccess
    {
        [TestMethod]
        public void NormalUseWithSuccessCase()
        {
            //Prepare
            var list = new List<Int32>(new[] { 1, 2, 3, 4 });
            var attempt = new Action<Int32, Exception>((item, lastException) =>
            {
                if (item < 3)
                    throw new IndexOutOfRangeException();
            });

            //Act
            list.TryUntilSuccess(attempt);

            //Asserts
            Assert.IsTrue(true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidationTestCollectionNull()
        {
            //Prepare
            const IEnumerable<Int32> NULL_ENUMERATOR = null;
            var NOT_RELEVANT_ACTION = new Action<Int32, Exception>((item, lastException) => { });

            //Act
            CollectionHelpers.TryUntilSuccess(NULL_ENUMERATOR, NOT_RELEVANT_ACTION);

            //Assert
            Assert.Fail();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidationTestExceptionMustBeException()
        {
            //Prepare
            var NOT_RELEVANT_LIST = new List<Int32>(new[] { 1, 2, 3, 4 });
            var NOT_RELEVANT_ACTION = new Action<Int32, Exception>((item, lastException) => { });
            var NOT_EXCEPTION_TYPE = typeof(Int32);

            //Act
            CollectionHelpers.TryUntilSuccess(NOT_RELEVANT_LIST, NOT_RELEVANT_ACTION, NOT_EXCEPTION_TYPE);

            //Assert
            Assert.Fail();
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void NoSuccessCase()
        {
            //Prepare
            var list = new List<Int32>(new[] { 1, 2, 3, 4 });
            var attempt = new Action<Int32, Exception>((item, lastException) =>
            {
                if (item < 10)
                    throw new IndexOutOfRangeException();
            });

            //Act
            list.TryUntilSuccess(attempt);

            //Asserts
            Assert.Fail();
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void StopedBySpecificException()
        {
            //Prepare
            var list = new List<Int32>(new[] { 1, 2, 3, 4 });
            var attempt = new Action<Int32, Exception>((item, lastException) =>
            {
                if (item < 2)
                    throw new IndexOutOfRangeException();
                else
                    throw new FormatException();
            });

            //Act
            list.TryUntilSuccess(attempt, typeof(FormatException));

            //Asserts
            Assert.Fail();
        }
    }
}
