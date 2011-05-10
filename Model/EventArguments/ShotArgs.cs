using System;

using SurfaceTower.Model.Gun;

namespace SurfaceTower.Model.EventArguments
{
  public class ShotArgs : EventArgs
  {
    private ShotPatterns s;
    public ShotArgs(ShotPatterns s)
    {
      this.s = s;
    }
    public ShotPatterns Shots
    {
      get { return s; }
    }
  }
}
