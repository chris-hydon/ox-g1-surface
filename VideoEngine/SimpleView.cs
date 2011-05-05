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
        #region Fields
        //You just lost this
        private Game gm;
        //The game model
        private BaseModel baseModel;
        //Graphics Manager
        private GraphicsDeviceManager graphics;
        //Content pipeline
        private ContentManager content;
        //Draws the sprites
        private SpriteBatch spritebatch;
        //Sprites
        private Texture2D enemy, bullet, middle;
        //Postprocessing to apply bloom
        private BloomPostprocess.BloomComponent bloom; 
        #endregion
        

        public SimpleView(BaseModel baseModel, GraphicsDeviceManager graphics, ContentManager content)
        {
            this.baseModel = baseModel;
            this.graphics = graphics;
            this.gm = App.Instance;
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
            
        }

        public void draw(GameTime gameTime){
            //Clear the previous frame 
            graphics.GraphicsDevice.Clear(Color.Black);
          
            spritebatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);
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
