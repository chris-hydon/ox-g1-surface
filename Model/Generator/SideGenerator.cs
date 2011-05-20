using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SurfaceTower.Model.Generator
{
  public class SideGenerator : AbstractGenerator
  {
    BaseModel model = App.Instance.Model;
    Vector2 start, end;

    public SideGenerator(int side, int waveCount) : base(waveCount)
    {
      switch(side)
      {
        case 0 :
          start = new Vector2(-100, -100);
          end = new Vector2(ScreenWidth + 100, -100);
          break;
        case 1 :
          start = new Vector2(ScreenWidth + 100, -100);
          end = new Vector2(ScreenWidth + 100, ScreenHeight + 100);
          break;
        case 2 :
          start = new Vector2(-100, ScreenHeight + 100);
          end = new Vector2(ScreenWidth + 100, ScreenHeight + 100);
          break;
        case 3 :
          start = new Vector2(-100, -100);
          end = new Vector2(-100, ScreenHeight + 100);
          break;
        default :
          throw new ArgumentOutOfRangeException("side", "Must be between 0 and 3 inclusive.");
      }
    }
    public override void Generate()
    {
      if (ShouldGenerate())
      {
        for (int i = 0; i < EnemiesPerWave; i++)
        {
          Vector2 pos = Vector2.Zero;
          // Avoid division by zero errors.
          if (EnemiesPerWave == 1)
          {
            pos = (start + end) / 2;
          }
          else
          {
            pos = start + (i * (end - start)) / (EnemiesPerWave - 1);
          }

          model.Spawn(NextEnemy(pos, 0f, (model.Tower.Location - pos) / 5, Color.SkyBlue));
        }
      }
    }
  }
}
