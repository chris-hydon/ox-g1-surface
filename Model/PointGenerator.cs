using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SurfaceTower.Model
{
  public class PointGenerator : IGenerator
  {
    protected Random random = new Random();
    protected BaseModel model = App.Instance.Model;
    // width and height are set to the width and height of the screen
    int width = App.Instance.GraphicsDevice.Viewport.Width;
    int height = App.Instance.GraphicsDevice.Viewport.Height;
    Vector2 pos;
    int difficulty;
    int interval;
    int amount;
    int generated = 0;
    int clickCount = 0;
    bool done = false;

    public bool Done
    {
      get { return done; }
    }

    public PointGenerator(Vector2 pos, int difficulty, int interval, int amount)
    {
      this.pos = pos;
      this.difficulty = difficulty;
      this.interval = interval;
      this.amount = amount;
    }

    public void Generate(int numEnemies)
    {
      clickCount++;
      if (clickCount % interval == 0)
      {
        model.Living.Add(new Enemy(pos, 0f, difficulty, difficulty, (model.Tower.Location - pos) / 5, Microsoft.Xna.Framework.Graphics.Color.BlanchedAlmond));
        generated++;
      }
      if (generated >= amount)
      {
        done = true;
      }
    }    
  }
}
