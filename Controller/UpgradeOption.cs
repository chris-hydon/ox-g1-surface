using Microsoft.Surface.Core;
using Microsoft.Xna.Framework;
using SurfaceTower.Model;
using SurfaceTower.Model.Gun;
using SurfaceTower.Model.Upgrades;

namespace SurfaceTower.Controller
{
  public class UpgradeOption : ITouchHandler
  {
    private Upgrade upgrade;

    #region Methods

    public UpgradeOption(Upgrade upgrade, int playerId)
    {
      this.upgrade = upgrade;
    }

    public void Press(ContactData contact, int playerId)
    {
      // Do nothing - the user might change their mind.
    }

    public void Release(ContactData contact, int playerId)
    {
      // Did the user change their mind? If not, do a regular Touch.
      if (upgrade.InRegion(contact.Contact))
      {
        Touch(contact, playerId);
      }
    }

    public void Touch(ContactData contact, int playerId)
    {
      upgrade.Apply();

      // Hide the menu.
      App.Instance.Model.Players[playerId].ShowMenu(false);
      App.Instance.Model.Players[playerId].CanUpgrade = false;
    }

    #endregion
  }
}
