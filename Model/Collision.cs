using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SurfaceTower.Model
{
    class Collision{
        public enum CollisionType {BULLET_ENEMY, ENEMY_TOWER};
        private int x, y;
        private CollisionType type;

        #region Properties
        public int X
        {
            get { return x; }
        }
        public int Y
        {
            get { return y; }
        }
        public CollisionType Type
        {
            get { return type; }
        }
        #endregion

    }
}
