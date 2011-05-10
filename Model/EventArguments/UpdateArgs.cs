using System;
using Microsoft.Xna.Framework;

namespace SurfaceTower.Model.EventArguments
{
  public class UpdateArgs : EventArgs
  {
    private TimeSpan t;
    public UpdateArgs(TimeSpan t)
    {
      this.t = t;
    }
    public TimeSpan Time
    {
      get { return t; }
    }
  }
}
