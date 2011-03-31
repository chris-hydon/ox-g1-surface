using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SurfaceTower.Model
{
    class Stage
    {
        public static Stage INSTANCE = new Stage();
        private ICollection<Enemy> living, dying, dead = new LinkedList<Enemy>();
        private ICollection<Bullet> bullets = new LinkedList<Bullet>();
        private ICollection<Collision> collisions = new LinkedList<Collision>();
    
        #region Properties
        public ICollection<Enemy> Living
        {
            get { return living; }
        }
        public ICollection<Enemy> Dying
        {
            get { return dying; }
        }
        public ICollection<Enemy> Dead
        {
            get { return dead; }
        }
        public ICollection<Bullet> Bullets
        {
            get { return bullets; }
        }
        public ICollection<Collision> Collisions
        {
            get {return collisions; }
        }
        #endregion

        #region Methods
        public void kill(Enemy enemy)
        {
            dying.Remove(enemy);
            dead.Add(enemy);
        }
        public void cremate(Enemy enemy)
        {
            dead.Remove(enemy);
        }
        #endregion
    }
}
