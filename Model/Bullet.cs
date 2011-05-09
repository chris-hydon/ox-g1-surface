using System;

using Microsoft.Xna.Framework;
using SurfaceTower.Model.Gun;
using SurfaceTower.Model.Shape;

namespace SurfaceTower.Model
{
  [Flags]
  public enum Effects
  {
    None = 0,
    Pierce = 1,
    Burn = 2,
    Stun = 4,
    Slow = 8,
  }

  public class Bullet : ICollidable, IMovable
  {
    protected Vector2 velocity;
    protected Vector2 acceleration = Vector2.Zero;
    protected readonly IShape shape;
    protected int power;
    protected int playerId;
    protected Effects effects;

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

    public int PlayerId
    {
      get { return playerId; }
    }

    public Effects Effects
    {
      get { return effects; }
    }
    
    #endregion

    public Bullet(Vector2 location, Vector2 velocity, int power, Effects effects, int playerId)
    {
      this.shape = new Circle(3, location);
      this.velocity = velocity;
      this.power = power;
      this.effects = effects;
      this.playerId = playerId;
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
