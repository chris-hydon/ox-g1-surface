using System;

using Microsoft.Xna.Framework;
using SurfaceTower.Model.Gun;
using SurfaceTower.Model.Shape;

namespace SurfaceTower.Model
{
  //Effects uses the Flags style to make it easy to have more than one effect active, and to add and remove from the effects.
  //They are powers of 2 to enable effects to be added, removed and checked using bitwise AND and OR operations.
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

    #region Methods

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
      if (age > Constants.BULLET_LIFE * Constants.UPDATES_PER_SECOND)
      {
        //The bullet has lived longer than the maximum bullet life - mark it for removal.
        App.Instance.Model.UsedBullets.Enqueue(this);
      }
      else
      {
        // Homing bullets can control their acceleration.
        if ((Effects.Homing & effects) != 0)
        {
          Acceleration = HomingAcceleration();
        }

        // Update movement.
        Velocity += Acceleration / Constants.UPDATES_PER_SECOND;
        Location += Velocity / Constants.UPDATES_PER_SECOND;
        age++;
      }
    }

    public bool Collides(ICollidable c)
    {
      return Shape.Collides(c.Shape);
    }

    private Vector2 HomingAcceleration()
    {
      // Find the nearest enemy.
      Enemy focus = Enemy.FindNearestLiving(this, PlayerId);

      // If there's no target, there's no homing acceleration
      if (focus == null)
      {
        return Vector2.Zero;
      }
      //Calculate the relative location.
      Vector2 target = focus.Location - Location;
      // Calculate the relative orientation.
      double orientMod = Math.Atan2(target.Y, target.X) - Orientation;

      // Apply an acceleration perpendicular to the velocity to rotate the bullet.
      return Constants.BULLET_HOMING_TURNSPEED * Vector2.Transform(Velocity,
        Matrix.CreateRotationZ((float) ((orientMod < 0 || orientMod > Math.PI) ? -Math.PI / 2 : Math.PI / 2)));
    }

    #endregion
  }
}
