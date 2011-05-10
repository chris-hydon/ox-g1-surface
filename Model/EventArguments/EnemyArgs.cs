using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SurfaceTower.Model.EventArguments
{
  public class EnemyArgs : EventArgs
  {
    private Enemy e;
    public EnemyArgs(Enemy e)
    {
      this.e = e;
    }
    public Enemy Enemy
    {
      get { return e; }
    }
  }
}
