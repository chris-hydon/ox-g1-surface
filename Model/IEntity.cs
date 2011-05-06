using Microsoft.Xna.Framework;
using SurfaceTower.Model.Shape;

namespace SurfaceTower.Model
{
  public interface IEntity
  {
    Vector2 Location { get; }
    IShape Shape { get; }
    float Orientation { get; }
  }
}
