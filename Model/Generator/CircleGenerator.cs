using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SurfaceTower.Model.Generator
{
  class CircleGenerator : AbstractGenerator
  {
    protected BaseModel model = App.Instance.Model;

    /// <summary>
    /// Generates enemies in a circle around the edge of the screen.
    /// </summary>
    /// <param name="groups">Number of groups to spawn before expiring the generator.</param>
    public CircleGenerator(int groups)
      : base(groups)
    {
    }

    public override void Generate()
    {
      if (ShouldGenerate())
      {
        while (HasNextEnemy())
        {
          //enemies spawn on a circle around the centre - angle determines the random place on that circle
          double angle = Math.PI * random.NextDouble() * 2;

          //x and y position of the enemy is on the circle, which is outside of the screen.
          Vector2 location = new Vector2(ScreenWidth / 2 + (int) (ScreenWidth * Math.Cos(angle)), ScreenHeight / 2 + (int) (ScreenHeight * Math.Sin(angle)));

          //spawn an enemy which will move towards the centre (and therefore onto the screen)
          model.Spawn(NextEnemy(location, 0.2f * (model.Tower.Location - location)));
        }
      }
    }
  }
}
