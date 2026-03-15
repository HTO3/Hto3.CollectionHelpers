using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hto3.CollectionHelpers;

namespace Hto3.CollectionHelpers.Test
{
    [TestClass]
    public class IsUnderCollectionChangedEvent
    {
        [TestMethod]
        public void ConfirmThatWeAreInEventExecution()
        {
#if NETFRAMEWORK
            //Prepare
            var onEvent = false;
            var observableCollection = new ObservableCollection<String>();

            //Act
            observableCollection.CollectionChanged += new NotifyCollectionChangedEventHandler((sender, e) =>
            {
                onEvent = observableCollection.IsUnderCollectionChangedEvent();
            });
            observableCollection.Add("first");

            //Assert
            Assert.IsTrue(onEvent);
            Assert.AreEqual("first", observableCollection[0]);
#endif
        }

        [TestMethod]
        public void ConfirmThatWeAreNotInEventExecution()
        {
#if NETFRAMEWORK
            //Prepare
            var observableCollection = new ObservableCollection<String>();

            //Act
            var onEventBefore = observableCollection.IsUnderCollectionChangedEvent();
            observableCollection.Add("first");
            var onEventAfter = observableCollection.IsUnderCollectionChangedEvent();

            //Assert
            Assert.IsFalse(onEventBefore);
            Assert.AreEqual("first", observableCollection[0]);
            Assert.IsFalse(onEventAfter);
#endif
        }
    }
}
