using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SurfaceTower.Model;
using SurfaceTower.VideoEngine.ParticleEngine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SurfaceTower.VideoEngine
{
    public class SimpleView : View
    {
        #region Fields
        //You just lost this
        private Game gm;
        //The game model
        private BaseModel baseModel;
        //Graphics Manager
        private GraphicsDeviceManager graphics;
        //Content pipeline
        public ContentManager content { get; set; }
        //Draws the sprites
        public SpriteBatch spritebatch { get; set; }
        //Sprites
        private Texture2D enemy, bullet, middle;
        //Postprocessing to apply bloom
        private BloomPostprocess.BloomComponent bloom; 
        //The particle engine
        private PEngine particleEngine;
        #endregion
        

        public SimpleView(BaseModel baseModel, GraphicsDeviceManager graphics, ContentManager content)
        {
            this.baseModel = baseModel;
            this.graphics = graphics;
            this.gm = App.Instance;
            this.content = content;
            //initializes the sprite batch
            spritebatch = new SpriteBatch(graphics.GraphicsDevice);
            //Loads the sprites
            enemy = content.Load<Texture2D>("enemy");
            bullet = content.Load<Texture2D>("bullet");
            middle = content.Load<Texture2D>("centre");
            graphics.GraphicsDevice.Clear(Color.Black);
            //Initializes the bloom postprocessing with the device to display on
            bloom = new BloomPostprocess.BloomComponent(gm, graphics.GraphicsDevice);
            //Registers the bloom compenent to the list of compenents, it will be drawn AFTER the rest of the game.
            gm.Components.Add(bloom);
            particleEngine = new PEngine(this);
            particleEngine.addEmitter(new Vector2(300));
            particleEngine.addEmitter(new Vector2(500));
            particleEngine.addEmitter(new Vector2(100));
            particleEngine.addEmitter(new Vector2(600));
            particleEngine.addExplosion(new Vector2(App.Instance.GraphicsDevice.Viewport.Width / 2, App.Instance.GraphicsDevice.Viewport.Height / 2));
        }

        public void draw(GameTime gameTime){


            //Clear the previous frame 
            graphics.GraphicsDevice.Clear(Color.Black);

            particleEngine.Update(baseModel.Living);

          
            spritebatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);
            //Particles
            particleEngine.Draw();
            //Living
            foreach (Enemy e in baseModel.Living){
                spritebatch.Draw(enemy, new Rectangle(e.X, e.Y, enemy.Width, enemy.Height), Color.White);
            }

            //bullets
            foreach (Bullet b in baseModel.Bullets)
            {
                spritebatch.Draw(bullet, new Rectangle(b.X, b.Y, 20, 20), null, Color.White, b.Rotation, new Vector2(bullet.Width/2, bullet.Height/2), SpriteEffects.None, 0f);
            }
            spritebatch.End();
            return;
            
        }
    }
}
