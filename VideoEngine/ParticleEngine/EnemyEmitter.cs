using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SurfaceTower.Model;

namespace SurfaceTower.VideoEngine.ParticleEngine
{
    class EnemyEmitter : AbstractEmitter
    {
        protected const int MAXPARTICLES = 40;
        private Enemy enemy;
        

        public EnemyEmitter(Enemy enemy, Texture2D sprite)
        {
            this.enemy = enemy;
            this.sprite = sprite;
        }

        public override bool IsFinished()
        {

            return (enemy.Health <= 0 && particles.Count == 0);
        }

        public override void Update()
        {
            Particle p;
            if (enemy.Health > 0)
            {
                for (int i = particles.Count; i < MAXPARTICLES; i++)
                {
                    p = newPart();
                    particles.Add(p);
                }
            }
            for (int i = 0; i < particles.Count; i++)
            {
                p = particles.ElementAt<Particle>(i);
                if (p.timeToLive <= 0)
                {
                    particles.Remove(p);
                }
            }
            for (int i = 0; i < particles.Count; i++)
            {
                particles.ElementAt<Particle>(i).Update();
            }
        }

        private Particle newPart()
        {
            Vector2 v = -0.04f * ((float)random.NextDouble())* enemy.Velocity;
            v = Vector2.Transform(v, Matrix.CreateRotationZ((float) (Math.PI*(0.35)*(random.NextDouble() - 0.5))));
            Vector2 p = enemy.Location;
            int s = random.Next(3)+3;
            int ttl = random.Next(15) + 1;
            Particle particle = new Particle(v, p, 0, 0, s, ttl, sprite, Color.HotPink);
            return particle;
        }
        

    }
}
