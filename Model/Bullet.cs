using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SurfaceTower.Model
{
    class Bullet
    {
        private int x, y, rotation;

        #region Properties
        public int X
        {
            get { return x; }
        }
        public int Y
        {
            get { return y; }
        }
        public int Rotation
        {
            get { return rotation; }
        }
        #endregion
    }
}
