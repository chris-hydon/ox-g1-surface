using System;

using SurfaceTower.Model.Gun;

namespace SurfaceTower.Model.EventArguments
{
  public class TurretArgs : EventArgs
  {
    private Turret t;
    public TurretArgs(Turret t)
    {
      this.t = t;
    }
    public Turret Turret
    {
      get { return t; }
    }
  }
}
