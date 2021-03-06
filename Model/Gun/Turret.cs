﻿using System;
using System.Collections.Generic;

using Microsoft.Surface.Core;
using Microsoft.Xna.Framework;
using SurfaceTower.Model;
using SurfaceTower.Model.EventArguments;
using SurfaceTower.Model.Shape;
using SurfaceTower.Model.Upgrades;
using SurfaceTower.Controller;

namespace SurfaceTower.Model.Gun
{
  public class Turret : IEntity, IGun, ITouchable
  {
    protected IShape shape;
    protected float orientation;
    protected int playerId;
    protected ShotPatterns shots;
    protected int strength;
    protected bool exists = false;
    protected bool active = false;
    protected ITouchHandler controller;
    protected Dictionary<Upgrade.UpgradeType, Upgrade> upgrades;

    #region Properties

    public IShape Shape
    {
      get { return shape; }
    }

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
      get { return shape.Origin; }
      set { shape.Origin = value; }
    }

    public ShotPatterns Shots
    {
      get { return shots; }
      set { shots = value; }
    }

    public int Strength
    {
      get { return strength; }
      set { strength = value; }
    }

    public int PlayerId
    {
      get { return playerId; }
    }

    /// <summary>
    /// Whether or not the turret exists yet. Turrets start in a "bubble", which must be popped to gain control.
    /// Setting Active to true for the first time sets Exists to true.
    /// </summary>
    public bool Exists
    {
      get { return exists; }
    }

    /// <summary>
    /// Turrets are inactive if they don't exist, or if they're currently being moved by their owner. Turrets
    /// only shoot bullets if they are active.
    /// </summary>
    public bool Active
    {
      get { return active; }
      set
      {
        if (value)
        {
          if (!Exists)
          {
            App.Instance.Model.Update -= new EventHandler<UpdateArgs>(GhostUpdate);
            exists = true;
          }
          App.Instance.Model.Update += new EventHandler<UpdateArgs>(OnUpdate);
          App.Instance.Model.Music.Click += new EventHandler(OnClick);
        }
        else
        {
          App.Instance.Model.Update -= new EventHandler<UpdateArgs>(OnUpdate);
          App.Instance.Model.Music.Click -= new EventHandler(OnClick);
        }
        active = value;
      }
    }

    public ITouchHandler Controller
    {
      get { return controller; }
    }

    public Dictionary<Upgrade.UpgradeType, Upgrade> Upgrades
    {
      get { return upgrades; }
    }

    #endregion

    #region Events

    public event EventHandler<ShotArgs> ShotFired;
    public event EventHandler UpgradeReady;
    public event EventHandler UpgradeDone;
    public event EventHandler<BulletArgs> NewBullet;

    #endregion

    #region Methods

    public Turret(Vector2 location, int owner)
    {
      Vector2 size = new Vector2(Constants.TURRET_WIDTH, Constants.TURRET_HEIGHT);
      this.shape = new SurfaceTower.Model.Shape.Rectangle(location - (size / 2), location + (size / 2));
      this.playerId = owner;
      Shots = ShotPatterns.Simple;
      strength = Constants.TURRET_DEFAULT_POWER;
      orientation = (owner - 1) * (float) Math.PI / 2;
      controller = new TurretMover(this);
      App.Instance.Controller.Touchables.Add(this);
      App.Instance.Model.Update += new EventHandler<UpdateArgs>(GhostUpdate);
    }

    /// <summary>
    /// Shoot this gun's ShotPatterns. It fires one bullet per ShotPattern with the
    /// appropriate parameters, with NewBullet fired for each bullet, then finally signals
    /// the ShotFired event.
    /// </summary>
    public void Shoot()
    {
      foreach (ShotPattern shot in Shots)
      {
        // Infrequent bullets should only be shot occasionally.
        if ((((shot.Effects & Effects.Infrequent) != 0) && App.Instance.Model.Music.ClickCount % 2 != 0) ||
          (((shot.Effects & Effects.VeryInfrequent) != 0) && App.Instance.Model.Music.ClickCount % 4 != 0))
        {
          continue;
        }

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
    /// Called in response to the BaseModel.Update signal. This method is responsible for rotating
    /// the turret to face the nearest enemy.
    /// </summary>
    /// <param name="sender">The BaseModel object which sent the Update signal.</param>
    /// <param name="e">The UpdateArgs containing the current TimeSpan.</param>
    void OnUpdate(object sender, UpdateArgs e)
    {
      // Find the nearest enemy.
      Enemy focus = Enemy.FindNearestLiving(this, playerId);

      if (focus != null)
      {
        // Get the difference between the bullet's location and that of the target.
        Vector2 target = focus.Location - Location;

        // Figure out the relative orientation and cap the rate of rotation.
        float orientMod = (float) Math.Atan2(target.Y, target.X) - Orientation;
        float turnSpeed = Constants.TURRET_TURNSPEED / Constants.UPDATES_PER_SECOND;
        while (orientMod < -Math.PI) orientMod += (float) Math.PI * 2;
        if (Math.Abs(orientMod) > turnSpeed)
        {
          orientMod = orientMod > 0 ? turnSpeed : -turnSpeed;
        }
        Orientation += orientMod;
      }
    }

    /// <summary>
    /// Called in response to BaseModel.Update when the turret doesn't exist yet. Float around and
    /// rotate idly.
    /// </summary>
    /// <param name="sender">The BaseModel object which sent the Update signal.</param>
    /// <param name="e">The UpdateArgs containing the current TimeSpan.</param>
    void GhostUpdate(object sender, UpdateArgs e)
    {
      BaseModel model = (BaseModel) sender;
      Vector2 originPoint = model.Tower.Location;
      switch (PlayerId)
      {
        case 0:
          originPoint -= new Vector2(0, originPoint.Y / 2);
          break;
        case 1:
          originPoint += new Vector2(originPoint.X / 2, 0);
          break;
        case 2:
          originPoint += new Vector2(0, originPoint.Y / 2);
          break;
        case 3:
          originPoint -= new Vector2(originPoint.X / 2, 0);
          break;
      }
      Vector2 acc = originPoint - Location;
      //acc.Normalize();
      Vector2 vel = Vector2.Transform(acc, Matrix.CreateRotationZ((float) Math.PI / 2)) + (acc / Constants.UPDATES_PER_SECOND);
      Location += vel / Constants.UPDATES_PER_SECOND;
      Orientation += Constants.TURRET_TURNSPEED / Constants.UPDATES_PER_SECOND;
    }

    public void ShowMenu(bool show)
    {
      if (show && App.Instance.Model.Players[playerId].CanUpgrade)
      {
        if (UpgradeReady != null) UpgradeReady(this, null);
      }
      else if (!show)
      {
        if (UpgradeDone != null) UpgradeDone(this, null);
      }
    }

    public void ApplyUpgrade(Upgrade.UpgradeType type)
    {
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
    
    public bool InRegion(Contact target)
    {
      return Shape.Collides(target);
    }

    #endregion
  }
}
