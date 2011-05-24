using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SurfaceTower.Model.Generator
{
  public class SideGenerator : AbstractGenerator
  {
    BaseModel model = App.Instance.Model;
    Vector2 start, end;

    /// <summary>
    /// Generate enemies at evenly-spaced intervals along one side of the screen.
    /// </summary>
    /// <param name="side">The side to use - sides are numbered clockwise starting at 0(the top).</param>
    /// <param name="groups">Number of groups to spawn before expiring the generator.</param>
    public SideGenerator(int side, int groups) : base(groups)
    {
      switch(side)
      {
          //Start and end points are set on the corners of a rectangle outside of the screen.
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
        for (int i = 0; i < EnemiesPerGroup; i++)
        {
          Vector2 pos = Vector2.Zero;
          // Avoid division by zero errors.
          if (EnemiesPerGroup == 1)
          {
            pos = (start + end) / 2;
          }
          else
          {
            pos = start + (i * (end - start)) / (EnemiesPerGroup - 1);
          }

          model.Spawn(NextEnemy(pos, (model.Tower.Location - pos) / 5));
        }
      }
    }
  }
}
