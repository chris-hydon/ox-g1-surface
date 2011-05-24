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
    /// <summary>
    /// A Spawner can add generators and prompt them to generate enemies.
    /// </summary>
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
      if (model.Living.Count < Constants.MAX_ENEMY_COUNT)
      {
        foreach (IGenerator eg in generators)
        {
          if (eg.Done)
          {
            //Generator has expired - mark it for removal.
            finished.Enqueue(eg);
          }
          else
          {
            //Generator has not expired - prompt it to generate its next enemy/enemies.
            eg.Generate();
          }
        }
        //The finished collection stores generators which need to be removed. It is used so that generators
        //are not removed during the foreach loop.
        while (finished.Count > 0)
        {
          generators.Remove(finished.Dequeue());
        }
      }
    }

    protected void DetermineWave()
    {
      int progress = model.Progress;
      //style can determine the style of the next wave.
      int style = random.Next(100) + progress;

      //The next wave depends on the style, though there are caps based on progress to ensure that waves
      //which are too difficult are not encountered too early.
      if (style < 50 || progress < 2)
      {
        CornersWave(EnemyType.Spiral, true);
      }
      else if (style < 60 || progress < 10)
      {
        CornersWave(EnemyType.Spiral, true);
      }
      else if (style < 70 || progress < 15)
      {
        CornersWave(EnemyType.Spiral, true);
      }
      else
      {
        CornersWave(EnemyType.Spiral, true);
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
      foreach (MainGun p in model.ActivePlayers)
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

      waves.Enqueue(wave);
    }
    /// <summary>
    /// Chooses a random generator type and enemy type per player and queues them.
    /// </summary>
    /// <param name="useAllSides">False if only each player's own side should be used to spawn, true if
    /// there should be a chance of using a random side.</param>
    /// <param name="playerSpecifc">If true, each spawn can only be damaged by the player who shares a
    /// colour with it.</param>
    void RandomWave(bool useAllSides, bool playerSpecifc)
    {
      ICollection<IGenerator> wave = new LinkedList<IGenerator>();
      AbstractGenerator g;
      foreach (MainGun p in model.ActivePlayers)
      {
        int side = p.PlayerId;
        // If useAllSides, sometimes choose a different side.
        if (useAllSides && random.Next(2) == 0)
        {
          side = random.Next(0, 3);
        }
        //Choose the type of generator.
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

        //Choose the type of enemy.
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
        }

        wave.Add(g);
      }

      waves.Enqueue(wave);
    }

    /// <summary>
    /// Queues up a wave of enemies per player in the corners of the screen.
    /// </summary>
    /// <param name="enemyType"> The type of enemy to spawn.</param>
    /// <param name="playerSpecifc">If true, each spawn can only be damaged by the player who shares a
    /// colour with it.</param>
    void CornersWave(EnemyType enemyType, bool playerSpecifc)
    {
      float width = App.Instance.GraphicsDevice.Viewport.Width;
      float height = App.Instance.GraphicsDevice.Viewport.Height;

      ICollection<IGenerator> wave = new LinkedList<IGenerator>();
      PointGenerator g;
      Vector2[] corners = {new Vector2(-100, -100), new Vector2(-100, height + 100), new Vector2(width+100, -100), new Vector2(width+100, height+100)};
      shuffle(corners);
      foreach (MainGun p in model.ActivePlayers)
      {
        Vector2 pos = corners[p.PlayerId];
        g = new PointGenerator(pos, 3 + LinearDifficulty(10, 12));
        g.EnemyHealth = 1 + LinearDifficulty(5, 0);
        g.EnemySize = 20;
        g.EnemySizeVariance = LinearDifficulty(5, 10);
        g.EnemyType = enemyType;
        g.Frequency = model.Music.ClicksPerBeat / 2;
        if (playerSpecifc)
        {
          g.PlayerSpecific = true;
          g.TargetPlayer = p.PlayerId;
        }
        wave.Add(g);
      }
      waves.Enqueue(wave);
    }

    //Shuffle randomly reorders the target array.
    void shuffle(Vector2[] target)
    {
      List<Vector2> temp = new List<Vector2>(target);
      for (int index = 0; index < target.Length; index++)
      {
        int r = random.Next(temp.Count);
        target[index] = temp[r];
        temp.RemoveAt(r);
      }


    }
    #endregion
  }
}
