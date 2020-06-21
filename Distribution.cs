using System;

namespace SortVisualizer
{
    using DatasetDistribution = Func<double, double>;

    public static class Distribution
    {
        public static readonly DatasetDistribution LinearDistribution = x => x;
        public static readonly DatasetDistribution SquareDistribution = x => Math.Pow(x, 2.0);
        public static readonly DatasetDistribution RootDistribution = x => Math.Pow(x, 0.5);
        public static readonly DatasetDistribution ConstantDistribution = x => 0.5;
        public static readonly DatasetDistribution SplitDistribution = x => x <= 0.5 ? 0.25 : 0.75;
        public static readonly DatasetDistribution SmoothDistribution = x => x * x * (3 - 2 * x);
        public static readonly DatasetDistribution SmootherDistribution = x => x * x * x * (x * (6 * x - 15) + 10);
    }
}
