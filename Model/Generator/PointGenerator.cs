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

    public PointGenerator(Vector2 pos, int spawnCount) : base(spawnCount)
    {
      this.pos = pos;
      // This should by default spawn only a single enemy at a time.
      GroupSize = 1;
      MultiplayerAdjustment = 1;
    }

    public override void Generate()
    {
      if (ShouldGenerate())
      {
        while (HasNextEnemy())
        {
          model.Spawn(NextEnemy(pos, 0f, (model.Tower.Location - pos) / 5, Color.BlanchedAlmond));
        }
      }
    }    
  }
}
