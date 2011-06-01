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
        private Color[] player_cols = new Color[5] { Color.BlanchedAlmond, Color.Red, Color.ForestGreen, Color.Teal, Color.Gold };
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
            App.Instance.Model.AddTurret += new EventHandler<SurfaceTower.Model.EventArguments.TurretArgs>(Model_AddTurret);
            foreach (Model.Gun.Turret t in App.Instance.Model.Turrets)
            {
                t.NewBullet += new EventHandler<SurfaceTower.Model.EventArguments.BulletArgs>(t_NewBullet);
            }
        }

        public void Reset()
        {
            App.Instance.Model.NewEnemy += new EventHandler<SurfaceTower.Model.EventArguments.EnemyArgs>(NewEnemy);
            App.Instance.Model.DeadEnemy += new EventHandler<SurfaceTower.Model.EventArguments.EnemyArgs>(Model_DeadEnemy);
            App.Instance.Model.AddTurret += new EventHandler<SurfaceTower.Model.EventArguments.TurretArgs>(Model_AddTurret);  
        }

        void Model_AddTurret(object sender, SurfaceTower.Model.EventArguments.TurretArgs e)
        {
            e.Turret.NewBullet+=new EventHandler<SurfaceTower.Model.EventArguments.BulletArgs>(t_NewBullet);
        }



        void t_NewBullet(object sender, SurfaceTower.Model.EventArguments.BulletArgs e)
        {
          //  emitters.Add(new FireBulletEmitter(e.Bullet, tex, 30));
        }

        void Model_DeadEnemy(object sender, SurfaceTower.Model.EventArguments.EnemyArgs e)
        {
            addExplosion(e.Enemy.Location, player_cols[e.Enemy.Player+1]);
        }

        void NewEnemy(object sender, SurfaceTower.Model.EventArguments.EnemyArgs e)
        {
            e.Enemy.EnemyReached += new EventHandler(Enemy_EnemyReached);
        }

        void Enemy_EnemyReached(object sender, EventArgs e)
        {
            Enemy enemy = (Enemy)sender;
            addExplosion(enemy.Location, player_cols[enemy.Player + 1]);
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

        public void Update()
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
