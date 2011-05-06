using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

using SurfaceTower.Model.EventArguments;

namespace SurfaceTower.Model
{
  #region Structures

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
    public TimeSpan time;
    public EnemyTimePair(Enemy enemy, TimeSpan time){
      this.enemy = enemy;
      this.time = time;
    }
  }

  public struct CollisionTimePair
  {
    public Collision collision;
    public TimeSpan time;
    public CollisionTimePair(Collision collision, TimeSpan time){
      this.collision = collision;
      this.time = time;
    }
  }

  #endregion

  public class BaseModel
  {
    protected ICollection<EnemyTimePair> dying = new LinkedList<EnemyTimePair>(), dead = new LinkedList<EnemyTimePair>();
    protected ICollection<CollisionTimePair> collisions = new LinkedList<CollisionTimePair>();
    protected ICollection<Bullet> bullets = new LinkedList<Bullet>();
    protected ICollection<Enemy> living = new LinkedList<Enemy>();

    protected TimeSpan lastUpdate;
    protected Music music = new Music();
    protected MainGun[] players = new MainGun[4] { new MainGun(0), new MainGun(1), new MainGun(2), new MainGun(3) };
    protected Tower tower = new Tower();
    protected ICollection<Turret> turrets = new LinkedList<Turret>();

    #region Properties

    public Music Music
    {
      get { return music; }
    }

    public MainGun[] Players
    {
      get { return players; }
    }

    public Tower Tower
    {
      get { return tower; }
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

    public TimeSpan LastUpdate
    {
      get { return lastUpdate; }
    }
    #endregion

    #region Events

    public event EventHandler<UpdateArgs> Update;

    #endregion

    #region Methods

    public virtual void OnUpdate(GameTime gameTime)
    {
      lastUpdate = gameTime.TotalRealTime;
      Update(this, new UpdateArgs(LastUpdate));
      Queue<Enemy> deathRow = new Queue<Enemy>();
      foreach(Bullet b in bullets)
      {
        foreach(Enemy e in living)
        {
          if (e.Collides(b))
          {
            e.Health -= b.Power;
            if (e.Health <= 0)
            {
              deathRow.Enqueue(e);
            }
          }
        }
        while (deathRow.Count > 0)
        {
          MakeDying(new EnemyTimePair(deathRow.Dequeue(), LastUpdate));
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
      Living.Remove(etp.enemy);
      dying.Add(etp);
    }

    public void Kill(EnemyTimePair etp)
    {
      dying.Remove(etp);
      dead.Add(new EnemyTimePair(etp.enemy, LastUpdate));
    }

    public void Cremate(EnemyTimePair etp)
    {
      dead.Remove(etp);
    }

    #endregion
  }
}
