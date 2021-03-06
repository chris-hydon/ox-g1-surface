﻿using System;
using System.Collections.Generic;

using Microsoft.Surface.Core;
using Microsoft.Xna.Framework;
using SurfaceTower.Model;
using SurfaceTower.Model.Gun;

namespace SurfaceTower.Controller
{
  public struct TouchableContactPair
  {
    private ITouchable touchable;
    private ContactData contact;

    public ITouchable Touchable
    {
      get { return touchable; }
    }

    public ContactData Contact
    {
      get { return contact; }
    }

    public TouchableContactPair(ITouchable touchable, ContactData contact)
    {
      this.touchable = touchable;
      this.contact = contact;
    }
  }

  public class ContactParser
  {
    private readonly IList<ContactData> activeContacts = new List<ContactData>();
    private Queue<ContactData> leavingTags = new Queue<ContactData>();
    private readonly int[] playerTags = new int[] {0, 0, 0, 0};
    private IList<ITouchable> touchables = new List<ITouchable>();
    private readonly IList<TouchableContactPair> pressed = new List<TouchableContactPair>();
    private readonly IList<TouchableContactPair> pendingPressed = new List<TouchableContactPair>();

    #region Properties

    public IList<ContactData> ActiveContacts
    {
      get { return activeContacts; }
    }

    public IList<ITouchable> Touchables
    {
      get { return touchables; }
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
      touchables.Add(App.Instance.Model.Tower);
    }

    public void Restart()
    {
      touchables = new List<ITouchable>();
      touchables.Add(App.Instance.Model.Tower);
    }

    public Contact GetContactByPlayerId(int playerId)
    {
      foreach (ContactData d in ActiveContacts)
      {
        if (d.Contact.Id == playerTags[playerId])
        {
          return d.Contact;
        }
      }
      return null;
    }

    /// <summary>
    /// Determine the ID of the player accociated with a tag contact, if any.
    /// </summary>
    /// <param name="c">The contact to check.</param>
    /// <returns>The PlayerId associated with the contact, or -1 if the contact is not associated with a player.</returns>
    public int GetPlayerIdByContact(Contact c)
    {
      for (int i = 0; i < 4; i++)
      {
        if (c.Id == playerTags[i])
        {
          return i;
        }
      }
      return -1;
    }

    protected void OnContactAdded(object sender, ContactData e)
    {
      if (e.Contact.IsTagRecognized)
      {
        AddPlayer(e);
      }
      else if (e.Contact.IsFingerRecognized)
      {
        foreach (ITouchable t in Touchables)
        {
          if (t.InRegion(e.Contact))
          {
            pendingPressed.Add(new TouchableContactPair(t, e));
          }
        }
      }
    }

    protected void OnContactRemoved(object sender, ContactData e)
    {
      if (e.Contact.IsTagRecognized)
      {
        RemovePlayer(e);
      }
      else if (e.Contact.IsFingerRecognized)
      {
        foreach (TouchableContactPair p in pressed)
        {
          if (p.Contact == e)
          {
            int player = WhichPlayer(e);
            if (player != -1)
            {
              p.Touchable.Controller.Release(e, player);
            }
            return;
          }
        }
        int length = pendingPressed.Count;
        for (int i = 0; i < length; i++)
        {
          if (pendingPressed[i].Contact == e)
          {
            if (pendingPressed[i].Touchable.InRegion(e.Contact))
            {
              int player = WhichPlayer(e);
              if (player != -1)
              {
                pendingPressed[i].Touchable.Controller.Touch(e, player);
              }
            }
            pendingPressed.RemoveAt(i);
            return;
          }
        }
      }
    }

    protected void OnTagUpdated(object sender, EventArgs e)
    {
      ContactData c =  (ContactData) sender;

      for (int i = 0; i < 4; i++)
      {
        if (playerTags[i] == c.Contact.Id)
        {
          float o = (float) ((i - 1) * Math.PI / 2) + c.Contact.Orientation - c.InitialOrientation;
          App.Instance.Model.Players[i].Orientation = o;
        }
      }
    }

    private void AddPlayer(ContactData e)
    {
      BaseModel model = App.Instance.Model;

      if (model.NumberOfPlayers < 4)
      {
        int newPlayer = WhichPlayer(new Vector2(e.Contact.CenterX, e.Contact.CenterY), false);
        model.PlayerJoin(newPlayer);
        playerTags[newPlayer] = e.Contact.Id;
        e.ContactUpdated += new EventHandler(OnTagUpdated);
      }
    }

    private void RemovePlayer(ContactData e)
    {
      for (int i = 0; i < 4; i++)
      {
        if (playerTags[i] == e.Contact.Id)
        {
          App.Instance.Model.PlayerLeave(i);
          playerTags[i] = 0;
          e.ContactUpdated -= new EventHandler(OnTagUpdated);
          break;
        }
      }
    }

    /// <summary>
    /// Determines the closest active (or inactive) player to the vector check.
    /// </summary>
    /// <param name="check">The vector to check.</param>
    /// <param name="active">True searches for active players only, false searches for inactive positions only.</param>
    /// <returns>The playerId of the relevant player or position. -1 if no such player exists.</returns>
    private int WhichPlayer(Vector2 check, bool active)
    {
      int[] sortedPlayers;

      // Which region?
      float x = (float) check.X / App.Instance.GraphicsDevice.Viewport.Width;
      float y = (float) check.Y / App.Instance.GraphicsDevice.Viewport.Height;

      // Top/right (0/1)
      if (x > y)
      {
        // Top (0)
        if (x + y < 1)
        {
          sortedPlayers = new int[] { 0, x > 0.5 ? 1 : 3, x > 0.5 ? 3 : 1, 2 };
        }
        // Right (1)
        else
        {
          sortedPlayers = new int[] { 1, y > 0.5 ? 2 : 0, y > 0.5 ? 0 : 2, 3 };
        }
      }
      else
      {
        // Left (3)
        if (x + y < 1)
        {
          sortedPlayers = new int[] { 3, y > 0.5 ? 2 : 0, y > 0.5 ? 0 : 2, 1 };
        }
        // Bottom (2)
        else
        {
          sortedPlayers = new int[] { 2, x > 0.5 ? 1 : 3, x > 0.5 ? 3 : 1, 0 };
        }
      }

      for (int i = 0; i < 4; i++)
      {
        if (App.Instance.Model.Players[sortedPlayers[i]].IsActive == active)
        {
          return sortedPlayers[i];
        }
      }

      return -1;
    }

    /// <summary>
    /// Of the active players, this method decides who was responsible for the given contact.
    /// </summary>
    /// <param name="c">The contact to check. Type should be tag or finger.</param>
    /// <returns>The playerid of the most likely responsible active player. -1 if no such player exists.</returns>
    private int WhichPlayer(ContactData c)
    {
      MainGun[] players = App.Instance.Model.Players;
      int[] sortedPlayers = null;
      int octant = (int) (4 * c.InitialOrientation / Math.PI);

      switch (octant)
      {
        case 0: // WNW
          sortedPlayers = new int[] { 3, 0, 2, 1 };
          break;
        case 1: // NNW
          sortedPlayers = new int[] { 0, 3, 1, 2 };
          break;
        case 2: // NNE
          sortedPlayers = new int[] { 0, 1, 3, 2 };
          break;
        case 3: // ENE
          sortedPlayers = new int[] { 1, 0, 2, 3 };
          break;
        case 4: // ESE
          sortedPlayers = new int[] { 1, 2, 0, 3 };
          break;
        case 5: // SSE
          sortedPlayers = new int[] { 2, 1, 3, 0 };
          break;
        case 6: // SSW
          sortedPlayers = new int[] { 2, 3, 1, 0 };
          break;
        case 7: // WSW
          sortedPlayers = new int[] { 3, 2, 0, 1 };
          break;
      }

      for (int i = 0; i < 4; i++)
      {
        if (players[sortedPlayers[i]].IsActive)
        {
          return sortedPlayers[i];
        }
      }

      return -1;
    }

    private Contact FindReturned(ReadOnlyContactCollection contacts, ContactData gone)
    {
      foreach (Contact c in contacts)
      {
        // Not a tag, or is already associated with a player.
        if (!c.IsTagRecognized || GetPlayerIdByContact(c) != -1)
        {
          continue;
        }

        for (int i = 0; i < 4; i++)
        {
          if (playerTags[i] == gone.Contact.Id)
          {
            if (Vector2.Distance(new Vector2(c.CenterX, c.CenterY), gone.LastLocation) < 50)
            {
              playerTags[i] = c.Id;
              return c;
            }
            return null;
          }
        }
      }
      return null;
    }

    public void ProcessContacts(GameTime gameTime, ReadOnlyContactCollection contacts)
    {
      TimeSpan now = gameTime.TotalRealTime;

      // First process tags that are leaving.
      Queue<ContactData> q = leavingTags;
      ContactData current;
      leavingTags = new Queue<ContactData>();
      while (q.Count > 0)
      {
        current = q.Dequeue();
        if (current.LastSeen + new TimeSpan(0, 0, 0, 1) < now)
        {
          if (ContactRemoved != null) ContactRemoved(this, current);
        }
        else
        {
          Contact returned = FindReturned(contacts, current);
          if (returned != null)
          {
            current.Update(returned, now);
            activeContacts.Add(current);
          }
          else
          {
            leavingTags.Enqueue(current);
          }
        }
      }

      // Now get current contacts.
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
          if (d.Contact.IsTagRecognized)
          {
            leavingTags.Enqueue(d);
          }
          else
          {
            if (ContactRemoved != null) ContactRemoved(this, d);
          }
        }
      }

      // Finally, check to see if anything in pendingPressed can be moved to pressed.
      length = pendingPressed.Count;
      for (int i = 0; i < length; i++)
      {
        TouchableContactPair p = pendingPressed[i];
        if ((p.Contact.LastSeen - p.Contact.TimeAdded).TotalMilliseconds > 100 &&
          p.Touchable.InRegion(p.Contact.Contact))
        {
          pressed.Add(p);
          pendingPressed.RemoveAt(i);
          i--;
          length--;
          int player = WhichPlayer(p.Contact);
          if (player != -1)
          {
            p.Touchable.Controller.Press(p.Contact, player);
          }
        }
      }
    }

    #endregion
  }
}
