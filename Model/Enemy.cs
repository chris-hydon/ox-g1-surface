using Microsoft.Xna.Framework;

namespace SurfaceTower.Model
{
  public class Enemy
  {
    //An enemy with a speed of 1 will take MOVES moves to reach the tower.
    protected int MOVES = 100;
    protected int x, y, size, health, speed;

    #region Properties

    public int X
    {
      get { return x; }
      set { x = value; }
    }
    public int Y
    {
      get { return y; }
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
    public void Move(){
      //int XDist = TOWER.X - x;
      //int YDist = TOWER.Y - y;
      //x += XDist * (speed / MOVES);
      //y += YDist * (speed / MOVES);
    }
    #endregion
  }
}
