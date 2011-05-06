using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using SurfaceTower.Model.Shape;

namespace SurfaceTower.Model
{
  public interface ICollidable : IEntity
  {
    bool Collides(ICollidable target);
  }
}
