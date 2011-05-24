using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SurfaceTower.Model;

namespace SurfaceTower.VideoEngine.ParticleEngine
{
    public class Particle
    {
        #region fields
        private Vector2 velocity;
        private Vector2 position;
        public int timeToLive;
        private int originalttl;
        private Texture2D sprite;
        public int size { get; set; }
        public float angle { get; set; }
        public float angVelocity { get; set; }
        private Color color;
        #endregion

        public Particle(Vector2 velocity, Vector2 position, float angle, float angVelocity, int size, int timeToLive, Texture2D sprite, Color color)
        {
            this.velocity = velocity;
            this.position = position;
            this.timeToLive = timeToLive;
            this.size = size;
            this.angle = angle;
            this.angVelocity= angVelocity;
            this.sprite = sprite;
            this.color = color;
            originalttl = timeToLive;
        }
        
        public void Update(){
            /**if (position.Y > App.Instance.GraphicsDevice.Viewport.Height || position.Y < 0)
            {
                this.velocity.Y *= -1;
            }
            if (position.X > App.Instance.GraphicsDevice.Viewport.Width || position.X < 0)
            {
                this.velocity.X *= -1;
            }*/

            if (timeToLive > 0)
            {
                timeToLive--;
            }
            angle += angVelocity;
            position += velocity;
            return;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 origin = new Vector2(sprite.Width/2, sprite.Height/2);
            int a = (255 * timeToLive) / originalttl;
            color.A = (byte)((255 * timeToLive) / originalttl);
            spriteBatch.Draw(sprite, new Rectangle((int)position.X, (int)position.Y, size, size), null, color, 0, new Vector2(sprite.Width/2, sprite.Height/2), SpriteEffects.None, 0);

        }


    }
}
