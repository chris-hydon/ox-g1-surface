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
            App.Instance.Model.DeadEnemy += new EventHandler<SurfaceTower.Model.EventArguments.EnemyArgs>(Model_DeadEnemy);
            foreach (Model.Gun.Turret t in App.Instance.Model.Turrets)
            {
                t.NewBullet += new EventHandler<SurfaceTower.Model.EventArguments.BulletArgs>(t_NewBullet);
            }
        }

        void t_NewBullet(object sender, SurfaceTower.Model.EventArguments.BulletArgs e)
        {
            emitters.Add(new FireBulletEmitter(e.Bullet, tex, 30));
        }

        void Model_DeadEnemy(object sender, SurfaceTower.Model.EventArguments.EnemyArgs e)
        {
            addExplosion(e.Enemy.Location, e.Enemy.Colour);
        }

        void NewEnemy(object sender, SurfaceTower.Model.EventArguments.EnemyArgs e)
        {
            //emitters.Add(new EnemyEmitter(e.Enemy, tex));
        }

        public void addEmitter(Vector2 position, Color color, int size)
        {

            emitters.Add(new Emitter(position, tex, color, size));
        }

        public void addExplosion(Vector2 position, Color color)
        {
            ExplosionEmitter e = new ExplosionEmitter(position, tex, color);
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
