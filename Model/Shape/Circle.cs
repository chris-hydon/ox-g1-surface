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

    public bool CheckCollides(Circle otherShape)
    {
      return Radius + otherShape.Radius > (Origin - otherShape.Origin).Length();
    }
  }
}
