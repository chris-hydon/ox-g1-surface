using SurfaceTower.Model.Gun;

namespace SurfaceTower.Model.Upgrades
{
  public class StrengthUpgrade : Upgrade
  {
    private int strength;

    public StrengthUpgrade(IGun toUpgrade, int strength)
      : base(toUpgrade)
    {
      this.strength = strength;
    }

    public override void Apply()
    {
      UpgradeTarget.Strength += strength;
    }
  }
}
