using Microsoft.Xna.Framework;

namespace SurfaceTower.Model
{
  public interface IMovable : IEntity
  {
    new float Orientation { get; }
    new Vector2 Location { get; set; }
    Vector2 Velocity { get; set; }
    Vector2 Acceleration { get; set; }

    void Move();
  }
}
