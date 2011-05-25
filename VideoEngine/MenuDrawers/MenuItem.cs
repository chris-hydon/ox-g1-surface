using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SurfaceTower.VideoEngine.MenuDrawers
{
    public class MenuItem : IMenuItem
    {
        private Texture2D menuImage;

        public MenuItem(Texture2D menuImage)
        {
            this.menuImage = menuImage;
        }

        public void Draw(SpriteBatch sb, Vector2 position)
        {
            sb.Draw(menuImage, position, null, Color.White, 0f, new Vector2(menuImage.Height / 2, menuImage.Width / 2), 1f, SpriteEffects.None, 0);
        }

    }
}
