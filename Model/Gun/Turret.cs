using System;

using Microsoft.Xna.Framework;
using SurfaceTower.Model;
using SurfaceTower.Model.EventArguments;
using SurfaceTower.Model.Shape;


namespace SurfaceTower.Model.Gun
{
  public class Turret : IEntity, IGun
  {
    protected Circle shape;
    protected float orientation;
    protected int playerId;
    protected ShotPatterns shots;
    protected int strength;

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

    #endregion

    #region Events

    public event EventHandler<ShotArgs> ShotFired;
    public event EventHandler UpgradeReady;
    public event EventHandler<BulletArgs> NewBullet;

    #endregion

    #region Methods

    public Turret(Vector2 location, int owner)
    {
      this.shape = new Circle(Constants.TURRET_RADIUS, location);
      this.playerId = owner;
      Shots = ShotPatterns.Simple;
      strength = Constants.TURRET_DEFAULT_POWER;
      orientation = (owner - 1) * (float) Math.PI / 2;
      App.Instance.Model.Update += new EventHandler<UpdateArgs>(OnUpdate);
      App.Instance.Model.Music.Click += new EventHandler(OnClick);
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
      Enemy focus = Enemy.FindNearestLiving(this);

      if (focus != null)
      {
        // Get the difference between the bullet's location and that of the target.
        Vector2 target = focus.Location - Location;

        // Figure out the relative orientation and cap the rate of rotation.
        float orientMod = (float) Math.Atan2(target.Y, target.X) - Orientation;
        float turnSpeed = Constants.TURRET_TURNSPEED / Constants.UPDATES_PER_SECOND;
        if (Math.Abs(orientMod) > turnSpeed)
        {
          orientMod = orientMod > 0 ? turnSpeed : -turnSpeed;
        }

        Orientation += orientMod;
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
    
    #endregion
  }
}
