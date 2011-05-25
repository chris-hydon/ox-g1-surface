using System;

using Microsoft.Surface.Core;
using SurfaceTower.Controller;
using SurfaceTower.Model.Gun;

namespace SurfaceTower.Model.Upgrades
{
  public abstract class Upgrade : ITouchable
  {
    public enum UpgradeType
    {
      Strength,
      Homing,
      TwoShot,
      Spread
    }

    private int playerId;
    private ITouchHandler controller;
    private IGun upgradeTarget;

    #region Properties

    public ITouchHandler Controller
    {
      get { return controller; }
    }

    protected IGun UpgradeTarget
    {
      get { return upgradeTarget; }
    }

    #endregion

    #region Methods

    public Upgrade(IGun upgradeTarget)
    {
      this.upgradeTarget = upgradeTarget;
      playerId = upgradeTarget.PlayerId;
      controller = new UpgradeOption(this, playerId);
    }

    public abstract void Apply();

    public bool InRegion(Contact target)
    {
      throw new System.NotImplementedException();
    }

    public static Upgrade CreateUpgrade(UpgradeType type, IGun gun)
    {
      switch (type)
      {
        case UpgradeType.Homing:
          return new EffectUpgrade(gun, Effects.Homing, true);
        case UpgradeType.Spread:
          return new ShotUpgrade(gun, ShotPatterns.Spread, false);
        case UpgradeType.Strength:
          return new StrengthUpgrade(gun, 2);
        case UpgradeType.TwoShot:
          return new ShotUpgrade(gun, ShotPatterns.TwoShot, false);
        default:
          throw new NotImplementedException();
      }
    }

    #endregion
  }
}
