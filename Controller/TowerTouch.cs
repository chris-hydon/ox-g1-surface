using SurfaceTower.VideoEngine.Touchable;
using Microsoft.Surface.Core;

namespace SurfaceTower.Controller
{
  public class TowerTouch : ITouchHandler
  {
    public void Touch(int playerId)
    {
      // Upgrade ready?
      if (App.Instance.Model.Players[playerId].CanUpgrade)
      {
        // Do... something?
      }
    }

    public void Press(Contact contact, int playerId)
    {
    }

    public void Release(Contact contact, int playerId)
    {
    }
  }
}
