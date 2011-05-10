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
    Homing = 16,
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
      if (((Effects.Homing & effects) != 0) && App.Instance.Model.Living.Count > 0)
      {
        double neardist = double.PositiveInfinity;
        Enemy nearest = new Enemy(Location, Orientation, 0, 0, Velocity);
        foreach (Enemy e in App.Instance.Model.Living)
        {
          double dist = Math.Sqrt((e.Location.X - Location.X) * (e.Location.X - Location.X) + (e.Location.Y - Location.Y) * (e.Location.Y - Location.Y));
          if (dist < neardist)
          {
            nearest = e;
          }
        }
        Vector2 target = nearest.Location - Location;
        Velocity += (1000*target/(target.Length()*target.Length()));
        float speed = Velocity.Length();
        if (target.Length() < 60)
        {
          target.Normalize();
          Velocity = speed*target;
        }
      }
      else
      {
        Velocity += Acceleration / Constants.UPDATES_PER_SECOND;
      }
      Location += Velocity / Constants.UPDATES_PER_SECOND;
    }

    public bool Collides(ICollidable c)
    {
      return Shape.Collides(c.Shape);
    }
  }
}
