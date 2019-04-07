using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DequeTest
{
    [TestClass]
    public class ReverseDequeTests
    {        
        [TestMethod]
        public void CreationTest()
        {
            var deque = new Deque<int>();
            var reverse = deque.Reverse();
        }

        [TestMethod]
        public void DoubleReverseTest()
        {
            var deque = new Deque<int>();
            var reverse = deque.Reverse();
            Assert.AreSame(deque, reverse.Reverse());
        }

        [TestMethod]
        public void PeekLeftTest()
        {
            var deque = new Deque<int>();
            for (int i = 0; i < 10; i++)
            {
                deque.Add(i);
            }
            var reverse = deque.Reverse();
            Assert.AreEqual(deque.PeekRight(), reverse.PeekLeft());
        }

        [TestMethod]
        public void PeekRightTest()
        {
            var deque = new Deque<int>();
            for (int i = 0; i < 10; i++)
            {
                deque.Add(i);
            }
            var reverse = deque.Reverse();
            Assert.AreEqual(deque.PeekLeft(), reverse.PeekRight());
        }

        [TestMethod]
        public void PopLeftTest()
        {
            var deque = new Deque<int>();
            for (int i = 0; i < 10; i++)
            {
                deque.Add(i);
            }
            var reverse = deque.Reverse();
            Assert.AreEqual(deque.PeekRight(), reverse.PopLeft());
        }

        [TestMethod]
        public void PopRightTest()
        {
            var deque = new Deque<int>();
            for (int i = 0; i < 10; i++)
            {
                deque.Add(i);
            }
            var reverse = deque.Reverse();
            Assert.AreEqual(deque.PeekLeft(), reverse.PopRight());
        }

        [TestMethod]
        public void PushLeftTest()
        {
            var deque = new Deque<int>();
            for (int i = 0; i < 10; i++)
            {
                deque.Add(i);
            }
            var reverse = deque.Reverse();
            reverse.PushLeft(99);
            Assert.AreEqual(deque.PeekRight(), 99);
        }

        [TestMethod]
        public void PushRightTest()
        {
            var deque = new Deque<int>();
            for (int i = 0; i < 10; i++)
            {
                deque.Add(i);
            }
            var reverse = deque.Reverse();
            reverse.PushRight(99);
            Assert.AreEqual(deque.PeekLeft(), 99);
        }

        [TestMethod]
        public void RemoveAtTest()
        {
            var deque = new Deque<int>();
            for (int i = 0; i < 10; i++)
            {
                deque.Add(i);
            }
            var reverse = deque.Reverse();
            reverse.RemoveAt(0);
            Assert.AreEqual(deque.PeekRight(), 8);
            Assert.AreEqual(deque.Count, 9);
        }

        [TestMethod]
        public void IndexerTest()
        {
            var deque = new Deque<int>();
            var reverse = deque.Reverse();
            for (int i = 0; i < 124; i++)
            {
                deque.Add(i);
            }
            for (int i = 0; i < deque.Count; i++)
            {
                Assert.AreEqual(deque[i], reverse[deque.Count - 1 - i]);
            }
        }

        [TestMethod]
        public void IndexerSetTest()
        {
            var deque = new Deque<int>();
            var reverse = deque.Reverse();
            for (int i = 0; i < 124; i++)
            {
                deque.Add(i);
            }
            for (int i = 0; i < deque.Count; i++)
            {
                reverse[deque.Count - 1 - i] = 0;
                Assert.AreEqual(0, reverse[deque.Count - 1 - i]);
            }
        }

        [TestMethod]
        public void EnumeratorTest()
        {
            var deque = new Deque<int>();
            var reverse = deque.Reverse();
            for (int i = 0; i < 125; i++)
            {
                deque.Add(i);
            }

            var y = 124;
            foreach (var item in reverse)
            {
                Assert.AreEqual(y, item);
                y--;
            }
        }
    }
}
