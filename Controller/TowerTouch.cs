using System.Collections.Generic;

using Microsoft.Surface.Core;
using SurfaceTower.Model;
using SurfaceTower.Model.Gun;

namespace SurfaceTower.Controller
{
  public class TowerTouch : ITouchHandler
  {
    private readonly ICollection<ITouchable> menuOptions = new List<ITouchable>();

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
        foreach (ITouchable option in menuOptions)
        {
          if (option.InRegion(contact.Contact))
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
      if (p.UpgradeMenuShowing)
      {
        p.ShowMenu(false);
      }
    }

    #endregion
  }
}
