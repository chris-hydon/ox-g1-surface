using SurfaceTower.Model.Gun;

namespace SurfaceTower.Model.Upgrades
{
  public class EffectUpgrade : Upgrade
  {
    private Effects newEffects;
    private bool keepExistingEffects;

    public EffectUpgrade(IGun toUpgrade, Effects newEffects, bool keepExistingEffects)
      : base(toUpgrade)
    {
      this.newEffects = newEffects;
      this.keepExistingEffects = keepExistingEffects;
    }

    public override void Apply()
    {
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
      }
    }
  }
}
