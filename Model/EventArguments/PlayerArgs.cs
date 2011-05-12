using System;

namespace SurfaceTower.Model.EventArguments
{
  public class PlayerArgs : EventArgs
  {
    private int p;
    public PlayerArgs(int p)
    {
      this.p = p;
    }
    public int PlayerID
    {
      get { return p; }
    }
  }
}
