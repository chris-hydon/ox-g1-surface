using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SurfaceTower.VideoEngine.ParticleEngine
{ 
    public class Emitter
    {
        const int MAXPARTICLES = 200;
        private int numberOfParticles;
        private Vector2 position;
        private ICollection<Particle> particles;
        private Texture2D sprite;
        private Random random;

        public Emitter(Vector2 position, Texture2D sprite)
        {
            this.position = position;
            this.sprite = sprite;
            particles = new List<Particle>(MAXPARTICLES);
            numberOfParticles = 0;
            this.random = new Random();
        }

        public void Update(){
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
            int ttl = random.Next(150)+1;
            Particle particle = new Particle(v, p, a, av, s, ttl, sprite);
            return particle;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Particle p in particles)
            {
                p.Draw(spriteBatch);
            }
        }

    }
}
