using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using SurfaceTower.Model.Gun;
using SurfaceTower.Model.Upgrades;

namespace SurfaceTower.VideoEngine.MenuDrawers

{
    
    /// <summary>
    /// Creates a line upgrade menu.
    /// </summary>
    public class UGMenu : LineMenu
    {
        //The icons for menu choices.
        private static Texture2D homingTex = App.Instance.Content.Load<Texture2D>("menu");
        private static Texture2D spreadTex = App.Instance.Content.Load<Texture2D>("menu");
        private static Texture2D strengthTex = App.Instance.Content.Load<Texture2D>("menu");
        private static Texture2D twoshotTex = App.Instance.Content.Load<Texture2D>("menu");
        private static Dictionary<Upgrade.UpgradeType, Texture2D> upgradeTextures = new Dictionary<Upgrade.UpgradeType, Texture2D>()
        {
            {Upgrade.UpgradeType.Homing, homingTex},{Upgrade.UpgradeType.Spread, spreadTex}, {Upgrade.UpgradeType.Strength, strengthTex}, {Upgrade.UpgradeType.TwoShot, twoshotTex}
        };

        public UGMenu(Vector2 position, int playerID, ICollection<Upgrade.UpgradeType> availableUpgrades) : base(position, playerID)
        {
            numberofitems = availableUpgrades.Count;
            offsets = new Vector2[numberofitems];
            items = new MenuItem[numberofitems];
            int i = 0;
            foreach (Upgrade.UpgradeType upgrade in availableUpgrades)
            {
                Texture2D t;
                upgradeTextures.TryGetValue(upgrade,out t);
                items[i] = new MenuItem(t);
                i++;
            }
            singleItemWidth = homingTex.Width;
        }
    }
}
