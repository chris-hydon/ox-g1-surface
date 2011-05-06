using System;

using Microsoft.Xna.Framework;
using SurfaceTower.Model.Shape;


namespace SurfaceTower.Model
{
  public class Turret : IEntity
  {
    [Flags]
    public enum Effects {None = 0}
    
    protected Circle shape;
    protected float orientation = 0;
    protected int owner;
    protected Effects effect = Effects.None;
    protected int strength = 1;

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
      get { return shape.Origin; }
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

    public Turret(Vector2 location, int owner)
    {
      this.shape = new Circle(Constants.TURRET_RADIUS, location);
      this.owner = owner;
    }
    
    public void Shoot()
    {
      Bullet bullet = new Bullet(Location, new Vector2((float) Math.Cos(orientation), (float) Math.Sin(orientation)), strength, effect);
      App.Instance.Model.Bullets.Add(bullet);
    }
    
    #endregion
  }
}
