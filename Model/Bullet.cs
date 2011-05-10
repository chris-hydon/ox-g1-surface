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
    protected int age;
    protected Enemy focus;

    #region Properties
    public int Age
    {
      get { return age; }
    }

    public IShape Shape
    {
      get { return shape; }
    }

    public float Orientation
    {
      get { return (float) Math.Atan2(Velocity.Y, Velocity.X); }
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

    protected Enemy FindNearest()
    {
      double neardist = double.PositiveInfinity;
      Enemy nearest = new Enemy(Location, Orientation, 0, 0, Velocity);
      foreach (Enemy e in App.Instance.Model.Living)
      {
        double dist = (e.Location - Location).Length();
        if (dist < neardist)
        {
          nearest = e;
          neardist = dist;
        }
      }
      return nearest;
    }

    public void Move()
    {
      if (age > Constants.BULLET_LIFE)
      {
        App.Instance.Model.UsedBullets.Enqueue(this);
      }
      else
      {
        // Homing bullets can control their acceleration.
        if ((Effects.Homing & effects) != 0)
        {
          if (App.Instance.Model.Living.Count == 0)
          {
            Acceleration = Vector2.Zero;
          }
          else
          {
            // Find the nearest enemy.
            //if (!App.Instance.Model.Living.Contains(focus))
            //{
              focus = FindNearest();
            //}

            // Get the difference between the bullet's location and that of the target.
            Vector2 target = focus.Location - Location;

            // Figure out the relative orientation.
            float orient = Orientation;
            double orientMod = Math.Atan2(target.Y, target.X) - orient;

            // Apply an acceleration perpendicular to the velocity to rotate the bullet.
            Acceleration = Vector2.Transform(Velocity, Matrix.CreateRotationZ((float) ((orientMod < 0 || orientMod > Math.PI) ? -Math.PI / 2 : Math.PI / 2)));
          }

          // Update movement.
          Velocity += Acceleration / Constants.UPDATES_PER_SECOND;
          Location += Velocity / Constants.UPDATES_PER_SECOND;
          age++;
        }
      }
    }

    public bool Collides(ICollidable c)
    {
      return Shape.Collides(c.Shape);
    }
  }
}
