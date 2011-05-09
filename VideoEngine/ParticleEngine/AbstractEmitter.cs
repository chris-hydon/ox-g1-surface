using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SurfaceTower.VideoEngine.ParticleEngine
{
    public abstract class AbstractEmitter
    {
        const int MAXPARTICLES = 200;
        protected int numberOfParticles;
        protected Vector2 position;
        protected ICollection<Particle> particles;
        protected Texture2D sprite;
        protected Random random;

        public AbstractEmitter()
        {
            random = new Random();
        }

        public void Draw(SpriteBatch spriteBatch){
            foreach (Particle p in particles)
            {
                p.Draw(spriteBatch);
            }
        }
        public abstract void Update();
        public abstract bool IsFinished();
    }
}
