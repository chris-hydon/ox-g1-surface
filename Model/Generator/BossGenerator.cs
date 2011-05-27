using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SurfaceTower.Model.Generator
{
  public class BossGenerator : IGenerator
  {
    public enum Boss {Invader}
    bool done;
    Boss target;
    Vector2 pos;

    #region Properties
    public bool Done
    {
      get { return done; }
    }
    #endregion

    #region Methods
    /// <summary>
    /// A generator for creating boss enemies.
    /// </summary>
    /// <param name="target">Which type of boss to spawn.</param>
    /// <param name="pos">Where to spawn the boss.</param>
    public BossGenerator(Boss target, Vector2 pos)
    {
      this.target = target;
      this.pos = pos;
    }
    public void Generate()
    {
      switch (target)
      {
          //If the target is implemented, spawn it and then make done true.
        case Boss.Invader: App.Instance.Model.Spawn(new Invader(pos)); done = true;  break;
          //If the target is not implemented here, make done true to remove the generator.
        default : done = true;
          break;
      }
    }

    #endregion



  }
}
