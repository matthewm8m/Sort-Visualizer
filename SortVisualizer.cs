using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SortVisualizer
{
    // Alias definitions
    using DatasetOrderer = Func<int, int, int>;
    using DatasetSorter = Action<Dataset>;
    using DatasetAction = Tuple<DatasetActionType, int, int>;

    public class SortVisualizer
    {
        public enum VisualizerDirection
        {
            FORWARD,
            BACKWARD
        }

        // Data values, ordering, sorting variables
        public Dataset Data { get; }
        public DatasetOrderer Order { get; }
        public DatasetSorter Sort { get; }

        // Graphics variables
        public Rectangle Bounds { get; set; }
        public DataBarTexture Texture { get; set; }

        // Logic variables
        public VisualizerDirection Direction { get; set; } = VisualizerDirection.FORWARD;

        // Internal logic variables
        private DatasetAction[] SortActions;
        private DatasetAction CurrentSortAction;
        private int SortActionIndex = 0;

        public SortVisualizer(Dataset data, DatasetOrderer orderer, DatasetSorter sorter, Rectangle bounds, DataBarTexture texture)
        {
            Data = data;
            Order = orderer;
            Sort = sorter;

            Bounds = bounds;
            Texture = texture;

            RecordOrderSort();
        }

        private void RecordOrderSort()
        {
            Data.ClearRecord();
            Data.StartRecord();

            for (int i = 0; i < Data.Count; i++)
            {
                Data.SwapItems(i, Order.Invoke(Data.Count, i));
            }
            Sort.Invoke(Data);

            Data.StopRecord();

            SortActions = new DatasetAction[Data.Record.Count];
            Data.Record.CopyTo(SortActions);

            Data.PerformActions(SortActions, reversed: true);
        }

        public void Update(GameTime gameTime)
        {
            switch (Direction)
            {
                case VisualizerDirection.FORWARD:
                    if (SortActionIndex < SortActions.Length)
                    {
                        CurrentSortAction = SortActions[SortActionIndex];
                        Data.PerformAction(SortActions[SortActionIndex++]);
                    }
                    else
                        CurrentSortAction = null;
                    break;
                case VisualizerDirection.BACKWARD:
                    if (SortActionIndex > 0)
                    {
                        Data.PerformAction(SortActions[--SortActionIndex]);
                        CurrentSortAction = SortActions[SortActionIndex];
                    }
                    else
                        CurrentSortAction = null;
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Texture != null)
            {
                // Deal with determining special colors based on current action.
                Color specialColor = Color.White;
                if (CurrentSortAction != null)
                {
                    switch (CurrentSortAction.Item1)
                    {
                        case DatasetActionType.ACCESS:
                            specialColor = Color.Red;
                            break;
                        case DatasetActionType.COMPARE:
                            specialColor = Color.Green;
                            break;
                        case DatasetActionType.SWAP:
                            specialColor = Color.Blue;
                            break;
                    }
                }

                double barWidth = (double)Bounds.Width / Data.Count;

                for (int i = 0; i < Data.Count; i++)
                {
                    int barHeight = (int)(Bounds.Height * Data[i]);

                    int barX1 = (int)(barWidth * i) + Bounds.X;
                    int barX2 = (int)(barWidth * (i + 1)) + Bounds.X;
                    int barY = Bounds.Bottom - barHeight;

                    if (CurrentSortAction != null && (i == CurrentSortAction.Item2 || i == CurrentSortAction.Item3))
                        Texture.DrawBar(spriteBatch, new Rectangle(barX1, barY, barX2 - barX1, barHeight), specialColor);
                    else
                        Texture.DrawBar(spriteBatch, new Rectangle(barX1, barY, barX2 - barX1, barHeight), Color.White);
                }
            }
        }
    }
}
