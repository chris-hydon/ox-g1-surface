using System;
using Microsoft.Xna.Framework;


namespace SurfaceTower.Model
{
  public class Turret
  {
    [Flags]
    public enum Effects {None = 0}
    
    protected Vector2 position;
    protected float orientation = 0;
    protected int owner;
    protected Effects effect = Effects.None;
    protected int strength = 1;

    #region Properties

    public float Orientation
    {
      get { return orientation; }
      set { orientation = value; }
    }

    public Vector2 Position
    {
      get { return position; }
    }
    public Effects Effect
    {
      get { return effect; }
      set { effect = value; }
    }
    public int Strength
    {
      get { return strength; }
      set { strength = value; }
    }
    public int Owner
    {
      get { return owner; }
    }

    #endregion
    #region Methods
    public Turret(Vector2 position, int owner)
    {
      this.position = position;
      this.owner = owner;
    }
    public void Shoot()
    {
      Bullet bullet = new Bullet((int)position.X, (int)position.Y, orientation, 5, strength, effect);
      App.Instance.Model.Bullets.Add(bullet);
    }
    #endregion
  }
}
