using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using SurfaceTower.Model.EventArguments;
using SurfaceTower.Model.Upgrades;

namespace SurfaceTower.Model.Gun
{
  //Any Gun must store its owner, its shot type (pattern and strength), notify of new shots and bullets, and shoot when prompted.
  public interface IGun
  {
    int PlayerId { get; }
    ShotPatterns Shots { get; set; }
    int Strength { get; set; }
    Dictionary<Upgrade.UpgradeType, Upgrade> Upgrades { get; }

    event EventHandler<ShotArgs> ShotFired;
    event EventHandler UpgradeReady;
    event EventHandler UpgradeDone;
    event EventHandler<BulletArgs> NewBullet;

    void ApplyUpgrade(Upgrade.UpgradeType type);
    void Shoot();
    void ShowMenu(bool show);
  }
}
