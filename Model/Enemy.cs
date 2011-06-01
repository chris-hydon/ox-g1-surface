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
    protected IShape shape;
    protected int health;
    protected States state;
    protected int age = 0;
    protected int player = -1;

    #region Properties

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
      get { return (float) Math.Atan2(Velocity.Y, Velocity.X); }
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
      get { return (Shape is Circle) ? ((Circle) Shape).Radius : ((SurfaceTower.Model.Shape.Rectangle) Shape).Width; }
    }

    public int Health
    {
      get { return health; }
      set { health = value; }
    }

    public int Player
    {
      get { return player; }
      set { player = value; }
    }

    #endregion

    #region Events
    public event EventHandler EnemyReached;
    #endregion

    #region Methods

    public Enemy(Vector2 location, int size, int health, Vector2 velocity, int player)
    {
      this.shape = new Circle(size, location);
      this.health = health;
      this.velocity = new Vector2(velocity.X, velocity.Y);
      this.player = player;
    }

    public virtual void Move()
    {
      Velocity += Acceleration / Constants.UPDATES_PER_SECOND;
      Location += ((state & States.Slowed) == 0 ? Velocity / 2 : Velocity) / Constants.UPDATES_PER_SECOND;
    }
    public void Update()
    {
      // Damage the enemy if it is burning
      if ((state & States.Burning) != 0)
      {
          health -= Constants.BURN_DAMAGE;
      }

      //If the enemy is in contact with the tower, damage the tower and mark the enemy for removal.
      if (Collides(App.Instance.Model.Tower))
      {
        if (this is BossEnemy)
        {
          App.Instance.Model.Tower.Health = 0;
        }
        else
        {
          App.Instance.Model.Tower.Health -= (int) Math.Round(Size);
        }
        if (EnemyReached != null) EnemyReached(this, null);
        App.Instance.Model.DeathRow.Enqueue(new EnemyTimeWho(this, TimeSpan.MinValue, -1));

        // Remove all EnemyReached subscribers listening to this enemy.
        EnemyReached = null;
      }
    }

    public bool Collides(ICollidable c)
    {
      //If enemy is player specific, bullets from other players don't collide with it.
      if (Player != -1 && c is Bullet && ((Bullet) c).PlayerId != Player)
      {
        return false;
      }

      return Shape.Collides(c.Shape);
    }
    #endregion

    #region Statics

    /// <summary>
    /// Determine the closest enemy to the IEntity origin, using the standard 2D distance metric.
    /// </summary>
    /// <param name="origin">The IEntity from which to base the search.</param>
    /// <param name="targetRestriction">The playerID of the origin, if applicable.</param>
    /// <returns>The closest living Enemy to origin, or null if no Enemy exists.</returns>
    public static Enemy FindNearestLiving(IEntity origin, int targetRestriction)
    {
      double neardist = double.PositiveInfinity;
      Enemy nearest = null;
      foreach (Enemy e in App.Instance.Model.Living)
      {
        //If the enemy is on the screen, and is either non-player-specific or its target matches the targetRestriction
        if (App.Instance.onScreen(e.Location) && (e.Player == -1 || e.Player == targetRestriction))
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
