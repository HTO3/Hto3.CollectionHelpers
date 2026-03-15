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
            var succeeded = false;
            Exception lastExceptionObserved = null;

            var attempt = new Action<Int32, Exception>((item, lastException) =>
            {
                // capture the last exception passed to the attempt
                lastExceptionObserved = lastException;

                if (item < 3)
                    throw new IndexOutOfRangeException();

                // mark success when the action does not throw
                succeeded = true;
            });

            //Act
            list.TryUntilSuccess(attempt);

            //Asserts: ensure the attempt eventually succeeded and the lastException passed was the expected one
            Assert.IsTrue(succeeded, "The attempt did not succeed for any item in the collection.");
            Assert.IsNotNull(lastExceptionObserved, "Expected a previous exception to be passed to the successful attempt.");
            Assert.IsInstanceOfType(lastExceptionObserved, typeof(IndexOutOfRangeException));
        }

        [TestMethod]
        public void ValidationTestCollectionNull()
        {
            //Prepare
            const IEnumerable<Int32> NULL_ENUMERATOR = null;
            var NOT_RELEVANT_ACTION = new Action<Int32, Exception>((item, lastException) => { });

            //Act & Assert
            TestAssert.Throws<ArgumentNullException>(() => CollectionHelpers.TryUntilSuccess(NULL_ENUMERATOR, NOT_RELEVANT_ACTION));
        }

        [TestMethod]
        public void ValidationTestExceptionMustBeException()
        {
            //Prepare
            var NOT_RELEVANT_LIST = new List<Int32>(new[] { 1, 2, 3, 4 });
            var NOT_RELEVANT_ACTION = new Action<Int32, Exception>((item, lastException) => { });
            var NOT_EXCEPTION_TYPE = typeof(Int32);

            //Act & Assert
            TestAssert.Throws<ArgumentException>(() => CollectionHelpers.TryUntilSuccess(NOT_RELEVANT_LIST, NOT_RELEVANT_ACTION, NOT_EXCEPTION_TYPE));
        }

        [TestMethod]
        public void NoSuccessCase()
        {
            //Prepare
            var list = new List<Int32>(new[] { 1, 2, 3, 4 });
            var attempt = new Action<Int32, Exception>((item, lastException) =>
            {
                if (item < 10)
                    throw new IndexOutOfRangeException();
            });

            //Act & Assert
            TestAssert.Throws<AggregateException>(() => list.TryUntilSuccess(attempt));
        }

        [TestMethod]
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

            //Act & Assert
            TestAssert.Throws<FormatException>(() => list.TryUntilSuccess(attempt, typeof(FormatException)));
        }
    }
}
