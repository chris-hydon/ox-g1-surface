using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;

namespace SurfaceTower.VideoEngine.MenuDrawers
{
    public interface IMenu
    {
        void Draw(SpriteBatch sb);
        void Close();
    }
}
