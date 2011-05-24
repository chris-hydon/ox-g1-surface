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

        private const int BIG_GUN_SIZE = 30;
        private Color[] player_colors = new Color[4] {Color.Red, Color.ForestGreen, Color.Teal, Color.Gold};
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
        private Texture2D enemy, bullet, middle, gun, background;
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
            gun = content.Load<Texture2D>("turret");
            background = content.Load<Texture2D>("bg");
            graphics.GraphicsDevice.Clear(Color.Black);
            //Initializes the bloom postprocessing with the device to display on
            bloom = new BloomPostprocess.BloomComponent(gm, graphics.GraphicsDevice);
            //Registers the bloom compenent to the list of compenents, it will be drawn AFTER the rest of the game.
            gm.Components.Add(bloom);
            particleEngine = new PEngine(this);
        }

        public void Restart()
        {
            particleEngine.Reset();
        }

        public void draw(GameTime gameTime){

            //Clear the previous frame 
            graphics.GraphicsDevice.Clear(Color.Black);
           
            particleEngine.Update();

            spritebatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);

            //Particles
            spritebatch.Draw(background, new Rectangle(0,0,graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height), Color.White);
            particleEngine.Draw();


            //middle
            Color c = Color.DeepPink;
            c.A = (byte)100;
            spritebatch.Draw(middle, new Rectangle((int)baseModel.Tower.Location.X, (int)baseModel.Tower.Location.Y, (int)baseModel.Tower.Shape.Width, (int)baseModel.Tower.Shape.Height),
                null, c, 0, new Vector2(middle.Width / 2, middle.Height / 2), SpriteEffects.None, 0);

            //Guns
            foreach (Model.Gun.Turret t in baseModel.Turrets)
            {
                spritebatch.Draw(gun, new Rectangle((int)t.Location.X, (int)t.Location.Y, (int)t.Shape.Width, (int)t.Shape.Height), new Rectangle(0, 0, gun.Width, gun.Height),
                    player_colors[t.PlayerId], t.Orientation, new Vector2(gun.Width / 2, gun.Height / 2), SpriteEffects.None, 1);
            }
            foreach (Model.Gun.MainGun m in baseModel.Players)
            {
                if (m.IsActive)
                {
                    spritebatch.Draw(gun, new Rectangle((int)m.Location.X, (int)m.Location.Y, BIG_GUN_SIZE, BIG_GUN_SIZE), new Rectangle(0, 0, gun.Width, gun.Height),
                      player_colors[m.PlayerId], m.Orientation, new Vector2(gun.Width / 2, gun.Height / 2), SpriteEffects.None, 1);
                }
            }

            //Living
            foreach (Enemy e in baseModel.Living)
            {
                Rectangle rect = new Rectangle((int)e.Location.X, (int)e.Location.Y, (int)e.Shape.Width, (int)e.Shape.Height);
                spritebatch.Draw(enemy, rect, new Rectangle(0, 0, enemy.Width, enemy.Height), e.Colour, e.Orientation, new Vector2(enemy.Width / 2, enemy.Height / 2), SpriteEffects.None, 1);
            }
            //dying
             foreach (EnemyTimeWho et in baseModel.Dying){
                 Enemy e = et.enemy;
                Rectangle rect = new Rectangle((int)e.Location.X, (int)e.Location.Y, (int)e.Shape.Width, (int)e.Shape.Height);
                spritebatch.Draw(enemy, rect, new Rectangle(0,0,enemy.Width, enemy.Height), e.Colour, e.Orientation, new Vector2(enemy.Width / 2, enemy.Height / 2), SpriteEffects.None, 1);
            }



            //bullets
            foreach (Bullet b in baseModel.Bullets)
            {
                Rectangle rect = new Rectangle((int)b.Location.X, (int)b.Location.Y, (int)(2*b.Shape.Width), (int)(2*b.Shape.Height));
                spritebatch.Draw(bullet, rect, new Rectangle(0, 0, bullet.Width, bullet.Height), player_colors[b.PlayerId], b.Orientation, new Vector2(bullet.Width / 2, bullet.Height / 2), SpriteEffects.None, 1);
            }

            spritebatch.End();
            return;
            
            
        }
    }
}
