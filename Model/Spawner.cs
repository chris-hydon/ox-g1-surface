using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using SurfaceTower.Model.Generator;

namespace SurfaceTower.Model
{
  public class Spawner
  {
    protected ICollection<IGenerator> generators = new LinkedList<IGenerator>();
    protected BaseModel model = App.Instance.Model;
    protected Random random = new Random();
    protected Queue<IGenerator> finished = new Queue<IGenerator>();

    public Spawner()
    {
      model.Music.Click += new EventHandler(OnClick);
      model.Music.Bar += new EventHandler(OnBar);
    }

    void OnBar(object sender, EventArgs e)
    {
      AbstractGenerator g = new CircleGenerator(1);
      g.EnemyHealth = 1;
      g.EnemySize = 10;
      g.EnemySizeVariance = 5;
      g.EnemyType = EnemyType.Regular;
      g.MultiplayerAdjustment = 1.7f;
      g.Frequency = 1;
      g.WaveSize = 10;
      generators.Add(g);

      if (random.NextDouble() < 0.75f)
      {
        double angle = Math.PI * random.NextDouble() * 2;
        // width and height are set to the width and height of the screen
        int width = App.Instance.GraphicsDevice.Viewport.Width;
        int height = App.Instance.GraphicsDevice.Viewport.Height;
        int x = width / 2 + (int)(width * Math.Cos(angle));
        int y = height / 2 + (int)(height * Math.Sin(angle));

        g = new PointGenerator(new Vector2(x, y), random.Next(20));
        g.EnemyHealth = 1;
        g.EnemySize = 20;
        g.EnemySizeVariance = 0;
        g.EnemyType = EnemyType.Wave;
        g.Frequency = model.Music.ClicksPerBeat / 2;
        generators.Add(g);
      }
      if (random.NextDouble() < 0.50f)
      {
        g = new SideGenerator(random.Next(4), 1);
        g.EnemyHealth = 1;
        g.EnemySize = 20;
        g.EnemySizeVariance = 5;
        g.EnemyType = EnemyType.Regular;
        g.MultiplayerAdjustment = 1.7f;
        g.Frequency = 1;
        g.WaveSize = 10;
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

  }
}
