using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortVisualizer
{
    using DatasetDistribution = Func<double, double>;
    using DatasetAction = Tuple<DatasetActionType, int, int>;

    public enum DatasetActionType
    {
        ACCESS,
        COMPARE,
        SWAP
    }

    public class Dataset : List<double>
    {
        private readonly DatasetDistribution distribution;

        public bool Recording { get; set; } = false;
        public readonly List<DatasetAction> Record = new List<DatasetAction>();
        
        public Dataset(int numItems, DatasetDistribution dist)
        {
            // Store distribution
            distribution = dist;

            // Distribute data
            Distribute(numItems);
        }

        public void Distribute(int numItems)
        {
            // Clear the list
            Clear();

            // Set the new capacity
            Capacity = numItems;

            // Add all data items
            for (int i = 0; i < numItems; i++)
                Add(distribution.Invoke((i + 1) / (double)numItems));
        }

        public void StartRecord() => Recording = true;
        public void StopRecord() => Recording = false;
        public void ClearRecord() => Record.Clear();

        public double AccessItem(int index)
        {
            if (Recording)
                Record.Add(new DatasetAction(DatasetActionType.ACCESS, index, index));
            return this[index];
        }

        public bool IsGreaterThan(int indexA, int indexB)
        {
            if (Recording)
                Record.Add(new DatasetAction(DatasetActionType.COMPARE, indexA, indexB));
            return this[indexA] > this[indexB];
        }

        public bool IsLessThan(int indexA, int indexB)
        {
            if (Recording)
                Record.Add(new DatasetAction(DatasetActionType.COMPARE, indexA, indexB));
            return this[indexA] < this[indexB];
        }

        public bool IsEqualTo(int indexA, int indexB)
        {
            if (Recording)
                Record.Add(new DatasetAction(DatasetActionType.COMPARE, indexA, indexB));
            return this[indexA] == this[indexB];
        }

        public void SwapItems(int indexA, int indexB)
        {
            if (Recording)
                Record.Add(new DatasetAction(DatasetActionType.SWAP, indexA, indexB));
            double temp = this[indexA];
            this[indexA] = this[indexB];
            this[indexB] = temp;
        }

        public void PerformAction(DatasetAction action)
        {
            switch (action.Item1)
            {
                case DatasetActionType.ACCESS:
                    AccessItem(action.Item2);
                    break;
                case DatasetActionType.COMPARE:
                    IsEqualTo(action.Item2, action.Item3);
                    break;
                case DatasetActionType.SWAP:
                    SwapItems(action.Item2, action.Item3);
                    break;
            }
        }

        public void PerformActions(DatasetAction[] actions, bool reversed=false)
        {
            if (reversed)
            {
                for (int i = actions.Length - 1; i >= 0; i--)
                {
                    PerformAction(actions[i]);
                }
            }
            else
            {
                for (int i = 0; i < actions.Length; i++)
                {
                    PerformAction(actions[i]);
                }
            }
        }
    }
}
