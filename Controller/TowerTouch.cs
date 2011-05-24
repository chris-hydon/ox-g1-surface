using Microsoft.Surface.Core;
using SurfaceTower.Model;

namespace SurfaceTower.Controller
{
  public class TowerTouch : ITouchHandler
  {

    #region Methods

    public void Press(ContactData contact, int playerId)
    {
    }

    public void Release(ContactData contact, int playerId)
    {
    }

    public void Touch(ContactData contact, int playerId)
    {
      // Upgrade ready?
      if (App.Instance.Model.Players[playerId].CanUpgrade)
      {
        // Make upgrade menu appear.
      }
    }

    #endregion
  }
}
