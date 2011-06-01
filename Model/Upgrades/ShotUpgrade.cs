using SurfaceTower.Model.Gun;

namespace SurfaceTower.Model.Upgrades
{
  public class ShotUpgrade : Upgrade
  {
    private ShotPatterns newShotPatterns;
    private bool keepExistingShots;

    public ShotUpgrade(IGun toUpgrade, ShotPatterns newShotPatterns, bool keepExistingShots, UpgradeType type)
      : base(toUpgrade, type)
    {
      this.newShotPatterns = newShotPatterns;
      this.keepExistingShots = keepExistingShots;
    }

    public override void Apply()
    {
      if (keepExistingShots)
      {
        foreach (ShotPattern shot in newShotPatterns)
        {
          UpgradeTarget.Shots = UpgradeTarget.Shots.Add(shot);
        }
      }
      else
      {
        UpgradeTarget.Shots = newShotPatterns;
      }
      UpgradeTarget.ApplyUpgrade(Type);
    }
  }
}
