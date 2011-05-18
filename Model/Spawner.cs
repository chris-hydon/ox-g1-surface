using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

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
      generators.Add(new CircleGenerator(random.Next(40)));
      if (random.NextDouble() < 0.75f)
      {
        double angle = Math.PI * random.NextDouble() * 2;
        // width and height are set to the width and height of the screen
        int width = App.Instance.GraphicsDevice.Viewport.Width;
        int height = App.Instance.GraphicsDevice.Viewport.Height;
        int x = width / 2 + (int)(width * Math.Cos(angle));
        int y = height / 2 + (int)(height * Math.Sin(angle));
        generators.Add(new PointGenerator(new Vector2(x,y), random.Next(Constants.LARGEST_ENEMIES), 1 + (int) Math.Round(10 * random.NextDouble()), random.Next(20)));
      }
      if (random.NextDouble() < 0.50f)
      {
        generators.Add(new SideGenerator(random.Next(4)));
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
