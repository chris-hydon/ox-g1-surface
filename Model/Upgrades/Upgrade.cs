using System;

using Microsoft.Surface.Core;
using SurfaceTower.Controller;
using SurfaceTower.Model.Gun;

namespace SurfaceTower.Model.Upgrades
{
  public abstract class Upgrade : ITouchable
  {
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
      this.playerId = upgradeTarget.PlayerId;
      controller = new UpgradeOption(this, upgradeTarget.PlayerId);
    }

    public abstract void Apply();

    public bool InRegion(Contact target)
    {
      throw new System.NotImplementedException();
    }

    #endregion
  }
}
