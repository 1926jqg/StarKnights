using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StarKnightsLibrary.UtilityObjects.Extensions
{
    public static class SpriteBatchExtensions
    {
        /// <summary> 
        /// Will draw a border (hollow rectangle) of the given 'thicknessOfBorder' (in pixels) 
        /// of the specified color. 
        /// </summary> 
        /// <param name="rectangleToDraw"></param> 
        /// <param name="thicknessOfBorder"></param> 
        public static void DrawBorder(this SpriteBatch spriteBatch, IContentContainer contentContainer, Rectangle rectangleToDraw, int thicknessOfBorder, Color borderColor)
        {
            var pixel = contentContainer.LoadTexture("Pixel");

            // Draw top line 
            spriteBatch.Draw(pixel, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, rectangleToDraw.Width, thicknessOfBorder), borderColor);

            // Draw left line 
            spriteBatch.Draw(pixel, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, thicknessOfBorder, rectangleToDraw.Height), borderColor);

            // Draw right line 
            spriteBatch.Draw(pixel, new Rectangle((rectangleToDraw.X + rectangleToDraw.Width - thicknessOfBorder),
            rectangleToDraw.Y,
            thicknessOfBorder,
            rectangleToDraw.Height), borderColor);
            // Draw bottom line 
            spriteBatch.Draw(pixel, new Rectangle(rectangleToDraw.X,
            rectangleToDraw.Y + rectangleToDraw.Height - thicknessOfBorder,
            rectangleToDraw.Width,
            thicknessOfBorder), borderColor);
        }

        public static void DrawLine(this SpriteBatch spriteBatch, IContentContainer contentContainer, Vector2 start, float angle, int length)
        {
            spriteBatch.Draw(contentContainer.LoadTexture("Pixel"),
                new Rectangle(// rectangle defines shape of line and position of start of line
                    (int)start.X,
                    (int)start.Y,
                    length, //sb will strech the texture to fill this rectangle
                    1), //width of line, change this to make thicker line
                null,
                Color.Red, //colour of line
                angle,     //angle of line
                new Vector2(0, 0), // point in line about which to rotate
                SpriteEffects.None,
                0);
        }
    }
}
