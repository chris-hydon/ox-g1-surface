using Microsoft.Xna.Framework;
using System;

namespace SurfaceTower.Model
{
  public class Enemy
  {
    protected float x, y;
    protected int size, health, speed;

    #region Properties

    public int X
    {
      get { return (int) x; }
      set { x = value; }
    }
    public int Y
    {
      get { return (int) y; }
      set { y = value; }
    }
    public int Size
    {
      get { return size; }
      set { size = value; }
    }
    public int Health
    {
      get { return health; }
      set { health = value; }
    }
    public int Speed
    {
      get { return speed; }
      set { speed = value; }
    }

    #endregion

    #region Methods

    public Enemy(int x, int y, int size, int health, int speed)
    {
      this.x = x;
      this.y = y;
      this.size = size;
      this.health = health;
      this.speed = speed;
    }
    public virtual void Move()
    {
      Vector2 pos = MainTurret.TowerPos();
      float XDist = pos.X - x;
      float YDist = pos.Y - y;
      if (XDist != 0 || YDist != 0)
      {
        Vector2 direction = new Vector2(pos.X - x, pos.Y - y);
        if (direction.Length() < speed)
        {
          x = pos.X;
          y = pos.Y;
        }
        else
        {
          direction.Normalize();
          x += direction.X * speed/Constants.SPEED_CONSTANT;
          y += direction.Y * speed/Constants.SPEED_CONSTANT;
        }
      }
    }

    public virtual bool IsHit(Bullet b)
    {
      return (Math.Abs(b.X - X) <= Size && Math.Abs(b.Y - Y) <= Size);
    }
    #endregion
  }
}
