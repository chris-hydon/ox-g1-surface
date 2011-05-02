using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SurfaceTower.Model
{
  public class Bullet
  {
    protected int speed, power;
    protected float x, y, rotation;
    protected Turret.Effects effect;

  #region Properties
    public int X
    {
      get { return (int) x; }
    }
    public int Y
    {
      get { return (int) y; }
    }
    public float Rotation
    {
      get { return rotation; }
    }
    public int Speed
    {
      get { return speed; }
    }
    public int Power
    {
      get { return power; }
    }
    public Turret.Effects Effect
    {
      get { return effect; }
    }
    
  #endregion

    public Bullet(int x, int y, float rotation, int speed, int power, Turret.Effects effect)
    {
      this.x = x;
      this.y = y;
      this.rotation = rotation;
      this.speed = speed;
      this.power = power;
      this.effect = effect;
    }

    public void Move()
    {
      x += (float) (speed * Math.Cos(rotation)/Constants.SPEED_CONSTANT);
      y += (float) (speed * Math.Sin(rotation)/Constants.SPEED_CONSTANT);
    }
  }
}
