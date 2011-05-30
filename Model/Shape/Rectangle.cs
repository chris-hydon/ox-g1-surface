using System;

using Microsoft.Xna.Framework;
using Microsoft.Surface.Core;

namespace SurfaceTower.Model.Shape
{
  public class Rectangle : IShape
  {
    private Vector2 corner1;
    private Vector2 corner2;

    #region Properties

    public Vector2 Origin
    {
      get { return (corner1 + corner2) / 2; }
      set
      {
        Vector2 mod = value - Origin;
        corner1 += mod;
        corner2 += mod;
      }
    }

    public float Width
    {
      get { return Math.Abs(corner1.X - corner2.X); }
    }

    public float Height
    {
      get { return Math.Abs(corner1.Y - corner2.Y); }
    }

    public float Left
    {
      get { return Math.Min(corner1.X, corner2.X); }
    }

    public float Right
    {
      get { return Math.Max(corner1.X, corner2.X); }
    }

    public float Top
    {
      get { return Math.Min(corner1.Y, corner2.Y); }
    }

    public float Bottom
    {
      get { return Math.Max(corner1.Y, corner2.Y); }
    }

    #endregion

    public Rectangle(Vector2 corner1, Vector2 corner2)
    {
      this.corner1 = corner1;
      this.corner2 = corner2;
    }

    public bool Collides(IShape otherShape)
    {
      return otherShape.CheckCollides(this);
    }

    public bool Collides(Contact contact)
    {
      // Assume a circle for ease of calculation.
      return new Circle(contact.MajorAxis, new Vector2(contact.CenterX, contact.CenterY)).CheckCollides(this);
    }

    public bool CheckCollides(Circle circle)
    {
      // Code is located in the Circle class - don't repeat it here.
      return circle.CheckCollides(this);
    }

    public bool CheckCollides(Rectangle rect)
    {
      return (Left <= rect.Right && rect.Left <= Right && Top <= rect.Bottom && rect.Top <= Bottom);
    }
  }
}
