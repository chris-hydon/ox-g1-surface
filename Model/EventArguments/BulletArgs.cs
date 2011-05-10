using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SurfaceTower.Model.EventArguments
{
  public class BulletArgs : EventArgs
  {
    private Bullet b;
    public BulletArgs(Bullet b)
    {
      this.b = b;
    }
    public Bullet Bullet
    {
      get { return b; }
    }
  }
}
