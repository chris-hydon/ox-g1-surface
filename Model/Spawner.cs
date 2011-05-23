using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using SurfaceTower.Model.Generator;
using SurfaceTower.Model.Gun;

namespace SurfaceTower.Model
{
  public class Spawner
  {
    protected ICollection<IGenerator> generators = new LinkedList<IGenerator>();
    protected BaseModel model = App.Instance.Model;
    protected Random random = new Random();
    protected Queue<IGenerator> finished = new Queue<IGenerator>();
    protected Queue<ICollection<IGenerator>> waves = new Queue<ICollection<IGenerator>>();

    public Spawner()
    {
      model.Music.Click += new EventHandler(OnClick);
      model.Music.Bar += new EventHandler(OnBar);
    }

    #region Methods

    void OnBar(object sender, EventArgs e)
    {
      if (waves.Count == 0)
      {
        DetermineWave();
      }

      ICollection<IGenerator> wave = waves.Dequeue();
      foreach (IGenerator g in wave)
      {
        generators.Add(g);
      }
    }

    void OnClick(object sender, EventArgs e)
    {
      int numEnemies = (int)(1 + model.LastUpdate.TotalMinutes + model.NumberOfPlayers * (int)Math.Round(Constants.MAX_SPAWNING * random.NextDouble()));
      if (model.Living.Count < Constants.MAX_ENEMY_COUNT)
      {
        foreach (IGenerator eg in generators)
        {
          if (eg.Done)
          {
            finished.Enqueue(eg);
          }
          else
          {
            eg.Generate();
          }
        }
        while (finished.Count > 0)
        {
          generators.Remove(finished.Dequeue());
        }
      }
    }

    protected void DetermineWave()
    {
      Console.WriteLine("Progress: " + model.Progress + ", Players: " + model.NumberOfPlayers);
      int progress = model.Progress;
      int style = random.Next(100) + progress;

      if (style < 50 || progress < 2)
      {
        RandomWave(false, false);
      }
      else if (style < 60 || progress < 10)
      {
        RandomWave(true, false);
      }
      else if (style < 70 || progress < 15)
      {
        RandomWave(false, true);
      }
      else
      {
        RandomWave(true, true);
      }
    }

    /// <summary>
    /// Simple algorithm to apply linear increments to difficulty for generator parameters.
    /// </summary>
    /// <param name="step">Progress between each difficulty step.</param>
    /// <param name="max">Maximum difficulty, 0 indicates no cap.</param>
    /// <returns></returns>
    protected int LinearDifficulty(int step, int max)
    {
      return (max == 0) ? model.Progress / step : Math.Min(max, model.Progress / step);
    }

    #endregion

    #region Waves

    /// <summary>
    /// Queues up a wave of enemies whose power and number depend on the time played, coming
    /// from one point per player. The points chosen are usually on the player's own side.
    /// </summary>
    /// <param name="useAllSides">False if only each player's own side should be used to spawn, true if
    /// there should be a chance of using a random side.</param>
    /// <param name="playerSpecifc">If true, each spawn can only be damaged by the player who shares a
    /// colour with it.</param>
    void SimpleWave(bool useAllSides, bool playerSpecifc)
    {
      ICollection<IGenerator> wave = new LinkedList<IGenerator>();
      PointGenerator g;
      foreach (MainGun p in model.Players)
      {
        if (p.IsActive)
        {
          int side = p.PlayerId;
          // If useAllSides, sometimes choose a different side.
          if (useAllSides && random.Next(2) == 0)
          {
            side = random.Next(0, 3);
          }

          g = new PointGenerator(PointGenerator.PointOnSide(side, 20), 3 + LinearDifficulty(10, 12));
          g.EnemyHealth = 1 + LinearDifficulty(5, 0);
          g.EnemySize = 20;
          g.EnemySizeVariance = LinearDifficulty(5, 10);
          g.EnemyType = EnemyType.Regular;
          g.Frequency = model.Music.ClicksPerBeat / 2;
          if (playerSpecifc)
          {
            g.PlayerSpecific = true;
            g.TargetPlayer = p.PlayerId;
          }

          wave.Add(g);
        }
      }

      waves.Enqueue(wave);
    }
    void RandomWave(bool useAllSides, bool playerSpecifc)
    {
      ICollection<IGenerator> wave = new LinkedList<IGenerator>();
      AbstractGenerator g;
      foreach (MainGun p in model.Players)
      {
        if (p.IsActive)
        {
          int side = p.PlayerId;
          // If useAllSides, sometimes choose a different side.
          if (useAllSides && random.Next(2) == 0)
          {
            side = random.Next(0, 3);
          }
          int genType = random.Next(3);

          switch (genType)
          {
            case 0: g = new CircleGenerator(1);
              g.GroupSize = 20;
              g.MultiplayerAdjustment = 1;
              System.Console.WriteLine(0);
              break;
            case 1: g = new PointGenerator(PointGenerator.PointOnSide(side, 20), 3 + LinearDifficulty(10, 12));
              System.Console.WriteLine(1);
              break;
            case 2: g = new SideGenerator(side, 1);
              g.GroupSize = 20;
              g.MultiplayerAdjustment = 1;
              System.Console.WriteLine(2);
              break;
            default : throw new InvalidOperationException();
          }
          int enemyType = random.Next(3);
          switch (enemyType)
          {
            case 0:
              g.EnemyType = EnemyType.Regular;
              break;
            case 1:
              g.EnemyType = EnemyType.Spiral;
              break;
            case 2:
              g.EnemyType = EnemyType.Wave;
              break;
            default: throw new InvalidOperationException();
          }

          g.EnemyHealth = 1 + LinearDifficulty(5, 0);
          g.EnemySize = 20;
          g.EnemySizeVariance = LinearDifficulty(5, 10);
          g.Frequency = model.Music.ClicksPerBeat / 2;
          if (playerSpecifc)
          {
            g.PlayerSpecific = true;
            //g.TargetPlayer = p.PlayerId;
          }

          wave.Add(g);
        }
      }

      waves.Enqueue(wave);
    }

    #endregion
  }
}
