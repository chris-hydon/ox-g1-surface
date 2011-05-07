﻿using System;
using System.Collections.Generic;

using SurfaceTower.Model.EventArguments;
using Microsoft.Xna.Framework;

namespace SurfaceTower.Model.Gun
{
  public interface IGun
  {
    int PlayerId { get; }
    ShotPatterns Shots { get; set; }

    event EventHandler<ShotArgs> ShotFired;
    event EventHandler UpgradeReady;

    void Shoot();
  }
}
