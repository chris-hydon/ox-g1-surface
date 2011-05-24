using System;
using System.Collections.Generic;

using SurfaceTower.Model.EventArguments;
using Microsoft.Xna.Framework;

namespace SurfaceTower.Model.Gun
{
  //Any Gun must store its owner, its shot type (pattern and strength), notify of new shots and bullets, and shoot when prompted.
  public interface IGun
  {
    int PlayerId { get; }
    ShotPatterns Shots { get; set; }
    int Strength { get; set; }

    event EventHandler<ShotArgs> ShotFired;
    event EventHandler<BulletArgs> NewBullet;

    void Shoot();
  }
}
