using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using SurfaceTower.Model;
using SurfaceTower.Model.EventArguments;
using SurfaceTower.Model.Shape;

namespace SurfaceTower.Model.Gun
{
  public class MainGun : IGun
  {
    private bool active = false;
    private float orientation;
    private int playerId;
    private ShotPatterns shots;
    private int strength;

    public MainGun(int playerId)
    {
      this.playerId = playerId;
      strength = Constants.MAIN_TURRET_DEFAULT_POWER;
      Shots = ShotPatterns.Simple;
    }

    #region Properties

    public float Orientation
    {
      get { return orientation; }
      set
      {
        float cap = (float) Math.PI * 2;
        orientation = value;
        while (orientation < 0) orientation += cap;
        while (orientation >= cap) orientation -= cap;
      }
    }

    public Vector2 Location
    {
      get
      {
        Vector2 offset = Constants.MAIN_TURRET_RADIUS * (new Vector2((float) Math.Cos(Orientation), (float) Math.Sin(Orientation)));
        return App.Instance.Model.Tower.Location + offset;
      }
    }

    public int PlayerId
    {
      get { return playerId; }
    }

    public int Strength
    {
      get { return strength; }
      set { strength = value; }
    }

    public ShotPatterns Shots
    {
      get { return shots; }
      set { shots = value; }
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
            m.Music.Click += new EventHandler(OnClick);
            m.Music.Beat += new EventHandler(OnBeat);
          }
          else
          {
            // This bit is magic.
            m.Music.Click -= new EventHandler(OnClick);
            m.Music.Beat -= new EventHandler(OnBeat);
          }

          m.Music.Voices[PlayerId].IsActive = value;
        }
      }
    }

    #endregion

    #region Events

    public event EventHandler<ShotArgs> ShotFired;
    public event EventHandler UpgradeReady;
    public event EventHandler<BulletArgs> NewBullet;

    #endregion

    #region Methods

    /// <summary>
    /// Shoot this gun's ShotPatterns. It fires one bullet per ShotPattern with the
    /// appropriate parameters, with NewBullet fired for each bullet, then finally signals
    /// the ShotFired event.
    /// </summary>
    public void Shoot()
    {
      foreach (ShotPattern shot in Shots)
      {
        Vector2 velocity = Constants.BULLET_VELOCITY * new Vector2(
          (float) Math.Cos(Orientation + shot.OrientationModifier),
          (float) Math.Sin(Orientation + shot.OrientationModifier)
        );
        Vector2 locMod = Vector2.Transform(shot.PositionModifier, Matrix.CreateRotationZ(Orientation));
        Bullet bullet = new Bullet(Location + locMod, velocity, Strength, shot.Effects, PlayerId);
        App.Instance.Model.Bullets.Add(bullet);
        if (NewBullet != null) NewBullet(this, new BulletArgs(bullet));
      }
      if (ShotFired != null) ShotFired(this, new ShotArgs(Shots));
    }

    /// <summary>
    /// Called in response to the Music.Click signal.
    /// </summary>
    /// <param name="sender">The Music object which sent the Click signal.</param>
    /// <param name="e">Always null</param>
    public void OnClick(object sender, EventArgs e)
    {
      Shoot();
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
