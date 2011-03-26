using System;
using Microsoft.Xna.Framework;

using SurfaceTower.Model.EventArguments;

namespace SurfaceTower.Model
{
  public class MainTurret
  {
    private bool active = false;
    private float orientation;
    private int playerId;

    public MainTurret(int playerId)
    {
      this.playerId = playerId;
    }

    #region Properties

    public float Orientation
    {
      get { return orientation; }
      set
      {
        orientation = value % 360;
      }
    }

    public int PlayerId
    {
      get { return playerId; }
    }

    public bool IsActive
    {
      get { return active; }
      set
      {
        // Only do this stuff if we're changing the value of active.
        if (active != value)
        {
          active = value;
          BaseModel m = App.Instance.Model;

          if (value)
          {
            m.Update += new EventHandler<UpdateArgs>(OnUpdate);
            m.Music.Click += new EventHandler(OnClick);
            m.Music.Beat += new EventHandler(OnBeat);
          }
          else
          {
            // This bit is magic.
            m.Update -= new EventHandler<UpdateArgs>(OnUpdate);
            m.Music.Click -= new EventHandler(OnClick);
            m.Music.Beat -= new EventHandler(OnBeat);
          }

          m.Music.Voices[PlayerId].IsActive = value;
        }
      }
    }

    #endregion

    #region Events

    public event EventHandler ShotFired;
    public event EventHandler UpgradeReady;

    #endregion

    #region Methods

    /// <summary>
    /// Called in response to the BaseModel.Update signal.
    /// </summary>
    /// <param name="sender">The BaseModel object which sent the Update signal.</param>
    /// <param name="args">UpdateArgs object containing the current TimeSpan snapshot.</param>
    public void OnUpdate(object sender, UpdateArgs args)
    {
    }

    /// <summary>
    /// Called in response to the Music.Click signal.
    /// </summary>
    /// <param name="sender">The Music object which sent the Click signal.</param>
    /// <param name="e">Always null</param>
    public void OnClick(object sender, EventArgs e)
    {
    }

    /// <summary>
    /// Called in response to the Music.Beat signal.
    /// </summary>
    /// <param name="sender">The Music object which sent the Beat signal.</param>
    /// <param name="e">Always null</param>
    public void OnBeat(object sender, EventArgs e)
    {
    }

    #endregion
  }
}
