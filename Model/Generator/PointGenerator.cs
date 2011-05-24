using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SurfaceTower.Model.Generator
{
  public class PointGenerator : AbstractGenerator
  {
    protected BaseModel model = App.Instance.Model;
    protected Vector2 pos;
    /// <summary>
    /// A generator for creating enemies at a given point.
    /// </summary>
    /// <param name="pos">Where to spawn the enemies.</param>
    /// <param name="spawnCount">How many times to spawn enemies at the point.</param>
    public PointGenerator(Vector2 pos, int spawnCount) : base(spawnCount)
    {
      this.pos = pos;
      // This should by default spawn only a single enemy at a time.
      GroupSize = 1;
      // MultiplayerAdjustment multiplies the number of enemies per spawn based on the number of players.
      MultiplayerAdjustment = 1;
    }

    public override void Generate()
    {
      if (ShouldGenerate())
      {
        while (HasNextEnemy())
        {
          //Spawn an enemy at the point, moving towards the centre.
          model.Spawn(NextEnemy(pos, (model.Tower.Location - pos) / 5));
        }
      }
    }

    /// <summary>
    /// Determines a point just off screen on the given side.
    /// </summary>
    /// <param name="sideId">The side to choose - numbered clockwise from 0(top).</param>
    /// <param name="offset">Screen offset to use, in pixels.</param>
    /// <returns>A vector on the side chosen.</returns>
    public static Vector2 PointOnSide(int sideId, int offset)
    {
      int width = App.Instance.GraphicsDevice.Viewport.Width;
      int height = App.Instance.GraphicsDevice.Viewport.Height;
      float rn = (float) random.NextDouble();

      switch (sideId)
      {
        case 0:
          return new Vector2(rn * width, -offset);
        case 1:
          return new Vector2(width + offset, rn * height);
        case 2:
          return new Vector2(rn * width, height + offset);
        case 3:
          return new Vector2(-offset, rn * height);
        default:
          throw new ArgumentOutOfRangeException("sideId", "Must be between 0 and 3 inclusive.");
      }
    }
  }
}
