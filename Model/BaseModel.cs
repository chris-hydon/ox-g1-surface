﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

using SurfaceTower.Model.EventArguments;
using SurfaceTower.Model.Gun;

namespace SurfaceTower.Model
{
  #region Structures
  // Stores a triple of an Enemy, a TimeSpan and an int representing a playerID.
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
  #endregion

  public abstract class BaseModel
  {
    protected ICollection<EnemyTimeWho> dying = new LinkedList<EnemyTimeWho>(), dead = new LinkedList<EnemyTimeWho>();
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

    public MainGun[] ActivePlayers
    {
      get
      {
        MainGun[] actives = new MainGun[NumberOfPlayers];
        int index = 0;
        for (int i = 0; i < 4; i++)
        {
          if (players[i].IsActive)
          {
            actives[index] = players[i];
            index++;
          }
        }
        return actives;
      }

    }

    public Spawner Spawner
    {
      get { return spawner; }
    }
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

    public bool FightingBoss
    {
      get
      {
        foreach (Enemy e in Living)
        {
          if (e is BossEnemy)
          {
            return true;
          }
        }
        return false;
      }
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
      if (Tower.Dead) return;

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
                //Enemy e has died - mark it for removal, storing the playerID of its killer and its time of death.
                deathRow.Enqueue(new EnemyTimeWho(e, LastUpdate, b.PlayerId));
              }

              if ((Effects.Pierce & b.Effects) == 0)
              {
                //Bullet b is a non-piercing bullet and has hit a target - mark it for removal.
                usedBullets.Enqueue(b);
              }
              //The following if-statements add the effects of the bullet to the enemy which it hit.
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
          //deathRow is used to store enemies which need to be removed from the living collection.
          //It is used to avoid modifying the collection during the foreach loop. Once the loop is finished
          //deathRow is emptied of its contents, which are dealt with accordingly.
          while (deathRow.Count > 0)
          {
            MakeDying(deathRow.Dequeue());
          }
          b.Move();
        }
        else 
        {
          //Bullet b is not on screen, so mark it for removal.
          usedBullets.Enqueue(b);
        }
      }
      //usedBullets is the Bullet equivalent of deathRow which is for Enemies. See deathRow for details.
      while (usedBullets.Count > 0)
      {
        bullets.Remove(usedBullets.Dequeue());
      }
      foreach (Enemy e in living)
      {
        //Don't move the enemy if it's stunned.
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

    /// <summary>
    /// MakeDying mmoves an enemy from the living collection to the dying collection (unless it hit the centre).
    /// The dying collection is used to store enemies until the AudioEngine is ready to make a sound for them.
    /// </summary>
    public void MakeDying(EnemyTimeWho etw)
    {
      Living.Remove(etw.enemy);
      //A 'who' of -1 signifies that the enemy was removed by reaching the centre, rather than being destroyed by a player
      if (etw.who > -1)
      {
        dying.Add(etw);
      }
    }

    /// <summary>
    /// Kill is called by the AudioEngine and moves an enemy from dying to dead, which allows the VideoEngine
    /// to play its death animation, and also fires the DeadEnemy event.
    /// </summary>
    public void Kill(EnemyTimeWho etw)
    {
      dying.Remove(etw);
      if (DeadEnemy != null) DeadEnemy(this, new EnemyArgs(etw.enemy));
      Players[etw.who].Killed(etw.enemy);
      dead.Add(new EnemyTimeWho(etw.enemy, LastUpdate, etw.who));
    }

    /// <summary>
    /// Cremate removes an enemy from the dead collection, and is called by the VideoEngine once the death
    /// animation is done. The enemy is then no longer referred to and is garbage-collected. 
    /// </summary>
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
        // TODO: Investigate this memory leak.
        t.Active = false;
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
    //Restart resets the BaseModel, ready for a new game to begin.
    public virtual void Restart()
    {
      music.Stop();
      UnsubscribeAll();
      spawner = null;
      living.Clear();
      dying.Clear();
      dead.Clear();
      bullets.Clear();
      usedBullets.Clear();
      deathRow.Clear();
      turrets.Clear();
      players = new MainGun[4] { new MainGun(0), new MainGun(1), new MainGun(2), new MainGun(3) };
      tower = new Tower();

      // Free up memory - we've chucked a lot of stuff away and there shouldn't be much processing going on right now.
      System.GC.Collect();
    }
    //This ensures there are no memory leaks from event listeners listening to the model.
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
