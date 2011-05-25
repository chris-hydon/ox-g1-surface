using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SurfaceTower.VideoEngine
{
    public interface View
    {
         void draw(GameTime gameTime);
         void Restart();
    }
}
