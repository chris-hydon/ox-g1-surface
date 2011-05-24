using System;

using SurfaceTower.Model.Shape;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SurfaceTower.Model
{
  public class Tower : IEntity, ICollidable
  {
    private readonly Circle shape;
    private int health;
    private int maxHealth = Constants.TOWER_DEFAULT_HEALTH;

    #region Properties

    public Vector2 Location
    {
      get { return shape.Origin; }
    }

    public float Orientation
    {
      get { return 0; }
    }

    public IShape Shape
    {
      get { return shape; }
    }

    public int Health
    {
      get { return health; }
      set
      {
        health = Math.Min(value, MaxHealth);
        //If health has reached zero, fire an event to notify that the tower has ZeroHealth.
        if (health <= 0 && ZeroHealth != null)
        {
          ZeroHealth(this, null);
        }
      }
    }

    public int MaxHealth
    {
      get { return maxHealth; }
      set
      {
        // Increasing maxHealth - add on the difference.
        if (maxHealth < value)
        {
          health += value - maxHealth;
        }
        // Reducing maxHealth - remove any excess.
        else if (health > value)
        {
          health = value;
        }
        maxHealth = value;
      }
    }

    #endregion

    #region Events

    public event EventHandler ZeroHealth;

    #endregion

    /// <summary>
    /// The Tower is the central target for the enemies - if its health reaches zero the game is over.
    /// </summary>
    public Tower()
    {
      Viewport v = App.Instance.GraphicsDevice.Viewport;
      shape = new Circle(Constants.MAIN_TURRET_RADIUS, new Vector2(v.Width / 2, v.Height / 2));
      health = maxHealth;
    }

    public void OnBeat(object sender, EventArgs e)
    {
      //The tower has a healing effect based on the TOWER_REGENERATION constant.
      Health += Constants.TOWER_REGENERATION;
    }

    public bool Collides(ICollidable c)
    {
      return Shape.Collides(c.Shape);
    }
  }
}
