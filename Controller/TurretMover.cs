using System;

using Microsoft.Surface.Core;
using SurfaceTower.Model.Gun;
using Microsoft.Xna.Framework;

namespace SurfaceTower.Controller
{
  public class TurretMover : ITouchHandler
  {
    private Turret turret;

    public TurretMover(Turret toMake)
    {
      turret = toMake;
    }

    public void Press(ContactData contact, int playerId)
    {
      if (playerId == turret.PlayerId)
      {
        contact.ContactUpdated += new EventHandler(Update);
        turret.Active = false;
      }
    }

    public void Release(ContactData contact, int playerId)
    {
      if (playerId == turret.PlayerId)
      {
        contact.ContactUpdated -= new EventHandler(Update);
        turret.Active = true;
      }
    }

    public void Touch(ContactData contact, int playerId)
    {
      if (playerId == turret.PlayerId)
      {
        turret.Active = true;
      }
    }

    private void Update(object sender, EventArgs e)
    {
      Contact c = ((ContactData) sender).Contact;
      if (!App.Instance.Model.Tower.Shape.Collides(c))
      {
        turret.Location = new Vector2(c.CenterX, c.CenterY);
      }
    }
  }
}
