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
    public class FlatTree
    {
        [TestMethod]
        public void TreeWithInfiniteLoop()
        {
            //Prepare
            var node1 = new Node() { Value = 1 };
            var node2 = new Node() { Value = 2 };
            var node3 = new Node() { Value = 3 };
            var node4 = new Node() { Value = 4 };
            var node5 = new Node() { Value = 5 };
            var node6 = new Node() { Value = 6 };
            var node7 = new Node() { Value = 7 };
            var node8 = new Node() { Value = 8 };
            var node9 = new Node() { Value = 9 };
            var node10 = new Node() { Value = 10 };
            var node11 = new Node() { Value = 11 };

            node1.Children.Add(node9);
            node1.Children.Add(node8);
            node1.Children.Add(node2);
            node2.Children.Add(node7);
            node2.Children.Add(node6);
            node2.Children.Add(node3);
            node3.Children.Add(node5);
            node3.Children.Add(node4);
            node7.Children.Add(node10);
            node10.Children.Add(node11);
            node11.Children.Add(node1);

            //Act
            var nodes = node1.FlatTree(n => n.Children);
            
            //Assert
            Assert.AreEqual(11, nodes.Count);
            Assert.AreEqual(node1, nodes.ElementAt(0));
            Assert.AreEqual(node9, nodes.ElementAt(1));
            Assert.AreEqual(node8, nodes.ElementAt(2));
            Assert.AreEqual(node2, nodes.ElementAt(3));
            Assert.AreEqual(node7, nodes.ElementAt(4));
            Assert.AreEqual(node10, nodes.ElementAt(5));
            Assert.AreEqual(node11, nodes.ElementAt(6));
            Assert.AreEqual(node6, nodes.ElementAt(7));
            Assert.AreEqual(node3, nodes.ElementAt(8));
            Assert.AreEqual(node5, nodes.ElementAt(9));
            Assert.AreEqual(node4, nodes.ElementAt(10));
        }

    }


    public class Node
    {
        public Int32 Value { get; set; }
        public List<Node> Children { get; set; } = new List<Node>();

        public override string ToString()
        {
            return $"{{Value={Value}}}";
        }
    }
}
