using System.Collections.Generic;

using Microsoft.Surface.Core;
using SurfaceTower.Model;

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
      // Upgrade ready?
      if (App.Instance.Model.Players[playerId].CanUpgrade)
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
        App.Instance.Model.Players[playerId].ShowMenu(false);
      }
    }

    public void Touch(ContactData contact, int playerId)
    {
      // Upgrade ready?
      if (App.Instance.Model.Players[playerId].CanUpgrade)
      {
        // Make upgrade menu appear.
        App.Instance.Model.Players[playerId].ShowMenu(true);
      }
    }

    #endregion
  }
}
