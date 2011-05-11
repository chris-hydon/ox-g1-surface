using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SurfaceTower.Model;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SurfaceTower.VideoEngine.ParticleEngine
{
    class FireBulletEmitter : AbstractEmitter
    {
        private Bullet bullet;
        private const int MAXPARTICLES = 100;
        private Texture2D tex;
        private int size;

        public FireBulletEmitter(Bullet bullet, Texture2D tex, int size)
        {
            this.bullet = bullet;
            this.tex = tex;
            this.size = size;
        }

        public override void Update()
        {
            Particle p;
            for (int i = particles.Count; i < MAXPARTICLES; i++)
            {
                particles.Add(newParticle());
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
        

        public override bool IsFinished()
        {
            return (bullet.Age <= 0 && particles.Count == 0);
        }
        private Particle newParticle()
        {
            Vector2 v = (float)random.NextDouble() * 0.05f * (new Vector2(random.Next(size), random.Next(size)));
            if (random.Next(3) > 1)
            {
                v.X *= -1;
            }
            if (random.Next(3) > 1)
            {
                v.Y *= -1;
            }
            Vector2 p = bullet.Location;
            int s = random.Next(3) + 3;
            int ttl = random.Next(50) + 1;
            Particle particle = new Particle(v, p, 0, 0, s, ttl, tex, Color.Red);
            return particle;
        }


    }
}
