using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using Microsoft.Surface.Core;

namespace SurfaceTower.VideoEngine.MenuDrawers
{
    public class MenuItem : IMenuItem
    {
        private Texture2D menuImage;
        public Vector2 position;

        public MenuItem(Texture2D menuImage, Vector2 position)
        {
            this.position = position;
            this.menuImage = menuImage;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(menuImage, position, null, Color.White, 0f, new Vector2(menuImage.Height / 2, menuImage.Width / 2), 1f, SpriteEffects.None, 0);
        }

        public bool IsHit(Contact c)
        {
            Vector2 touch = new Vector2(c.CenterX, c.CenterY);
            bool hit;
            hit = (touch.X > (position.X - menuImage.Width/2)) && (touch.X < (position.X + menuImage.Width/2)) && (touch.Y > (position.Y - menuImage.Height/2)) 
                && (touch.Y < (position.Y + menuImage.Height/2));
            return hit;
        }

    }
}
