using System;

using Microsoft.Xna.Framework;
using SurfaceTower.Model.Shape;
using Microsoft.Xna.Framework.Graphics;

namespace SurfaceTower.Model
{
  public class Enemy : ICollidable, IMovable
  {
    [Flags]
    public enum States
    {
      None = 0,
      Slowed = 1,
      Burning = 2,
      Stunned = 4,
    }
    protected Vector2 velocity;
    protected Vector2 acceleration = Vector2.Zero;
    protected readonly IShape shape;
    protected float orientation;
    protected int health;
    protected States state;
    protected int age = 0;
    protected Color colour;

    #region Properties
    public Color Colour
    {
      get { return colour; }
      set { colour = value; }
    }

    public int Age
    {
      get { return age; }
    }
    public States State
    {
      get { return state; }
      set { state = value; }
    }

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

    #region Events
    public event EventHandler EnemyReached;
    #endregion

    #region Methods

    public Enemy(Vector2 location, float orientation, int size, int health, Vector2 velocity, Color colour)
    {
      this.shape = new Circle(size, location);
      this.orientation = orientation;
      this.health = health;
      this.colour = colour;
      this.velocity = new Vector2(velocity.X, velocity.Y);
    }

    public virtual void Move()
    {
      Velocity += Acceleration / Constants.UPDATES_PER_SECOND;
      Location += ((state & States.Slowed) == 0 ? Velocity / 2 : Velocity) / Constants.UPDATES_PER_SECOND;
    }
    public void Update()
    {
      // damage if burning
      if ((state & States.Burning) != 0)
      {
          health--;
      }

      //Damage the tower and die if in contact
      if (Collides(App.Instance.Model.Tower))
      {
        App.Instance.Model.Tower.Health -= (int) Math.Round(Size);
        if (EnemyReached != null) EnemyReached(this, null);
        App.Instance.Model.DeathRow.Enqueue(new EnemyTimeWho(this, TimeSpan.MinValue, -1));
      }
    }

    public bool Collides(ICollidable c)
    {
      return Shape.Collides(c.Shape);
    }
    #endregion

    #region Statics

    /// <summary>
    /// Determine the closest enemy to the IEntity origin, using the standard 2D distance metric.
    /// </summary>
    /// <param name="from">The IEntity from which to base the search.</param>
    /// <returns>The closest Enemy to origin, or null if no such Enemy exists.</returns>
    public static Enemy FindNearestLiving(IEntity origin)
    {
      double neardist = double.PositiveInfinity;
      Enemy nearest = null;
      foreach (Enemy e in App.Instance.Model.Living)
      {
        //if the enemy is on the screen
        if (App.Instance.onScreen(e.Location))
        {
          double dist = (e.Location - origin.Location).Length();
          if (dist < neardist)
          {
            nearest = e;
            neardist = dist;
          }
        }
      }
      return nearest;
    }

    #endregion
  }
}
