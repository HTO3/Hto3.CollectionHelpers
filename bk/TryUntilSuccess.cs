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
