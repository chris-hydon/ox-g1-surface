using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SurfaceTower.Model
{
  public class Bullet
  {
    public int x, y, rotation, speed, power, effect;

    public Bullet(int x, int y, int rotation, int speed, int power, Turret.Effects effect)
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
      x += (int) Math.Ceiling(speed * Math.Cos(rotation));
      y += (int) Math.Ceiling(speed * Math.Sin(rotation));
    }
  }
}
