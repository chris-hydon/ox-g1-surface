using SurfaceTower.VideoEngine.Touchable;

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
  }
}
