using Microsoft.Xna.Framework;

namespace SurfaceTower.Model
{
  public class Turret
  {
    private float orientation;
    private Vector2 position;

    #region Properties

    public float Orientation
    {
      get { return orientation; }
    }

    public Vector2 Position
    {
      get { return position; }
    }

    #endregion
  }
}
