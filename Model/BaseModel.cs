using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

using SurfaceTower.Model.EventArguments;
using SurfaceTower.Model.Gun;

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

  public struct EnemyTimeWho
  {
    public Enemy enemy;
    public TimeSpan time;
    public int who;
    public EnemyTimeWho(Enemy enemy, TimeSpan time, int who)
    {
      this.enemy = enemy;
      this.time = time;
      this.who = who;
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
    protected ICollection<EnemyTimeWho> dying = new LinkedList<EnemyTimeWho>(), dead = new LinkedList<EnemyTimeWho>();
    protected ICollection<CollisionTimePair> collisions = new LinkedList<CollisionTimePair>();
    protected ICollection<Bullet> bullets = new LinkedList<Bullet>();
    protected ICollection<Enemy> living = new LinkedList<Enemy>();
    protected Queue<Bullet> usedBullets = new Queue<Bullet>();
    protected Queue<EnemyTimeWho> deathRow = new Queue<EnemyTimeWho>();
    protected Spawner spawner;
    protected TimeSpan lastUpdate;
    protected Music music = new Music();
    protected MainGun[] players = new MainGun[4] { new MainGun(0), new MainGun(1), new MainGun(2), new MainGun(3) };
    protected Tower tower = new Tower();
    protected ICollection<Turret> turrets = new LinkedList<Turret>();

    #region Properties

    public Queue<EnemyTimeWho> DeathRow
    {
      get { return deathRow; }
    }

    public Queue<Bullet> UsedBullets
    {
      get { return usedBullets; }
    }

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

    public ICollection<EnemyTimeWho> Dying
    {
      get { return dying; }
    }

    public ICollection<EnemyTimeWho> Dead
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

    public int NumberOfPlayers
    {
      get
      {
        int n = 0;
        for (int i = 0; i < 4; i++)
        {
          if (players[i].IsActive)
          {
            n++;
          }
        }
        return n;
      }
    }

    public int Progress
    {
      get { return (int) (Music.BarCount * Constants.PROGRESSION_RATE); }
    }

    #endregion

    #region Events

    public event EventHandler<UpdateArgs> Update;
    public event EventHandler<EnemyArgs> NewEnemy;
    public event EventHandler<EnemyArgs> DeadEnemy;
    public event EventHandler<TurretArgs> AddTurret;
    public event EventHandler<TurretArgs> RemoveTurret;
    public event EventHandler<PlayerArgs> AddPlayer;
    public event EventHandler<PlayerArgs> RemovePlayer;

    #endregion

    #region Methods

    public virtual void OnUpdate(GameTime gameTime)
    {
      lastUpdate = gameTime.TotalRealTime;
      Update(this, new UpdateArgs(LastUpdate));
      foreach(Bullet b in bullets)
      {
        if (App.Instance.onScreen(b.Location))
        {
          foreach (Enemy e in living)
          {
            if (e.Collides(b))
            {
              e.Health -= b.Power;
              if (e.Health <= 0)
              {
                deathRow.Enqueue(new EnemyTimeWho(e, LastUpdate, b.PlayerId));
              }

              if ((Effects.Pierce & b.Effects) == 0)
              {
                usedBullets.Enqueue(b);
              }
              if ((Effects.Burn & b.Effects) != 0)
              {
                e.State |= Enemy.States.Burning;
              }
              if ((Effects.Slow & b.Effects) != 0)
              {
                e.State |= Enemy.States.Slowed;
              }
              if ((Effects.Stun & b.Effects) != 0)
              {
                e.State |= Enemy.States.Stunned;
              }
            }
          }
          while (deathRow.Count > 0)
          {
            MakeDying(deathRow.Dequeue());
          }
          b.Move();
        }
        else
        {
          usedBullets.Enqueue(b);
        }
      }
      while (usedBullets.Count > 0)
      {
        bullets.Remove(usedBullets.Dequeue());
      }
      foreach (Enemy e in living)
      {
        if ((e.State & Enemy.States.Stunned) == 0)
        {
          e.Move();
        }
        e.Update();
      }
    }

    public void Spawn(Enemy e)
    {
      Living.Add(e);
      if (NewEnemy != null) NewEnemy(this, new EnemyArgs(e));
    }

    public void MakeDying(EnemyTimeWho etw)
    {
      Living.Remove(etw.enemy);
      //A 'who' of -1 signifies that the enemy was removed by reaching the centre, rather than being destroyed by a player
      if (etw.who > -1)
      {
        dying.Add(etw);
      }
    }

    public void Kill(EnemyTimeWho etw)
    {
      dying.Remove(etw);
      if (DeadEnemy != null) DeadEnemy(this, new EnemyArgs(etw.enemy));
      Players[etw.who].Killed(etw.enemy);
      dead.Add(new EnemyTimeWho(etw.enemy, LastUpdate, etw.who));
    }

    public void Cremate(EnemyTimeWho etw)
    {
      dead.Remove(etw);
    }

    public void CreateTurret(Vector2 location, int owner)
    {
      Turret t = new Turret(location, owner);
      Turrets.Add(t);
      if (AddTurret != null) AddTurret(this, new TurretArgs(t));
    }

    public void DeleteTurret(Turret t)
    {
      if (Turrets.Contains(t))
      {
        Turrets.Remove(t);
        if (RemoveTurret != null) RemoveTurret(this, new TurretArgs(t));
      }
    }

    public bool PlayerJoin(int player)
    {
      if (!Players[player].IsActive)
      {
        Players[player].IsActive = true;
        if (AddPlayer != null) AddPlayer(this, new PlayerArgs(player));
        return true;
      }
      return false;
    }

    public virtual bool PlayerLeave(int player)
    {
      if (Players[player].IsActive)
      {
        Players[player].IsActive = false;
        if (RemovePlayer != null) RemovePlayer(this, new PlayerArgs(player));
        return true;
      }
      return false;
    }

    public virtual void Restart()
    {
      music.Stop();
      UnsubscribeAll();
      spawner = null;
      living.Clear();
      dying.Clear();
      dead.Clear();
      collisions.Clear();
      bullets.Clear();
      usedBullets.Clear();
      deathRow.Clear();
      turrets.Clear();
      players = new MainGun[4] { new MainGun(0), new MainGun(1), new MainGun(2), new MainGun(3) };
      tower = new Tower();

      // Free up memory - we've chucked a lot of stuff away and there shouldn't be much processing going on right now.
      System.GC.Collect();
    }

    private void UnsubscribeAll()
    {
      Update = null;
      NewEnemy = null;
      DeadEnemy = null;
      AddTurret = null;
      RemoveTurret = null;
      AddPlayer = null;
      RemovePlayer = null;
    }

    #endregion
  }
}
