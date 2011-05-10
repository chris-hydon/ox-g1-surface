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
    protected float orientation = 0;
    protected int playerId;
    protected ShotPatterns shots;
    protected int strength = 1;

    #region Properties

    public IShape Shape
    {
      get { return shape; }
    }

    public float Orientation
    {
      get { return orientation; }
      set { orientation = value; }
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
    }
    
    public void Shoot()
    {
      foreach (ShotPattern shot in Shots)
      {
        Vector2 velocity = new Vector2(
          (float) Math.Cos(Orientation + shot.OrientationModifier),
          (float) Math.Sin(Orientation + shot.OrientationModifier)
        );
        Vector2 locMod = Vector2.Transform(shot.PositionModifier, Matrix.CreateRotationZ(Orientation));
        Bullet bullet = new Bullet(App.Instance.Model.Tower.Location + locMod, velocity, Strength, shot.Effects, PlayerId);
        App.Instance.Model.Bullets.Add(bullet);
        if (NewBullet != null) NewBullet(this, new BulletArgs(bullet));
      }
      if (ShotFired != null) ShotFired(this, new ShotArgs(Shots));
    }
    
    #endregion
  }
}
