using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

using SurfaceTower.Model.EventArguments;

namespace SurfaceTower.Model
{
  #region Structures

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

  #endregion

  public class BaseModel
  {
    public static BaseModel INSTANCE = new BaseModel();
    protected ICollection<EnemyTimePair> dying, dead = new LinkedList<EnemyTimePair>();
    protected ICollection<CollisionTimePair> collisions = new LinkedList<CollisionTimePair>();
    protected ICollection<ContactTriple> contacts = new LinkedList<ContactTriple>();
    protected ICollection<Bullet> bullets = new LinkedList<Bullet>();
    protected ICollection<Enemy> living = new LinkedList<Enemy>();

    protected Music music = new Music();
    protected MainTurret[] players = new MainTurret[4] { new MainTurret(0), new MainTurret(1), new MainTurret(2), new MainTurret(3) };
    protected ICollection<Turret> turrets = new LinkedList<Turret>();

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

    public virtual void OnUpdate(GameTime gameTime)
    {
      Update(this, new UpdateArgs(gameTime.TotalRealTime));
      foreach(Bullet b in bullets)
      {
        foreach(Enemy e in living)
        {
          if (e.IsHit(b))
          {
            e.Health -= b.power;
            if (e.Health <= 0)
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
