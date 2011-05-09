using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SurfaceTower.Model;

namespace SurfaceTower.VideoEngine.ParticleEngine
{
    public class PEngine
    {
        const int MAXEMITTERS = 50;
        private int numberOfEmitters;
        private ICollection<AbstractEmitter> emitters;
        private SimpleView view;
        private Texture2D tex;

        public PEngine(SimpleView view)
        {
            this.view = view;
            emitters = new List<AbstractEmitter>(MAXEMITTERS);
            tex = view.content.Load<Texture2D>("particle");
            numberOfEmitters = 0;
        }

        public void addEmitter(Vector2 position)
        {

            emitters.Add(new Emitter(position, tex));
            numberOfEmitters++;
        }

        public void addExplosion(Vector2 position)
        {
            ExplosionEmitter e = new ExplosionEmitter(position, tex);
            e.Update();
            emitters.Add(e);
            numberOfEmitters++;
        }

        public void Update(ICollection<Enemy> living)
        {
            AbstractEmitter em;
            for (int i=0; i < numberOfEmitters; i++)
            {
                em = emitters.ElementAt<AbstractEmitter>(i);
                if (em.IsFinished())
                {
                    emitters.Remove(em);
                    numberOfEmitters--;
                }
            }
            foreach (AbstractEmitter e in emitters)
            {
                e.Update();
            }
        }

        public void Draw()
        {
            foreach (AbstractEmitter e in emitters)
            {
                e.Draw(view.spritebatch);
            }

        }





    }
}
