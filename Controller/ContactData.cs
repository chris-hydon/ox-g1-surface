using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Surface.Core;
using Microsoft.Xna.Framework;

namespace SurfaceTower.Controller
{
  public class ContactData : EventArgs
  {
    private Contact contact;
    private TimeSpan lastSeen;
    private TimeSpan timeAdded;
    private float initialOrientation;
    private Vector2 lastLocation;

    #region Properties

    public Contact Contact
    {
      get { return contact; }
    }

    public float InitialOrientation
    {
      get { return initialOrientation; }
    }

    public TimeSpan TimeAdded
    {
      get { return timeAdded; }
    }

    public TimeSpan LastSeen
    {
      get { return lastSeen; }
    }

    public Vector2 LastLocation
    {
      get { return lastLocation; }
    }

    #endregion

    #region Methods

    public ContactData(Contact contact, TimeSpan time)
    {
      this.contact = contact;
      this.lastSeen = time;
      this.timeAdded = time;
      this.initialOrientation = contact.Orientation;
      this.lastLocation = new Vector2(contact.CenterX, contact.CenterY);
    }

    public void Update(Contact contact, TimeSpan time)
    {
      this.contact = contact;
      this.lastSeen = time;
      this.lastLocation = new Vector2(contact.CenterX, contact.CenterY);
      if (ContactUpdated != null) ContactUpdated(this, null);
    }

    #endregion

    #region Events

    public event EventHandler ContactUpdated;

    #endregion
  }
}
