using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortVisualizer
{
    using DatasetSorter = Action<Dataset>;

    public static class Sort
    {
        public static readonly DatasetSorter BubbleSorter = data =>
        {
            bool swapped = true;
            for (int i = data.Count; swapped; i--)
            {
                swapped = false;
                data.AccessItem(0);
                for (int j = 1; j < i; j++)
                {
                    data.AccessItem(j);
                    if (data.IsGreaterThan(j - 1, j))
                    {
                        data.SwapItems(j - 1, j);
                        swapped = true;
                    }
                }
            }
        };
        public static readonly DatasetSorter InsertionSorter = data =>
        {
            for (int i = 1; i < data.Count; i++)
            {
                data.AccessItem(i);
                for (int j = i; j > 0; j--)
                {
                    data.AccessItem(j - 1);
                    if (!data.IsGreaterThan(j - 1, j))
                        break;
                    data.SwapItems(j - 1, j);
                }
            }
        };
        public static readonly DatasetSorter SelectionSorter = data =>
        {
            for (int i = 0; i < data.Count - 1; i++)
            {
                data.AccessItem(i);
                int m = i;
                for (int j = i + 1; j < data.Count; j++)
                {
                    data.AccessItem(j);
                    if (data.IsGreaterThan(m, j))
                    {
                        m = j;
                    }
                }
                data.SwapItems(i, m);
            }
        };
        public static readonly DatasetSorter DoubleSelectionSorter = data =>
        {
            for (int i = 0; i < data.Count; i++)
            {
                if (i + 1 >= data.Count - i)
                    break;

                data.AccessItem(i);
                int min = i;
                int max = i;
                for (int j = i + 1; j < data.Count - i; j++)
                {
                    data.AccessItem(j);
                    if (data.IsGreaterThan(min, j))
                    {
                        min = j;
                    }
                    if (data.IsLessThan(max, j))
                    {
                        max = j;
                    }
                }

                if (max == i)
                {
                    max = min;
                }

                data.SwapItems(i, min);
                data.SwapItems(data.Count - i - 1, max);
            }

        };
        public static readonly DatasetSorter CocktailShakerSorter = data =>
        {
            int begin = 0;
            int end = data.Count;
            
            while (begin < end)
            {
                int beginNew = end;
                int endNew = begin;

                data.AccessItem(begin);
                for (int i = begin; i < end - 1; i++)
                {
                    data.AccessItem(i + 1);
                    if (data.IsGreaterThan(i, i + 1))
                    {
                        data.SwapItems(i, i + 1);
                        endNew = i + 1;
                    }
                }

                end = endNew;

                for (int i = end - 1; i > begin; i--)
                {
                    data.AccessItem(i - 1);
                    if (data.IsGreaterThan(i - 1, i))
                    {
                        data.SwapItems(i - 1, i);
                        beginNew = i;
                    }
                }

                begin = beginNew;
            }
        };
        public static readonly DatasetSorter GnomeSorter = data =>
        {
            int i = 0;
            data.AccessItem(0);
            while (i < data.Count)
            {
                if (i == 0)
                {
                    i++;
                }
                else
                {
                    data.AccessItem(i - 1);
                    if (!data.IsLessThan(i, i - 1))
                        i++;
                    else
                    {
                        data.SwapItems(i, i - 1);
                        i--;
                    }
                }
            }
        };
        public static readonly DatasetSorter MergeSorter = data =>
        {
            // Create working array to handle merging
            int[] working = new int[data.Count];

            void ResolveSwap(int start, int end)
            {
                for (int i = start; i < end; i++)
                {
                    while (working[i] < i)
                    {
                        working[i] = working[working[i]];
                    }
                    data.SwapItems(i, working[i]);
                }
            }
            
            void MergeSublists(int start, int pivot, int end)
            {
                int i = start;
                int j = pivot;

                data.AccessItem(i);
                data.AccessItem(j);

                for (int k = start; k < end; k++)
                {
                    if (i < pivot && (j >= end || !data.IsGreaterThan(i, j)))
                    {
                        working[k] = i++;
                        if (i < pivot)
                            data.AccessItem(i);
                    }
                    else
                    {
                        working[k] = j++;
                        if (j < end)
                            data.AccessItem(j);
                    }
                }

                ResolveSwap(start, end);
            }

            void MergeSortRecursive(int start, int end)
            {
                // Break up data longer than one element into sorted subarrays
                if (end - start > 1)
                {
                    int pivot = (start + end) / 2;

                    MergeSortRecursive(start, pivot);
                    MergeSortRecursive(pivot, end);

                    MergeSublists(start, pivot, end);
                }
            }
            MergeSortRecursive(0, data.Count);
        };
        public static readonly DatasetSorter QuickSorter = data =>
        {
            int Partition(int start, int end)
            {
                int pivotPosition = start;
                int i = start - 1;
                int j = end + 1;
                data.AccessItem(pivotPosition);

                while (true)
                {
                    do
                    {
                        i++;
                        data.AccessItem(i);
                    } while (data.IsLessThan(i, pivotPosition));

                    do
                    {
                        j--;
                        data.AccessItem(j);
                    } while (data.IsGreaterThan(j, pivotPosition));

                    if (i >= j)
                        return j;
                    data.SwapItems(i, j);
                }
            }

            void QuickSortRecursive(int start, int end)
            {
                if (start < end)
                {
                    int partitionIndex = Partition(start, end);

                    QuickSortRecursive(start, partitionIndex);
                    QuickSortRecursive(partitionIndex + 1, end);
                }
            }
            QuickSortRecursive(0, data.Count - 1);
        };
    }
}
