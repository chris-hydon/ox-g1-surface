using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SurfaceTower.Model.Generator
{
  public class BossGenerator : IGenerator
  {
    public enum Boss {}
    bool done;
    Boss target;
    Vector2 pos;

    public bool Done
    {
      get { return done; }
    }
    public BossGenerator(Boss target, Vector2 pos)
    {
      this.target = target;
      this.pos = pos;
    }
    public void Generate()
    {
      switch (target)
      {
        default : done = true;
          break;
      }
    }



  }
}
