using Microsoft.Surface.Core;
using Microsoft.Xna.Framework;

namespace SurfaceTower.Model.Shape
{
  public interface IShape
  {
    Vector2 Origin { get; set; }
    float Width { get; }
    float Height { get; }
    bool Collides(IShape otherShape);
    bool Collides(Contact contact);
    bool CheckCollides(Circle otherShape);
  }
}
