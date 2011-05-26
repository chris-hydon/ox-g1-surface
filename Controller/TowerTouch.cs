using System.Collections.Generic;

using Microsoft.Surface.Core;
using SurfaceTower.Model;
using SurfaceTower.Model.Gun;
using SurfaceTower.Model.Upgrades;

namespace SurfaceTower.Controller
{
  public class TowerTouch : ITouchHandler
  {
    #region Methods

    public void Press(ContactData contact, int playerId)
    {
      Touch(contact, playerId);
    }

    public void Release(ContactData contact, int playerId)
    {
      MainGun p = App.Instance.Model.Players[playerId];

      // Upgrade ready?
      if (p.CanUpgrade && p.UpgradeMenuShowing)
      {
        // If contact is currently over one of the menu items, fire that
        // item's Touch.
        foreach (ITouchable option in App.Instance.Controller.Touchables)
        {
          if (option is Upgrade && ((Upgrade) option).UpgradeTarget == p && option.InRegion(contact.Contact))
          {
            option.Controller.Touch(contact, playerId);
            break;
          }
        }

        // Hide the menu.
        p.ShowMenu(false);
      }
    }

    public void Touch(ContactData contact, int playerId)
    {
      MainGun p = App.Instance.Model.Players[playerId];

      // Upgrade ready?
      if (p.CanUpgrade && !p.UpgradeMenuShowing)
      {
        // Make upgrade menu appear.
        p.ShowMenu(true);
      }
      else if (p.UpgradeMenuShowing)
      {
        p.ShowMenu(false);
      }
    }

    #endregion
  }
}
