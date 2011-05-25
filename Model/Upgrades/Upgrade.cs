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
      Homing,
      Spread,
      Strength,
      TwoShot,
    }

    private int playerId;
    private ITouchHandler controller;
    private IGun upgradeTarget;

    #region Properties

    public ITouchHandler Controller
    {
      get { return controller; }
    }

    public IGun UpgradeTarget
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
        return false;
    }

    public static Upgrade CreateUpgrade(UpgradeType type, IGun gun)
    {
      Upgrade u;
      switch (type)
      {
        case UpgradeType.Homing:
          u = new EffectUpgrade(gun, Effects.Homing, true);
          break;
        case UpgradeType.Spread:
          u = new ShotUpgrade(gun, ShotPatterns.Spread, false);
          break;
        case UpgradeType.Strength:
          u = new StrengthUpgrade(gun, 2);
          break;
        case UpgradeType.TwoShot:
          u = new ShotUpgrade(gun, ShotPatterns.TwoShot, false);
          break;
        default:
          throw new NotImplementedException();
      }

      App.Instance.Controller.Touchables.Add(u);
      return u;
    }

    #endregion
  }
}
