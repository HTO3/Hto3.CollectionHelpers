using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hto3.CollectionHelpers.Test
{
    [TestClass]
    public class OnCollectionChangedEvent
    {
        [TestMethod]
        public void ConfirmThatWeAreInEventExecution()
        {
            //Prepare
            var onEvent = false;
            var observableCollection = new ObservableCollection<String>();

            //Act
            observableCollection.CollectionChanged += new NotifyCollectionChangedEventHandler((sender, e) =>
            {
                onEvent = observableCollection.OnCollectionChangedEvent();
            });
            observableCollection.Add("first");

            //Assert
            Assert.IsTrue(onEvent);
            Assert.AreEqual(observableCollection[0], "first");
        }

        [TestMethod]
        public void ConfirmThatIsNotReentrancy()
        {
            //Prepare
             var observableCollection = new ObservableCollection<String>();

            //Act
            var onEventBefore = observableCollection.OnCollectionChangedEvent();
            observableCollection.Add("first");
            var onEventAfter = observableCollection.OnCollectionChangedEvent();

            //Assert
            Assert.IsFalse(onEventBefore);
            Assert.AreEqual(observableCollection[0], "first");
            Assert.IsFalse(onEventAfter);
        }
    }
}
