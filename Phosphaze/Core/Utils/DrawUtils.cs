using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Phosphaze.Core.Utils
{
    public class DrawUtils
    {

        /// <summary>
        /// A 1x1 solid colour texture that can be scaled to draw filled rectangular regions of colour.
        /// </summary>
        public static Texture2D SolidFill = new Texture2D(
            Globals.graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color); 

        public static void DrawBorder(
            Rectangle rectangleToDraw, int thicknessOfBorder, Color borderColor)
        {
            // Draw top line
            Globals.spriteBatch.Draw(SolidFill, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, rectangleToDraw.Width, thicknessOfBorder), borderColor);

            // Draw left line
            Globals.spriteBatch.Draw(SolidFill, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, thicknessOfBorder, rectangleToDraw.Height), borderColor);

            // Draw right line
            Globals.spriteBatch.Draw(SolidFill, new Rectangle((rectangleToDraw.X + rectangleToDraw.Width - thicknessOfBorder),
                                            rectangleToDraw.Y,
                                            thicknessOfBorder,
                                            rectangleToDraw.Height), borderColor);
            // Draw bottom line
            Globals.spriteBatch.Draw(SolidFill, new Rectangle(rectangleToDraw.X,
                                            rectangleToDraw.Y + rectangleToDraw.Height - thicknessOfBorder,
                                            rectangleToDraw.Width,
                                            thicknessOfBorder), borderColor);
        }


        public static void DrawCentered(Texture2D texture, Vector2 Position, Color Tint)
        {
            Point center = texture.Bounds.Center;
            Globals.spriteBatch.Draw(texture, Position - new Vector2(center.X, center.Y), Tint);
        }

    }

    public static class SpriteBatchExtensions
    {
        public static void DrawStringCentered(this SpriteBatch sb, SpriteFont font, string text, Vector2 center, Color color)
        {
            var offset = font.MeasureString(text);
            sb.DrawString(font, text, center - offset / 2f, color);
        }
    }
}
