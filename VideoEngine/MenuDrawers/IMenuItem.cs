using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using Microsoft.Surface.Core;

namespace SurfaceTower.VideoEngine.MenuDrawers
{
    public interface IMenuItem
    {
        void Draw(SpriteBatch sb);
        bool IsHit(Contact c);
    }
}
