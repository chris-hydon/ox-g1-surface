using System;
using System.Collections.Generic;

using SurfaceTower.Model.EventArguments;
using Microsoft.Xna.Framework;

namespace SurfaceTower.Model.Gun
{
  public interface IGun
  {
    int PlayerId { get; }
    ShotPatterns Shots { get; set; }
    int Strength { get; set; }

    event EventHandler<ShotArgs> ShotFired;
    event EventHandler UpgradeReady;
    event EventHandler<BulletArgs> NewBullet;

    void Shoot();
  }
}
