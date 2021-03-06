﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SurfaceTower.VideoEngine.ParticleEngine
{ 
    public class Emitter : AbstractEmitter
    {
        private const int MAXPARTICLES = 200;
        private Color color;
        private int size;

        public Emitter(Vector2 position, Texture2D sprite, Color color, int size)
        {
            this.position = position;
            this.sprite = sprite;
            this.color = color;
            this.size = size;
        }

        public override void Update(){
            Particle p;
            for (int i = particles.Count; i < MAXPARTICLES; i++)
            {
                particles.Add(newParticle());
            }
            for (int i = 0; i < particles.Count; i++)
            {
                p = particles.ElementAt<Particle>(i);
                if (p.timeToLive <= 0){
                    particles.Remove(p);
                }
            }
            for (int i = 0; i < particles.Count; i++)
            {
                particles.ElementAt<Particle>(i).Update();
            }
        }

        public override bool IsFinished(){
            return false;
        }

        private Particle newParticle()
        {
            Vector2 v = (float)random.NextDouble()*0.05f*(new Vector2(random.Next(size), random.Next(size)));
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
            int s = random.Next(3)+3;
            int ttl = random.Next(50)+1;
            Particle particle = new Particle(v, p, a, av, s, ttl, sprite, color);
            return particle;
        }


    }
}
