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
        private Texture2D enemy, bullet, middle, middle0, middle1, middle2, gun1, gun2, gun3, background, boss;
        //Postprocessing to apply bloom
        private BloomPostprocess.BloomComponent bloom; 
        //The particle engine
        private PEngine particleEngine;
        //The Menu Engine
        private MenuDrawers.MenuManager menuManager;
        //The tower frame id
        private float frameId = 0;
        #endregion
        

        public SimpleView(BaseModel baseModel, GraphicsDeviceManager graphics, ContentManager content)
        {
            this.baseModel = baseModel;
            this.graphics = graphics;
            this.gm = (App) App.Instance;
            this.content = content;
            //initializes the sprite batch
            spritebatch = new SpriteBatch(graphics.GraphicsDevice);
            //Loads the sprites
            enemy = content.Load<Texture2D>("Drone");
            bullet = content.Load<Texture2D>("bullet");
            middle = content.Load<Texture2D>("centre");
            middle0 = content.Load<Texture2D>("tower-0");
            middle1 = content.Load<Texture2D>("tower-1");
            middle2 = content.Load<Texture2D>("tower-2");
            gun1 = content.Load<Texture2D>("turret");
            gun2 = content.Load<Texture2D>("2turret");
            gun3 = content.Load<Texture2D>("3turret");
            boss = content.Load<Texture2D>("spaceinvader");
            background = content.Load<Texture2D>("bg");
            graphics.GraphicsDevice.Clear(Color.Black);
            //Initializes the bloom postprocessing with the device to display on
            bloom = new BloomPostprocess.BloomComponent(gm, graphics.GraphicsDevice);
            //Registers the bloom compenent to the list of compenents, it will be drawn AFTER the rest of the game.
            gm.Components.Add(bloom);
            particleEngine = new PEngine(this);
            menuManager = new MenuDrawers.MenuManager();
            baseModel.Tower.ZeroHealth += new EventHandler(OnDeath);
        }

        public void Restart()
        {
            particleEngine.Reset();
            menuManager.reset();
            App.Instance.Model.Tower.ZeroHealth += new EventHandler(OnDeath);
        }

        public void draw(GameTime gameTime){

            //Clear the previous frame 
            graphics.GraphicsDevice.Clear(Color.Black);
           
            particleEngine.Update();

            spritebatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);

            //Particles
            spritebatch.Draw(background, new Rectangle(0,0,graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height), Color.White);
            particleEngine.Draw();

            menuManager.Draw(spritebatch);


            //middle
            //Color c = Color.DeepPink;
            //c.A = (byte)100;
            //spritebatch.Draw(middle, new Rectangle((int)baseModel.Tower.Location.X, (int)baseModel.Tower.Location.Y, (int)baseModel.Tower.Shape.Width, (int)baseModel.Tower.Shape.Height),
            //    null, c, 0, new Vector2(middle.Width / 2, middle.Height / 2), SpriteEffects.None, 0);
            if (!baseModel.Tower.Dead)
            {
              int width = (int) baseModel.Tower.Shape.Width;
              int height = (int) baseModel.Tower.Shape.Height;

              frameId = (frameId + 0.2f) % 50;

              int healthId = 7 - (int) (((float) baseModel.Tower.Health / baseModel.Tower.MaxHealth) * 7);
              if (healthId > 6)
              {
                healthId = 6;
              }

              Color c = Color.White;
              c.A = 200;
              Texture2D t = frameId < 20 ? middle0 : (frameId < 40 ? middle1 : middle2);
              Rectangle frame = new Rectangle(width * (((int) frameId) % 20), height * healthId, width, height);
              spritebatch.Draw(t, new Rectangle((int) baseModel.Tower.Location.X, (int) baseModel.Tower.Location.Y, width, height), frame,
                  c, 0, new Vector2(width / 2, height / 2), SpriteEffects.None, 0);

              //Guns
              drawGuns(spritebatch);
            }

            //Living
            foreach (Enemy e in baseModel.Living)
            {
                Color col = Color.BlanchedAlmond;
                col.A = 200;
                if (e.Player != -1)
                {
                    col = player_colors[e.Player];
                    col.A = 230;
                }
                
                Rectangle rect = new Rectangle((int)e.Location.X, (int)e.Location.Y, (int)e.Shape.Width, (int)e.Shape.Height);
                if (!(e is Invader))
                {
                    spritebatch.Draw(enemy, rect, new Rectangle(0, 0, enemy.Width, enemy.Height), col, e.Orientation, new Vector2(enemy.Width / 2, enemy.Height / 2), SpriteEffects.None, 1);
                }
                else
                {   
                 
                    spritebatch.Draw(boss, rect, new Rectangle(0, 0, boss.Width, boss.Height), col, e.Orientation, new Vector2(boss.Width / 2, boss.Height / 2), SpriteEffects.None, 1);
                }
            }
            //dying
            foreach (EnemyTimeWho et in baseModel.Dying)
            {
                Enemy e = et.enemy;
                Color col = Color.BlanchedAlmond;
                if (e.Player != -1)
                {
                    col = player_colors[e.Player];
                }
                col.A = 200;
                Rectangle rect = new Rectangle((int)e.Location.X, (int)e.Location.Y, (int)e.Shape.Width, (int)e.Shape.Height);
                spritebatch.Draw(enemy, rect, new Rectangle(0, 0, enemy.Width, enemy.Height), col, e.Orientation, new Vector2(enemy.Width / 2, enemy.Height / 2), SpriteEffects.None, 1);
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
        private void drawGuns(SpriteBatch spritebatch)
        {

            foreach (Model.Gun.Turret t in baseModel.Turrets)
            {
                Color col = player_colors[t.PlayerId];
                col.A = 200;
                Texture2D gun = gun1;

                spritebatch.Draw(gun, new Rectangle((int)t.Location.X, (int)t.Location.Y, (int)t.Shape.Width, (int)t.Shape.Height),null,
                    col, t.Orientation, new Vector2(gun.Width / 2, gun.Height / 2), SpriteEffects.None, 1);

                if (!t.Exists)
                {
                    spritebatch.Draw(middle, new Rectangle((int)t.Location.X, (int)t.Location.Y, (int)t.Shape.Height+10, (int)t.Shape.Height+10),null,
                    col, t.Orientation, new Vector2(middle.Width / 2, middle.Height / 2), SpriteEffects.None, 1);
                }
                
            }
            foreach (Model.Gun.MainGun m in baseModel.Players)
            {
                if (m.IsActive)
                {
                    Texture2D gun = gun1;
                    Color col = player_colors[m.PlayerId];
                    col.A = 200;
                    if (m.Shots.Count() == 2)
                    {
                        gun = gun2;
                    }
                    if (m.Shots.Count() == 3)
                    {
                        gun = gun3;
                    }
                    spritebatch.Draw(gun, new Rectangle((int)m.Location.X, (int)m.Location.Y, (int)(0.5 * gun.Width), (int)(0.5 * gun.Height)), new Rectangle(0, 0, gun.Width, gun.Height),
                      col, m.Orientation, new Vector2(gun.Width / 2, gun.Height / 2), SpriteEffects.None, 1);
                }
            }
        }


        void OnDeath(object sender, EventArgs e)
        {
          particleEngine.addExplosion(App.Instance.Model.Tower.Location, Color.HotPink);
        }
    }
}
