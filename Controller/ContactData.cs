using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Surface.Core;

namespace SurfaceTower.Controller
{
  public class ContactData : EventArgs
  {
    private Contact contact;
    private TimeSpan lastSeen;
    private TimeSpan timeAdded;
    private float initialOrientation;

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

    #endregion

    #region Methods

    public ContactData(Contact contact, TimeSpan time)
    {
      this.contact = contact;
      this.lastSeen = time;
      this.timeAdded = time;
      this.initialOrientation = contact.Orientation;
    }

    public void Update(Contact contact, TimeSpan time)
    {
      this.contact = contact;
      this.lastSeen = time;
      if (ContactUpdated != null) ContactUpdated(this, null);
    }

    #endregion

    #region Events

    public event EventHandler ContactUpdated;

    #endregion
  }
}
