using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using SurfaceTower.Model;
using SurfaceTower.Model.EventArguments;
using SurfaceTower.Model.Shape;
using SurfaceTower.Model.Upgrades;

namespace SurfaceTower.Model.Gun
{
  public class MainGun : IGun
  {
    private bool active = false;
    private float orientation;
    private int playerId;
    private ShotPatterns shots;
    private int strength;
    private float improvocity;
    private Dictionary<Upgrade.UpgradeType, Upgrade> upgrades;
    private bool menuShowing;

    public MainGun(int playerId)
    {
      this.playerId = playerId;
      strength = Constants.MAIN_TURRET_DEFAULT_POWER;
      Shots = ShotPatterns.Simple;
      orientation = (float) ((playerId - 1) * Math.PI / 2);
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

    public float Improvocity
    {
      get { return improvocity; }
      private set
      {
        improvocity = value;
        if (improvocity >= 1)
        {
          improvocity = 1;
        }
      }
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
            // Register the listeners.
            m.Music.Click += new EventHandler(OnClick);
            m.Music.Beat += new EventHandler(OnBeat);
          }
          else
          {
            // Deregister the listeners.
            m.Music.Click -= new EventHandler(OnClick);
            m.Music.Beat -= new EventHandler(OnBeat);
          }

          m.Music.Voices[PlayerId].IsActive = value;
        }
      }
    }

    public bool CanUpgrade
    {
      get { return Improvocity == 1; }
      set { Improvocity = value ? 1 : 0; }
    }

    public Dictionary<Upgrade.UpgradeType, Upgrade> Upgrades
    {
      get { return upgrades; }
    }

    public bool UpgradeMenuShowing
    {
      get { return menuShowing; }
    }

    #endregion

    #region Events

    public event EventHandler<ShotArgs> ShotFired;
    public event EventHandler UpgradeReady;
    public event EventHandler UpgradeDone;
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
        //Modifies the starting location and orientation of the bullet.
        Vector2 locMod = Vector2.Transform(shot.PositionModifier, Matrix.CreateRotationZ(Orientation));
        Bullet bullet = new Bullet(Location + locMod, velocity, Strength, shot.Effects, PlayerId);
        App.Instance.Model.Bullets.Add(bullet);
        if (NewBullet != null) NewBullet(this, new BulletArgs(bullet));
      }
      if (ShotFired != null) ShotFired(this, new ShotArgs(Shots));
    }

    public void Killed(Enemy e)
    {
      if (!IsActive)
      {
        return;
      }

      Improvocity += Constants.BASE_IMPROVOCITY * e.Size;
    }

    public void ShowMenu(bool show)
    {
      if (show && !UpgradeMenuShowing && CanUpgrade)
      {
        upgrades = new Dictionary<Upgrade.UpgradeType, Upgrade>(5);
        upgrades.Add(Upgrade.UpgradeType.Strength, Upgrade.CreateUpgrade(Upgrade.UpgradeType.Strength, this));
        upgrades.Add(Upgrade.UpgradeType.Homing, Upgrade.CreateUpgrade(Upgrade.UpgradeType.Homing, this));
        upgrades.Add(Upgrade.UpgradeType.TwoShot, Upgrade.CreateUpgrade(Upgrade.UpgradeType.TwoShot, this));
        upgrades.Add(Upgrade.UpgradeType.Spread, Upgrade.CreateUpgrade(Upgrade.UpgradeType.Spread, this));
        
        menuShowing = true;
        if (UpgradeReady != null) UpgradeReady(this, null);
      }
      else if (!show && UpgradeMenuShowing)
      {
        Queue<ITouchable> iterator = new Queue<ITouchable>(App.Instance.Controller.Touchables);
        ITouchable u;
        while (iterator.Count > 0)
        {
          u = iterator.Dequeue();
          if (u is Upgrade && ((Upgrade) u).UpgradeTarget == this)
          {
            App.Instance.Controller.Touchables.Remove(u);
          }
        }

        menuShowing = false;
        if (UpgradeDone != null) UpgradeDone(this, null);
      }
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
