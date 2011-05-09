using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SurfaceTower.VideoEngine.ParticleEngine
{ 
    public class Emitter : AbstractEmitter
    {
        const int MAXPARTICLES = 200;

        public Emitter(Vector2 position, Texture2D sprite)
        {
            this.position = position;
            this.sprite = sprite;
            particles = new List<Particle>(MAXPARTICLES);
            numberOfParticles = 0;
        }

        public override void Update(){
            Particle p;
            for (int i = numberOfParticles; i < MAXPARTICLES; i++)
            {
                particles.Add(newParticle());
                numberOfParticles++;
            }
            for (int i = 0; i < numberOfParticles; i++)
            {
                p = particles.ElementAt<Particle>(i);
                if (p.timeToLive <= 0){
                    particles.Remove(p);
                    numberOfParticles--;
                }
            }
            for (int i = 0; i < numberOfParticles; i++)
            {
                particles.ElementAt<Particle>(i).Update();
            }
        }

        public override bool IsFinished(){
            return false;
        }

        private Particle newParticle()
        {
            Vector2 v = (float)random.NextDouble()*0.05f*(new Vector2(random.Next(100), random.Next(100)));
            if (random.Next(3) > 1)
            {
                v.X *= -1;
            }
            if (random.Next(3) > 1)
            {
                v.Y *= -1;
            }
            Vector2 p = position;
            float a = (float)random.NextDouble();
            float av = (float)random.NextDouble();
            int s = random.Next(5);
            int ttl = random.Next(50)+1;
            Particle particle = new Particle(v, p, a, av, s, ttl, sprite, Color.Green);
            return particle;
        }


    }
}
