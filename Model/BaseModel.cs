using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

using SurfaceTower.Model.EventArguments;

namespace SurfaceTower.Model
{
  public struct Bullet
  {
    public int x, y, rotation, speed, power;
    public Bullet(int x, int y, int rotation, int speed, int power)
    {
      this.x = x;
      this.y = y;
      this.rotation = rotation;
      this.speed = speed;
      this.power = power;
    }
    public void Move()
    {
      x += (int)Math.Ceiling(speed * Math.Cos(rotation));
      y += (int)Math.Ceiling(speed * Math.Sin(rotation));
    }
  }
  public struct Collision
  {
    public enum CollisionType { BULLET_ENEMY, ENEMY_TOWER };
    public int x, y;
    public CollisionType type;
    public Collision(int x, int y, CollisionType type)
    {
      this.x = x;
      this.y = y;
      this.type = type;
    }
  }
  public struct EnemyTimePair
  {
    public Enemy enemy;
    public GameTime time;
    public EnemyTimePair(Enemy enemy, GameTime time){
      this.enemy = enemy;
      this.time = time;
    }
  }
  public struct CollisionTimePair
  {
    public Collision collision;
    public GameTime time;
    public CollisionTimePair(Collision collision, GameTime time){
      this.collision = collision;
      this.time = time;
    }
  }
  public struct ContactTriple
  {
    public int x, y;
    public GameTime time;
    public ContactTriple(int x, int y, GameTime time)
    {
      this.x = x;
      this.y = y;
      this.time = time;
    }
  }
  public class BaseModel
  {

    public static BaseModel INSTANCE = new BaseModel();
    private ICollection<EnemyTimePair> dying, dead = new LinkedList<EnemyTimePair>();
    private ICollection<CollisionTimePair> collisions = new LinkedList<CollisionTimePair>();
    private ICollection<ContactTriple> contacts = new LinkedList<ContactTriple>();
    private ICollection<Bullet> bullets = new LinkedList<Bullet>();
    private ICollection<Enemy> living = new LinkedList<Enemy>();

    private bool first;
    private Music music = new Music();
    private MainTurret[] players = new MainTurret[4] { new MainTurret(0), new MainTurret(1), new MainTurret(2), new MainTurret(3) };
    private ICollection<Turret> turrets = new LinkedList<Turret>();

    #region Properties

    public Music Music
    {
      get { return music; }
    }

    public MainTurret[] Players
    {
      get { return players; }
    }

    public ICollection<Turret> Turrets
    {
      get { return turrets; }
    }

    public ICollection<Enemy> Living
    {
      get { return living; }
    }
    public ICollection<EnemyTimePair> Dying
    {
      get { return dying; }
    }
    public ICollection<EnemyTimePair> Dead
    {
      get { return dead; }
    }
    public ICollection<Bullet> Bullets
    {
      get { return bullets; }
    }
    public ICollection<CollisionTimePair> Collisions
    {
      get { return collisions; }
    }
    public ICollection<ContactTriple> Contacts
    {
      get { return contacts; }
    }
    #endregion

    #region Events

    public event EventHandler<UpdateArgs> Update;

    #endregion

    #region Methods

    public void FirstUpdate(GameTime gameTime)
    {
      // Some simple demo stuff.
      players[0].IsActive = true;
      players[0].Orientation = 45;

      // Audio folks, something like this should be in your initialize, not mine!
      Music.TimeSignature = new TimeSignature(4, 4);
      Music.Tempo = 60;
      Music.ClicksPerBeat = 8;
      Music.Start(gameTime.TotalRealTime);

      Music.Beat += new EventHandler(OnBeat);
    }

    public void OnUpdate(GameTime gameTime)
    {
      if (!first)
      {
        FirstUpdate(gameTime);
        first = true;
      }
      Update(this, new UpdateArgs(gameTime.TotalRealTime));
      foreach(Bullet b in bullets)
      {
        foreach(Enemy e in living)
        {
          if(Math.Abs(b.x-e.X) <= e.Size && Math.Abs(b.y-e.Y) <= e.Size)
          {
            e.Health -= b.power;
            if(e.Health <= 0)
            {
              MakeDying(new EnemyTimePair(e, gameTime));
            }
          }
        }
        b.Move();
      }
      foreach (Enemy e in living)
      {
        e.Move();
      }
    }

    /// <summary>
    /// Called in response to the Music.Beat signal.
    /// </summary>
    /// <param name="sender">The Music object which sent the Beat signal.</param>
    /// <param name="e">Always null</param>
    public void OnBeat(object sender, EventArgs e)
    {
      // Some demo stuff.
      players[0].Orientation += 18;
    }

    public void MakeDying(EnemyTimePair etp)
    {
      dying.Add(etp);
    }
    public void Kill(EnemyTimePair etp)
    {
      dying.Remove(etp);
      //dead.Add(new EnemyTimePair(etp.enemy, CURRENT-GAME-TIME));
    }
    public void Cremate(EnemyTimePair etp)
    {
      dead.Remove(etp);
    }
    #endregion
  }
}
