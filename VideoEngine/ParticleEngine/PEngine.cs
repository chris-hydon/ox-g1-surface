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
        private ICollection<AbstractEmitter> emitters;
        private SimpleView view;
        private Texture2D tex;

        public PEngine(SimpleView view)
        {
            this.view = view;
            emitters = new List<AbstractEmitter>(MAXEMITTERS);
            tex = view.content.Load<Texture2D>("particle");
            App.Instance.Model.NewEnemy += new EventHandler<SurfaceTower.Model.EventArguments.EnemyArgs>(NewEnemy);
        }

        void NewEnemy(object sender, SurfaceTower.Model.EventArguments.EnemyArgs e)
        {
            //emitters.Add(new EnemyEmitter(e.Enemy, tex));
        }

        public void addEmitter(Vector2 position)
        {

            emitters.Add(new Emitter(position, tex));
        }

        public void addExplosion(Vector2 position)
        {
            ExplosionEmitter e = new ExplosionEmitter(position, tex);
            e.Update();
            emitters.Add(e);
        }

        public void Update(ICollection<Enemy> living)
        {
            AbstractEmitter em;
            for (int i=0; i < emitters.Count; i++)
            {
                em = emitters.ElementAt<AbstractEmitter>(i);
                if (em.IsFinished())
                {
                    emitters.Remove(em);
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
