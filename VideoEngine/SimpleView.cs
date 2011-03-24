using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SurfaceTower.Model;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SurfaceTower.VideoEngine
{
    public class SimpleView : View
    {
        private BaseModel baseModel;
        private GraphicsDeviceManager graphics;

        public SimpleView(BaseModel baseModel, GraphicsDeviceManager graphics)
        {
            this.baseModel = baseModel;
            this.graphics = graphics;
            graphics.GraphicsDevice.Clear(Color.BlanchedAlmond);
        }

        public void draw(GameTime gameTime){
            return;
            
        }
    }
}
