using System;
using System.Collections.Generic;

using Microsoft.Surface.Core;
using Microsoft.Xna.Framework;
using SurfaceTower.Model;
using SurfaceTower.Model.Gun;

namespace SurfaceTower.Controller
{
  public class ContactParser
  {
    private readonly IList<ContactData> activeContacts = new List<ContactData>();
    private readonly int[] playerTags = new int[] {0, 0, 0, 0};

    #region Properties

    public IList<ContactData> ActiveContacts
    {
      get { return activeContacts; }
    }

    #endregion

    #region Events

    public event EventHandler<ContactData> ContactAdded;
    public event EventHandler<ContactData> ContactRemoved;

    #endregion

    #region Methods

    public ContactParser()
    {
      ContactAdded += new EventHandler<ContactData>(OnContactAdded);
      ContactRemoved += new EventHandler<ContactData>(OnContactRemoved);
    }

    protected void OnContactAdded(object sender, ContactData e)
    {
      if (e.Contact.IsTagRecognized)
      {
        // Add a player!
        MainGun[] players = App.Instance.Model.Players;
        MainGun[] sortedPlayers;
        
        // Which region?
        int width = App.Instance.GraphicsDevice.PresentationParameters.BackBufferWidth;
        int height = App.Instance.GraphicsDevice.PresentationParameters.BackBufferHeight;
        float x = (float) e.Contact.CenterX / width;
        float y = (float) e.Contact.CenterY / height;

        // Top/right (0/1)
        if (x > y)
        {
          // Top (0)
          if (x + y < 1)
          {
            sortedPlayers = new MainGun[] { players[0], players[x > 0.5 ? 1 : 3], players[x > 0.5 ? 3 : 1], players[2] };
          }
          // Right (1)
          else
          {
            sortedPlayers = new MainGun[] { players[1], players[y > 0.5 ? 2 : 0], players[y > 0.5 ? 0 : 2], players[3] };
          }
        }
        else
        {
          // Left (3)
          if (x + y < 1)
          {
            sortedPlayers = new MainGun[] { players[3], players[y > 0.5 ? 2 : 0], players[y > 0.5 ? 0 : 2], players[1] };
          }
          // Bottom (2)
          else
          {
            sortedPlayers = new MainGun[] { players[2], players[x > 0.5 ? 1 : 3], players[x > 0.5 ? 3 : 1], players[0] };
          }
        }

        foreach (MainGun t in sortedPlayers)
        {
          if (!t.IsActive)
          {
            t.IsActive = true;
            playerTags[t.PlayerId] = e.Contact.Id;
            e.ContactUpdated += new EventHandler(OnTagUpdated);
            break;
          }
        }
      }
    }

    protected void OnContactRemoved(object sender, ContactData e)
    {
      if (e.Contact.IsTagRecognized)
      {
        for (int i = 0; i < 4; i++)
        {
          if (playerTags[i] == e.Contact.Id)
          {
            App.Instance.Model.Players[i].IsActive = false;
            playerTags[i] = 0;
            e.ContactUpdated -= new EventHandler(OnTagUpdated);
            break;
          }
        }
      }
    }

    void OnTagUpdated(object sender, EventArgs e)
    {
      Contact c = ((ContactData) sender).Contact;

      for (int i = 0; i < 4; i++)
      {
        if (playerTags[i] == c.Id)
        {
          App.Instance.Model.Players[i].Orientation = c.Orientation;
        }
      }
    }

    public void ProcessContacts(GameTime gameTime, ReadOnlyContactCollection contacts)
    {
      TimeSpan now = gameTime.TotalRealTime;
      foreach (Contact c in contacts)
      {
        bool found = false;
        // Search the collection of active contacts to see if we already know about this one.
        foreach (ContactData d in activeContacts)
        {
          if (c.Id == d.Contact.Id)
          {
            d.Update(c, now);
            found = true;
            break;
          }
        }

        // Not found? It's a new contact, add it and notify event handlers.
        if (!found)
        {
          ContactData d = new ContactData(c, now);
          activeContacts.Add(d);
          if (ContactAdded != null) ContactAdded(this, d);
        }
      }
      // Now check to see if there are any that need removing - these will have an outdated LastSeen.
      int length = activeContacts.Count;
      for (int i = 0; i < length; i++)
      {
        ContactData d = activeContacts[i];
        if (!d.LastSeen.Equals(now))
        {
          activeContacts.RemoveAt(i);
          i--;
          length--;
          if (ContactRemoved != null) ContactRemoved(this, d);
        }
      }
    }

    #endregion
  }
}
