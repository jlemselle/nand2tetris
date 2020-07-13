using System;
using Xunit;

namespace TDD
{
    public class UnitTest1
    {
        int[] heap = new int[100];
        public int listEnd = -1; // Value representing the end of the list
        public int headPtr; // Pointer to the first element in the list

        [Fact]
        public void TestCanAllocateFromHead()
        {
            // 0  : headPtr : [listEnd]
            // 1  :         : [9]
            // 2  :         : [ ]
            // 3  :         : [ ]
            // 4  :         : [ ]
            // 5  :         : [ ]
            // 6  :         : [ ]
            // 7  :         : [ ]
            // 8  :         : [ ]
            // 9  :         : [ ]
            // 10 :         : [ ]

            ArrangePointer(out headPtr, addr: 0, next: listEnd, size: 9);

            // Allocate 3 from free space
            int baseAddr = alloc(3);

            // 0  :          : [0]
            // 1  :          : [3]
            // 2  : baseAddr : [.]
            // 3  :          : [.]
            // 4  :          : [.]
            // 5  : headPtr  : [listEnd]
            // 6  :          : [4]
            // 7  :          : [ ]
            // 8  :          : [ ]
            // 9  :          : [ ]
            // 10 :          : [ ]

            AssertPointer(headPtr, addr: 5, next: listEnd, size: 4);

            Assert.Equal(2, baseAddr);

            // Allocate 2 from free space
            int baseAddr2 = alloc(2);

            // 0  :           : [0]
            // 1  :           : [3]
            // 2  : baseAddr  : [.]
            // 3  :           : [.]
            // 4  :           : [.]
            // 5  :           : [0]
            // 6  :           : [2]
            // 7  : baseAddr2 : [.]
            // 8  :           : [.]
            // 9  : headPtr   : [listEnd]
            // 10 :           : [0]

            AssertPointer(headPtr, addr: 9, next: listEnd, size: 0);

            Assert.Equal(7, baseAddr2);
        }

        [Fact]
        public void TestCanAllocateAfterHead()
        {
            // 0  : headPtr : [2]
            // 1  :         : [0]
            // 2  : nextPtr : [listEnd]
            // 3  :         : [7]
            // 4  :         : [ ]
            // 5  :         : [ ]
            // 6  :         : [ ]
            // 7  :         : [ ]
            // 8  :         : [ ]
            // 9  :         : [ ]
            // 10 :         : [ ]

            ArrangePointer(out int nextPtr, addr: 2, next: listEnd, size: 7);
            ArrangePointer(out headPtr, addr: 0, next: nextPtr, size: 0);

            // Allocate 3 from free space
            int baseAddr = alloc(3);

            // 0  : headPtr  : [7]
            // 1  :          : [0]
            // 2  :          : [0]
            // 3  :          : [3]
            // 4  : baseAddr : [.]
            // 5  :          : [.]
            // 6  :          : [.]
            // 7  : nextPtr  : [listEnd]
            // 8  :          : [2]
            // 9  :          : [ ]
            // 10 :          : [ ]

            AssertPointer(ptr: 7, next: listEnd, size: 2);
            AssertPointer(headPtr, addr: 0, next: 7, size: 0);
            Assert.Equal(4, baseAddr);
        }

        [Fact]
        public void TestCannotAllocate()
        {
            // 0  : headPtr : [2]
            // 1  :         : [0]
            // 2  : nextPtr : [listEnd]
            // 3  :         : [7]
            // 4  :         : [ ]
            // 5  :         : [ ]
            // 6  :         : [ ]
            // 7  :         : [ ]
            // 8  :         : [ ]
            // 9  :         : [ ]
            // 10 :         : [ ]

            ArrangePointer(out int nextPtr, addr: 2, next: listEnd, size: 7);
            ArrangePointer(out headPtr, addr: 0, next: nextPtr, size: 0);

            Assert.Throws<Exception>(() => alloc(10));
        }

        public int alloc(int size)
        {
            int prevPtr, ptr, segmentSize, baseAddr, newSize, newNext, newPtr;

            prevPtr = listEnd;
            ptr = headPtr;
            segmentSize = size + 2;
            while (ptr != listEnd && heap[ptr + 1] < segmentSize)
            {
                prevPtr = ptr;
                ptr = heap[ptr];
            }

            if (ptr == listEnd)
            {
                throw new Exception();
            }

            baseAddr = ptr + 2;
            newSize = heap[ptr + 1] - segmentSize;
            newNext = heap[ptr];
            newPtr = ptr + segmentSize;

            if (ptr == headPtr)
            {
                headPtr = newPtr;
            }
            if (prevPtr != listEnd)
            {
                heap[prevPtr] = newPtr;
            }

            heap[newPtr] = newNext;
            heap[newPtr + 1] = newSize;

            return baseAddr;
        }

        public int getSize(int ptr)
        {
            return heap[ptr + 1];
        }

        public int getBase(int ptr)
        {
            return ptr + 2;
        }

        public void setNext(int ptr, int val)
        {
            heap[ptr] = val;
        }

        public void setSize(int ptr, int val)
        {
            heap[ptr + 1] = val;
        }

        public void ArrangePointer(out int ptr, int addr, int next, int size)
        {
            ptr = addr;
            setNext(ptr, next);
            setSize(ptr, size);
        }

        public void AssertPointer(int ptr, int addr, int next, int size)
        {
            Assert.Equal(addr, ptr);
            Assert.Equal(next, heap[ptr]);
            Assert.Equal(size, getSize(ptr));
        }

        public void AssertPointer(int ptr, int next, int size)
        {
            Assert.Equal(next, heap[ptr]);
            Assert.Equal(size, getSize(ptr));
        }
    }
}
