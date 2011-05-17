﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SurfaceTower.Model.Shape
{
  public interface IShape
  {
    Vector2 Origin { get; set; }
    float Width { get; }
    float Height { get; }
    bool Collides(IShape otherShape);
    bool CheckCollides(Circle otherShape);
  }
}