using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SortVisualizer
{
    public class DataBarTexture
    {
        // Texture fields
        private readonly Texture2D Texture;
        private readonly Rectangle TopSource;
        private readonly Rectangle MiddleSource;
        private readonly Rectangle BottomSource;
        
        // Aspect ratio fields
        private readonly double AspectRatio;
        private readonly double TopAspectRatio;
        private readonly double BottomAspectRatio;

        public DataBarTexture(Texture2D texture, int topHeight, int bottomHeight)
        {
            // Set texture of bar
            Texture = texture;

            // Set source bounds of each part of the bar
            TopSource = new Rectangle(0, 0, texture.Width, topHeight);
            MiddleSource = new Rectangle(0, topHeight, texture.Width, texture.Height - topHeight - bottomHeight);
            BottomSource = new Rectangle(0, texture.Height - bottomHeight, texture.Width, bottomHeight);

            // Set aspect ratios of parts of bar
            TopAspectRatio = topHeight / (double)texture.Width;
            BottomAspectRatio = bottomHeight / (double)texture.Width;
            AspectRatio = TopAspectRatio + BottomAspectRatio;
        }

        public void DrawBar(SpriteBatch spriteBatch, Rectangle bounds, Color color)
        {
            double boundsAspectRatio = bounds.Height / (double)bounds.Width;

            if (boundsAspectRatio < AspectRatio)
            {
                // Draw squeezed version of bar
                spriteBatch.Draw(Texture, bounds, color);
            }
            else
            {
                // Get proportional sizes of bar segments
                int topHeight = (int)(TopAspectRatio * bounds.Width);
                int bottomHeight = (int)(BottomAspectRatio * bounds.Width);

                // Get resulting rectangles for bar
                Rectangle topRect = new Rectangle(bounds.X, bounds.Y, bounds.Width, topHeight);
                Rectangle bottomRect = new Rectangle(bounds.X, bounds.Bottom - bottomHeight, bounds.Width, bottomHeight);
                Rectangle middleRect = new Rectangle(bounds.X, topRect.Bottom, bounds.Width, bottomRect.Y - topRect.Bottom);

                // Draw segmented version of bar
                spriteBatch.Draw(Texture, topRect, TopSource, color);
                spriteBatch.Draw(Texture, middleRect, MiddleSource, color);
                spriteBatch.Draw(Texture, bottomRect, BottomSource, color);
            }
        }
    }
}
