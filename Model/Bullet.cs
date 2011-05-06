using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SurfaceTower.Model.Shape;
using Microsoft.Xna.Framework;

namespace SurfaceTower.Model
{
  public class Bullet : ICollidable, IMovable
  {
    protected Vector2 velocity;
    protected Vector2 acceleration = Vector2.Zero;
    protected readonly IShape shape;
    protected int power;
    protected Turret.Effects effect;

    #region Properties

    public IShape Shape
    {
      get { return shape; }
    }

    public float Orientation
    {
      get { return (float) Math.Atan(Velocity.Y / Velocity.X); }
      set
      {
        // The orientation of a bullet is always the direction of travel and cannot be set directly.
        throw new InvalidOperationException();
      }
    }

    public Vector2 Location
    {
      get { return shape.Origin; }
      set { shape.Origin = value; }
    }

    public Vector2 Velocity
    {
      get { return velocity; }
      set { velocity = value; }
    }

    public Vector2 Acceleration
    {
      get { return acceleration; }
      set { acceleration = value; }
    }

    public int Power
    {
      get { return power; }
    }

    public Turret.Effects Effect
    {
      get { return effect; }
    }
    
    #endregion

    public Bullet(Vector2 location, Vector2 velocity, int power, Turret.Effects effect)
    {
      this.shape = new Circle(3, location);
      this.velocity = velocity;
      this.power = power;
      this.effect = effect;
    }

    public void Move()
    {
      Velocity += Acceleration / Constants.UPDATES_PER_SECOND;
      Location += Velocity / Constants.UPDATES_PER_SECOND;
    }

    public bool Collides(ICollidable c)
    {
      return Shape.Collides(c.Shape);
    }
  }
}
