using Microsoft.Xna.Framework;
namespace SurfaceTower.Model.Shape
{
  public class Circle : IShape
  {
    float radius;
    Vector2 origin;

    #region Properties

    public float Radius
    {
      get { return radius; }
    }

    public float Width
    {
      get { return Radius * 2; }
    }

    public float Height
    {
      get { return Radius * 2; }
    }

    public Vector2 Origin
    {
      get { return origin; }
      set { origin = value; }
    }

    #endregion

    public Circle(float radius, Vector2 origin)
    {
      this.radius = radius;
      this.origin = origin;
    }

    public bool Collides(IShape otherShape)
    {
      return otherShape.CheckCollides(this);
    }

    public bool CheckCollides(Circle otherShape)
    {
      return Radius + otherShape.Radius > (Origin - otherShape.Origin).Length();
    }
  }
}
