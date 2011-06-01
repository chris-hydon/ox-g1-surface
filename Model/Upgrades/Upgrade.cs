using System;
using System.Collections.Generic;

using Microsoft.Surface.Core;
using SurfaceTower.Controller;
using SurfaceTower.Model.Gun;
using SurfaceTower.VideoEngine.MenuDrawers;

namespace SurfaceTower.Model.Upgrades
{
  public abstract class Upgrade : ITouchable
  {
    public enum UpgradeType
    {
      None,
      Homing,
      Spread,
      Strength,
      TwoShot,
      Fast,
      TwoFast,
      Disc,
      InfrequentShortDisc,
      InfrequentDisc,
      HomingDisc,
      DiscSpread,
      Wide,
      InfrequentWide,
      VeryInfrequentWide,
    }

    private int playerId;
    private ITouchHandler controller;
    private IGun upgradeTarget;
    private IMenuItem drawer = null;
    private UpgradeType upgradeType;

    #region Properties

    public ITouchHandler Controller
    {
      get { return controller; }
    }

    public IGun UpgradeTarget
    {
      get { return upgradeTarget; }
    }

    public IMenuItem Drawer
    {
      get { return drawer; }
      set { drawer = value; }
    }

    public UpgradeType Type
    {
      get { return upgradeType; }
    }

    #endregion

    #region Methods

    public Upgrade(IGun upgradeTarget, UpgradeType type)
    {
      this.upgradeTarget = upgradeTarget;
      this.upgradeType = type;
      playerId = upgradeTarget.PlayerId;
      controller = new UpgradeOption(this, playerId);
    }

    public abstract void Apply();

    public bool InRegion(Contact target)
    {
      return (drawer == null) ? false : drawer.IsHit(target);
      //return false;
    }

    public static Upgrade CreateUpgrade(UpgradeType type, IGun gun)
    {
      Upgrade u;
      switch (type)
      {
        case UpgradeType.Disc:
          u = new ShotUpgrade(gun, ShotPatterns.Disc, true, type);
          break;
        case UpgradeType.DiscSpread:
          u = new ShotUpgrade(gun, ShotPatterns.DiscSpread, false, type);
          break;
        case UpgradeType.Fast:
          u = new EffectUpgrade(gun, Effects.DoublePower, true, type);
          break;
        case UpgradeType.Homing:
          u = new EffectUpgrade(gun, Effects.Homing, true, type);
          break;
        case UpgradeType.HomingDisc:
          u = new ShotUpgrade(gun, ShotPatterns.HomingDisc, false, type);
          break;
        case UpgradeType.InfrequentDisc:
          u = new ShotUpgrade(gun, ShotPatterns.InfrequentDisc, true, type);
          break;
        case UpgradeType.InfrequentShortDisc:
          u = new ShotUpgrade(gun, ShotPatterns.InfrequentShortDisc, true, type);
          break;
        case UpgradeType.InfrequentWide:
          u = new ShotUpgrade(gun, ShotPatterns.InfrequentWide, true, type);
          break;
        case UpgradeType.Spread:
          u = new ShotUpgrade(gun, ShotPatterns.Spread, false, type);
          break;
        case UpgradeType.Strength:
          u = new StrengthUpgrade(gun, 10, type);
          break;
        case UpgradeType.TwoFast:
          u = new ShotUpgrade(gun, ShotPatterns.TwoFast, false, type);
          break;
        case UpgradeType.TwoShot:
          u = new ShotUpgrade(gun, ShotPatterns.TwoShot, false, type);
          break;
        case UpgradeType.VeryInfrequentWide:
          u = new ShotUpgrade(gun, ShotPatterns.VeryInfrequentWide, true, type);
          break;
        case UpgradeType.Wide:
          u = new ShotUpgrade(gun, ShotPatterns.Wide, false, type);
          break;
        default:
          throw new NotImplementedException();
      }

      App.Instance.Controller.Touchables.Add(u);
      return u;
    }

    #endregion

    #region Statics

    public static NodeTree<UpgradeType> UpgradeTree()
    {
      NodeTree<UpgradeType> n = new NodeTree<UpgradeType>(UpgradeType.None);
      NodeTree<UpgradeType> n1 = n.AddChild(UpgradeType.Homing);
      NodeTree<UpgradeType> n2 = n.AddChild(UpgradeType.TwoShot);
      NodeTree<UpgradeType> n3 = n.AddChild(UpgradeType.Fast);

      NodeTree<UpgradeType> n11 = n1.AddChild(UpgradeType.Fast);
      NodeTree<UpgradeType> n12 = n1.AddChild(UpgradeType.InfrequentShortDisc);

      NodeTree<UpgradeType> n21 = n2.AddChild(UpgradeType.Fast);
      NodeTree<UpgradeType> n22 = n2.AddChild(UpgradeType.Spread);

      NodeTree<UpgradeType> n31 = n3.AddChild(UpgradeType.Homing);
      NodeTree<UpgradeType> n32 = n3.AddChild(UpgradeType.TwoFast);

      n11.AddChild(UpgradeType.InfrequentWide);
      n11.AddChild(UpgradeType.Disc);

      n12.AddChild(UpgradeType.HomingDisc);
      n12.AddChild(UpgradeType.Wide);

      n21.AddChild(UpgradeType.InfrequentDisc);
      n21.AddChild(UpgradeType.VeryInfrequentWide);

      n22.AddChild(UpgradeType.Wide);
      n22.AddChild(UpgradeType.DiscSpread);

      n31.AddChild(UpgradeType.InfrequentWide);
      n31.AddChild(UpgradeType.Disc);

      n32.AddChild(UpgradeType.InfrequentDisc);
      n32.AddChild(UpgradeType.VeryInfrequentWide);

      return n;
    }

    #endregion
  }

  public class NodeTree<T>
  {
    private T data;
    private LinkedList<NodeTree<T>> children;
    
    public T Data
    {
      get { return data; }
    }

    public LinkedList<NodeTree<T>> Children
    {
      get { return children; }
    }

    public NodeTree(T data)
    {
      this.data = data;
      children = new LinkedList<NodeTree<T>>();
    }

    public NodeTree<T> AddChild(T data)
    {
      NodeTree<T> t = new NodeTree<T>(data);
      children.AddFirst(t);
      return t;
    }
  }
}
