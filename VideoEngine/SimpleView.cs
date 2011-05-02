using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SurfaceTower.Model;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SurfaceTower.VideoEngine
{
    public class SimpleView : View
    {
        private BaseModel baseModel;
        private GraphicsDeviceManager graphics;
        private ContentManager content;
        private SpriteBatch spritebatch;
        private Texture2D enemy;
        private Texture2D bullet;
        private Texture2D middle;

        public SimpleView(BaseModel baseModel, GraphicsDeviceManager graphics, ContentManager content)
        {
            this.baseModel = baseModel;
            this.graphics = graphics;
            graphics.GraphicsDevice.Clear(Color.BlanchedAlmond);
            spritebatch = new SpriteBatch(graphics.GraphicsDevice);
            enemy = content.Load<Texture2D>("enemy");
            bullet = content.Load<Texture2D>("bullet");
            middle = content.Load<Texture2D>("centre");
        }

        public void draw(GameTime gameTime){
            graphics.GraphicsDevice.Clear(Color.HotPink);
            spritebatch.Begin();
            foreach (Enemy e in baseModel.Living){
                spritebatch.Draw(enemy, new Rectangle(e.X, e.Y, enemy.Width, enemy.Height), Color.White);
            }
            foreach (Bullet b in baseModel.Bullets)
            {
                spritebatch.Draw(bullet, new Rectangle(b.X, b.Y, 20, 20), null, Color.White, b.Rotation, new Vector2(bullet.Width/2, bullet.Height/2), SpriteEffects.None, 0f);
            }

            spritebatch.End();

            return;
            
        }
    }
}
