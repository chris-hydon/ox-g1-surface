using SurfaceTower.Model.Gun;

namespace SurfaceTower.Model.Upgrades
{
  public class EffectUpgrade : Upgrade
  {
    private Effects newEffects;
    private bool keepExistingEffects;

    public EffectUpgrade(IGun toUpgrade, Effects newEffects, bool keepExistingEffects, UpgradeType type)
      : base(toUpgrade, type)
    {
      this.newEffects = newEffects;
      this.keepExistingEffects = keepExistingEffects;
    }

    public override void Apply()
    {
      ShotPatterns upgraded = new ShotPatterns();
      foreach (ShotPattern shot in UpgradeTarget.Shots)
      {
        if (keepExistingEffects)
        {
          shot.Effects |= newEffects;
        }
        else
        {
          shot.Effects = newEffects;
        }
        upgraded = upgraded.Add(shot);
      }
      UpgradeTarget.Shots = upgraded;
      UpgradeTarget.ApplyUpgrade(Type);
    }
  }
}
