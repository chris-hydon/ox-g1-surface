using System;

using Microsoft.Xna.Framework;
using Microsoft.Surface.Core;

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

    /// <summary>
    /// Actually use a circle - it's not much different to ellipse, easier to work out and more sensitive is
    /// a good thing in this case.
    /// </summary>
    /// <param name="contact">The Contact object to compare.</param>
    /// <returns>Whether or not the Contact is touching this shape.</returns>
    public bool Collides(Contact contact)
    {
      return Radius + (contact.MajorAxis / 2) > (Origin - new Vector2(contact.CenterX, contact.CenterY)).Length();
    }

    public bool CheckCollides(Circle circle)
    {
      return Radius + circle.Radius > (Origin - circle.Origin).Length();
    }

    public bool CheckCollides(Rectangle rect)
    {
      // Find the closest point to the circle in and on the boundary of the rectangle.
      float closestX = Math.Max(rect.Left, Math.Min(rect.Right, Origin.X));
      float closestY = Math.Max(rect.Top, Math.Min(rect.Bottom, Origin.Y));

      // Calculate the distance between the circle's center and this closest point.
      Vector2 distance = new Vector2(Origin.X - closestX, Origin.Y - closestY);

      // If the distance is less than the circle's radius, intersection.
      return distance.Length() < Radius;
    }
  }
}
