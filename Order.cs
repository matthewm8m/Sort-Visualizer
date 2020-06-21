using System;

namespace SortVisualizer
{
    using DatasetOrderer = Func<int, int, int>;

    public static class Order
    {
        private static readonly Random OrderRandom = new Random();

        public static readonly DatasetOrderer SortedOrder = (n, x) => x;
        public static readonly DatasetOrderer AlmostSortedOrder = (n, x) => (x + 1) == n ? x : x + 1;
        public static readonly DatasetOrderer ReversedOrder = (n, x) => x < (n / 2) ? n - x - 1 : x;
        public static readonly DatasetOrderer RandomOrder = (n, x) => OrderRandom.Next(n);
        public static readonly DatasetOrderer EvenOddOrder = (n, x) => (x % 2 == 0 && (x + 1) != n) ? x + 1 : x;
    }
}
