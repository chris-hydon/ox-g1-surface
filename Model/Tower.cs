using Microsoft.Xna.Framework;
using SurfaceTower.Model.Shape;
using Microsoft.Xna.Framework.Graphics;

namespace SurfaceTower.Model
{
  public class Tower : IEntity, ICollidable
  {
    private readonly Circle shape;
    private int health;

    #region Events

    #endregion

    #region Properties

    public Vector2 Location
    {
      get { return shape.Origin; }
    }

    public float Orientation
    {
      get { return 0; }
    }

    public IShape Shape
    {
      get { return shape; }
    }

    public int Health
    {
      get { return health; }
      set { health = value; }
    }

    #endregion

    public Tower()
    {
      Viewport v = App.Instance.GraphicsDevice.Viewport;
      shape = new Circle(Constants.MAIN_TURRET_RADIUS, new Vector2(v.Width / 2, v.Height / 2));
    }

    public bool Collides(ICollidable c)
    {
      return Shape.Collides(c.Shape);
    }
  }
}
