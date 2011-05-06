using System;

using Microsoft.Xna.Framework;
using SurfaceTower.Model.Shape;

namespace SurfaceTower.Model
{
  public class Enemy : ICollidable, IMovable
  {
    protected Vector2 velocity;
    protected Vector2 acceleration = Vector2.Zero;
    protected readonly IShape shape;
    protected float orientation;
    protected int health;

    #region Properties

    public IShape Shape
    {
      get { return shape; }
    }

    public float Orientation
    {
      get { return orientation; }
      set { orientation = value; }
    }

    public Vector2 Location
    {
      get { return Shape.Origin; }
      set { Shape.Origin = value; }
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

    public float Size
    {
      get { return ((Circle) Shape).Radius; }
    }

    public int Health
    {
      get { return health; }
      set { health = value; }
    }

    #endregion

    #region Methods

    public Enemy(Vector2 location, float orientation, int size, int health, Vector2 velocity)
    {
      this.shape = new Circle(size, location);
      this.orientation = orientation;
      this.health = health;
      this.velocity = new Vector2(velocity.X, velocity.Y);
    }

    public virtual void Move()
    {
      Velocity += Acceleration / Constants.UPDATES_PER_SECOND;
      Location += Velocity / Constants.UPDATES_PER_SECOND;
    }

    public bool Collides(ICollidable c)
    {
      return Shape.Collides(c.Shape);
    }
    #endregion
  }
}
