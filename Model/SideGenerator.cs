using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SurfaceTower.Model
{
  public class SideGenerator : IGenerator
  {
    bool done;
    BaseModel model = App.Instance.Model;
    protected Random random = new Random();
    Vector2 start, end;
    // width and height are set to the width and height of the screen
    int width = App.Instance.GraphicsDevice.Viewport.Width;
    int height = App.Instance.GraphicsDevice.Viewport.Height;

    public bool Done
    {
      get { return done; }
    }

    public SideGenerator(int side)
    {
      switch(side)
      {
        case 0 :
          start = new Vector2(-100, -100);
          end = new Vector2(width + 100, -100);
          break;
        case 1 :
          start = new Vector2(width + 100, -100);
          end = new Vector2(width + 100, height + 100);
          break;
        case 2 :
          start = new Vector2(-100, height + 100);
          end = new Vector2(width + 100, height + 100);
          break;
        case 3 :
          start = new Vector2(-100, -100);
          end = new Vector2(-100, height + 100);
          break;
        default :
          done = true;
          break;
      }
    }
    public void Generate()
    {
      int size = random.Next(Constants.LARGEST_ENEMIES);
      for (int i = 0; i <= 10; i++)
      {
        Vector2 pos = start + (i*(end-start))/10;
        model.Living.Add(new Enemy(pos, 0f, size, size, (model.Tower.Location - pos) / 5, Microsoft.Xna.Framework.Graphics.Color.SkyBlue));
      }
      done = true;
    }
  }
}
