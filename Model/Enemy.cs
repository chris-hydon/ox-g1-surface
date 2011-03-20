using Microsoft.Xna.Framework;

namespace SurfaceTower.Model
{
  public class Enemy
  {
    protected GameTime dying;
    protected GameTime dead;

    #region Properties

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
      // TODO: Method stub.
    }

    public void Cremate()
    {
      // TODO: Method stub.
    }

    #endregion
  }
}
