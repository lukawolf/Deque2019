using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DequeTest
{
    [TestClass]
    public class DequeTests
    {
        [TestMethod]
        public void CreationTest()
        {
            var deque = new Deque<int>();
        }

        [TestMethod]
        public void Creation9Test()
        {
            var deque = new Deque<int>(9);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void WrongCreationTest()
        {
            var deque = new Deque<int>(-5);
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void InvalidIndexTest()
        {
            var deque = new Deque<int>();
            var a = deque[1];
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void InvalidIndex2Test()
        {
            var deque = new Deque<int>();
            var a = deque[-1];
        }

        [TestMethod]
        public void AddTest()
        {
            var deque = new Deque<int>();
            deque.Add(5);
            Assert.AreEqual(5, deque[0]);
        }

        [TestMethod]
        public void LongAddTest()
        {
            var deque = new Deque<int>();
            for (int i = 0; i < 125; i++)
            {
                deque.Add(5);
            }
            deque.Add(7);
            Assert.AreEqual(7, deque.PeekRight());
        }

        [TestMethod]
        public void Count0Test()
        {
            var deque = new Deque<int>();
            Assert.AreEqual(0, deque.Count);
        }

        [TestMethod]
        public void CountTest()
        {
            var deque = new Deque<int>();
            deque.Add(5);
            deque.Add(5);
            deque.Add(5);
            Assert.AreEqual(3, deque.Count);
        }

        [TestMethod]
        public void ClearTest()
        {
            var deque = new Deque<int>();
            deque.Add(7);
            deque.Add(7);
            deque.Add(7);
            deque.Clear();
            Assert.AreEqual(0, deque.Count);
        }

        [TestMethod]
        public void ContainsTest()
        {
            var deque = new Deque<int>();
            deque.Add(6);
            deque.Add(7);
            Assert.IsTrue(deque.Contains(7));
        }

        [TestMethod]
        public void NotContainsTest()
        {
            var deque = new Deque<int>();
            Assert.IsFalse(deque.Contains(7));
        }

        [TestMethod]
        public void RemoveTest()
        {
            var deque = new Deque<int>();
            deque.Add(7);
            deque.Remove(7);
            Assert.AreEqual(0, deque.Count);
        }

        [TestMethod]
        public void LongRemoveTest()
        {
            var deque = new Deque<int>();
            for (int i = 0; i < 125; i++)
            {
                deque.Add(5);
            }
            deque.Add(7);
            for (int i = 0; i < 125; i++)
            {
                deque.Remove(5);
            }
            Assert.AreEqual(deque.Count, 1);
            Assert.AreEqual(deque.PeekLeft(), 7);
        }

        [TestMethod]
        public void RemoveUnexistentTest()
        {
            var deque = new Deque<int>();
            Assert.IsFalse(deque.Remove(5));
        }

        [TestMethod]
        public void PushLeftTest()
        {
            var deque = new Deque<int>();
            for (int i = 0; i < 125; i++)
            {
                deque.PushLeft(5);
            }
            Assert.AreEqual(125, deque.Count);
            Assert.AreEqual(5, deque.PeekLeft());
            Assert.AreEqual(5, deque.PeekRight());
        }

        [TestMethod]
        public void IndexOfTest()
        {
            var deque = new Deque<int>();
            deque.Add(5);
            for (int i = 0; i < 125; i++)
            {
                deque.PushLeft(6);
                deque.Add(6);
            }
            Assert.AreEqual(125, deque.IndexOf(5));
        }

        [TestMethod]
        public void IndexOfCustomComparerTest()
        {
            var deque = new Deque<int>();
            deque.Add(5);
            for (int i = 0; i < 125; i++)
            {
                deque.PushLeft(6);
                deque.Add(6);
            }
            Assert.AreEqual(125, deque.IndexOf(5, System.Collections.Generic.Comparer<int>.Default));
        }

        [TestMethod]
        public void IndexOfNotFoundTest()
        {
            var deque = new Deque<int>();
            deque.Add(5);
            for (int i = 0; i < 125; i++)
            {
                deque.PushLeft(6);
                deque.Add(6);
            }
            Assert.AreEqual(-1, deque.IndexOf(55));
        }

        [TestMethod]
        public void EnumeratorTest()
        {
            var deque = new Deque<int>();
            deque.Add(1);
            deque.Add(2);
            var enumerator = deque.GetEnumerator();
            var i = 1;
            while (enumerator.MoveNext())
            {
                Assert.AreEqual(i, enumerator.Current);               
                i++;
            }
        }

        [TestMethod]
        public void OldEnumeratorTest()
        {
            var deque = new Deque<int>();
            deque.Add(1);
            deque.Add(2);
            var enumerator = ((System.Collections.IEnumerable)deque).GetEnumerator();
            var i = 1;
            while (enumerator.MoveNext())
            {
                Assert.AreEqual(i, enumerator.Current);
                Assert.AreEqual(i, enumerator.Current);
                i++;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void EnumeratorResetTest()
        {
            var deque = new Deque<int>();
            var enumerator = deque.GetEnumerator();
            enumerator.Reset();
        }

        /*[TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EnumeratorUnpreparedTest()
        {
            var deque = new Deque<int>();
            deque.Add(1);
            deque.Add(2);
            var enumerator = deque.GetEnumerator();
            var i = enumerator.Current;
        }*/

        /*[TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EnumeratorEndedTest()
        {
            var deque = new Deque<int>();
            deque.Add(1);
            deque.Add(2);
            var enumerator = deque.GetEnumerator();
            enumerator.MoveNext();
            enumerator.MoveNext();
            enumerator.MoveNext();
            enumerator.MoveNext();
            var i = enumerator.Current;
        }*/

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DequeChangeWhileEnumeratingTest()
        {
            var deque = new Deque<int>();
            deque.Add(1);
            deque.Add(2);
            var enumerator = deque.GetEnumerator();
            enumerator.MoveNext();
            deque.Add(5);
            enumerator.MoveNext();
            var i = enumerator.Current;
        }

        /*[TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void EnumeratorDisposeTest()
        {
            var deque = new Deque<int>();
            deque.Add(1);
            deque.Add(2);
            var enumerator = deque.GetEnumerator();
            enumerator.Dispose();
            enumerator.MoveNext();
        }*/

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EmptyPopLeft()
        {
            var deque = new Deque<int>();
            deque.PopLeft();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EmptyPopRight()
        {
            var deque = new Deque<int>();
            deque.PopRight();
        }

        [TestMethod]
        public void CopyToTest()
        {
            var deque = new Deque<int>();
            var source = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var target = new int[10];
            foreach (var item in source)
            {
                deque.Add(item);
            }
            deque.CopyTo(target, 0);
            for (int i = 0; i < source.Length; i++)
            {
                Assert.AreEqual(source[i], target[i]);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CopyToNullTest()
        {
            var deque = new Deque<int>();
            deque.CopyTo(null, 0);
        }

        [TestMethod]
        public void PopRightTest()
        {
            var deque = new Deque<int>();
            deque.Add(5);
            Assert.AreEqual(5, deque.PopRight());
            Assert.AreEqual(0, deque.Count);
        }

        [TestMethod]
        public void LongPopRightTest()
        {
            var deque = new Deque<int>();
            deque.Add(1);
            for (int i = 0; i < 125; i++)
            {
                deque.Add(5);
            }
            for (int i = 0; i < 125; i++)
            {
                deque.PopRight();
            }
            Assert.AreEqual(1, deque.PopRight());
            Assert.AreEqual(0, deque.Count);
        }

        [TestMethod]
        public void ReadOnlyTest()
        {
            var deque = new Deque<int>();
            Assert.IsFalse(deque.IsReadOnly);
        }

        [TestMethod]
        public void IndexerSetTest()
        {
            var deque = new Deque<int>();
            deque.Add(5);
            deque[0] = 10;
            Assert.AreEqual(deque[0], 10);
        }

        [TestMethod]
        public void RemoveAtLeftTest()
        {
            var deque = new Deque<int>();
            deque.Add(1);
            deque.Add(2);
            deque.Add(3);
            deque.RemoveAt(0);
            Assert.AreEqual(deque[0], 2);
            Assert.AreEqual(deque[1], 3);
        }
        [TestMethod]
        public void RemoveAtRightTest()
        {
            var deque = new Deque<int>();
            deque.Add(1);
            deque.Add(2);
            deque.Add(3);
            deque.RemoveAt(2);
            Assert.AreEqual(deque[0], 1);
            Assert.AreEqual(deque[1], 2);
        }
        [TestMethod]
        public void RemoveAtMiddleTest()
        {
            var deque = new Deque<int>();
            deque.Add(1);
            deque.Add(2);
            deque.Add(3);
            deque.RemoveAt(1);
            Assert.AreEqual(deque[0], 1);
            Assert.AreEqual(deque[1], 3);
        }
        [TestMethod]
        public void LongRemoveAtMiddleTest()
        {
            var deque = new Deque<int>();
            deque.Add(1);
            for (int i = 0; i < 125; i++)
            {
                deque.Add(2);
            }
            deque.Add(3);
            for (int i = 0; i < 125; i++)
            {
                deque.RemoveAt(1);
            }
            
            Assert.AreEqual(deque[0], 1);
            Assert.AreEqual(deque[1], 3);
        }

        [TestMethod]
        public void InsertTest()
        {
            var deque = new Deque<int>();
            for (int i = 0; i < 124; i++)
            {
                deque.Add(2);
            }
            deque.Insert(124 / 2, 5);

            Assert.AreEqual(deque[124/2], 5);
        }

        [TestMethod]
        public void Insert0Test()
        {
            var deque = new Deque<int>();
            for (int i = 0; i < 124; i++)
            {
                deque.Add(2);
            }
            deque.Insert(0, 5);

            Assert.AreEqual(deque[0], 5);
        }
    }
}
