using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SurfaceTower.VideoEngine.ParticleEngine
{
    public class ExplosionEmitter : AbstractEmitter
    {
        private const int EXPLOSION_SIZE = 3000;
        private const int EXPLOSION_LENGTH = 50;
        private const int EXPLOSION_SPEED = 8;
        public ExplosionEmitter(Vector2 position, Texture2D sprite)
        {
            this.position = position;
            this.sprite = sprite;
            this.particles = new List<Particle>(EXPLOSION_SIZE);
            Vector2 v;
            for (int i = 0; i < EXPLOSION_SIZE; i++)
            {
                v = new Vector2(random.Next(1000), random.Next(1000));
                v.Normalize();
                v *= (float)(random.Next(EXPLOSION_SPEED)*random.NextDouble());
                if (random.Next(3) > 1)
                {
                    v.X *= -1;
                }
                if (random.Next(3) > 1)
                {
                    v.Y *= -1;
                }
                Particle p = new Particle(v, position, 0, 0, 10, EXPLOSION_LENGTH, sprite, Color.Red);
                particles.Add(p);
                numberOfParticles++;
            }

        }

        public override void Update()
        {
            Particle p;
            for (int i = 0; i < numberOfParticles; i++)
            {
                p = particles.ElementAt<Particle>(i);
                if (p.timeToLive == 0)
                {
                    particles.Remove(p);
                    numberOfParticles--;
                }
            }
            for (int i = 0; i < numberOfParticles; i++)
            {
                particles.ElementAt<Particle>(i).Update();
            }
        }

        public override bool IsFinished()
        {
            return false;
        }


        

    }
}
