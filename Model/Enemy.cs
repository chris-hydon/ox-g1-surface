using Microsoft.Xna.Framework;

namespace SurfaceTower.Model
{
  public class Enemy
  {
    protected GameTime dying;
    protected GameTime dead;
    protected int x, y;

    #region Properties

    public int X
    {
        get { return x; }
    }
    public int Y
    {
        get { return y; }
    }
    public GameTime Dying
    {
      get { return dying; }
    }

    public GameTime Dead
    {
      get { return dead; }
    }

    #endregion

    #region Dying Enemies

    public bool IsDying()
    {
      return (this.Dying == null);
    }

    public bool IsDead()
    {
      return (this.Dead == null);
    }

    public void Kill()
    {
        Stage.INSTANCE.kill(this);
    }

    public void Cremate()
    {
        Stage.INSTANCE.cremate(this);
    }

    #endregion

      #region Methods
      public void moveTo(Vector2 destination){
            x = (int)destination.X;
            y = (int)destination.Y;
        }
      #endregion
  }
}
