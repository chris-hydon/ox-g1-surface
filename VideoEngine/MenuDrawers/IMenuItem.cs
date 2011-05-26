using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SurfaceTower.VideoEngine.MenuDrawers
{
    public interface IMenuItem
    {
        void Draw(SpriteBatch sb, Vector2 position);
    }
}
