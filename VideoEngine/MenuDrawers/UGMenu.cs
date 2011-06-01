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
        private static Texture2D discTex = App.Instance.Content.Load<Texture2D>("disc");
        private static Texture2D fastTex = App.Instance.Content.Load<Texture2D>("fast");
        private static Texture2D homingTex = App.Instance.Content.Load<Texture2D>("homing");
        private static Texture2D spreadTex = App.Instance.Content.Load<Texture2D>("trishot");
        private static Texture2D spreadDiscTex = App.Instance.Content.Load<Texture2D>("discspread");
        private static Texture2D strengthTex = App.Instance.Content.Load<Texture2D>("powerup");
        private static Texture2D twoshotTex = App.Instance.Content.Load<Texture2D>("twoshot");
        private static Texture2D wideTex = App.Instance.Content.Load<Texture2D>("wide");
        private static Dictionary<Upgrade.UpgradeType, Texture2D> upgradeTextures = new Dictionary<Upgrade.UpgradeType, Texture2D>()
        {
            {Upgrade.UpgradeType.Disc, discTex},
            {Upgrade.UpgradeType.DiscSpread, spreadDiscTex},
            {Upgrade.UpgradeType.Fast, fastTex},
            {Upgrade.UpgradeType.Homing, homingTex},
            {Upgrade.UpgradeType.HomingDisc, discTex},
            {Upgrade.UpgradeType.InfrequentDisc, discTex},
            {Upgrade.UpgradeType.InfrequentShortDisc, discTex},
            {Upgrade.UpgradeType.InfrequentWide, wideTex},
            {Upgrade.UpgradeType.Spread, spreadTex},
            {Upgrade.UpgradeType.Strength, strengthTex},
            {Upgrade.UpgradeType.TwoFast, twoshotTex},
            {Upgrade.UpgradeType.TwoShot, twoshotTex},
            {Upgrade.UpgradeType.VeryInfrequentWide, wideTex},
            {Upgrade.UpgradeType.Wide, wideTex}
        };

        public UGMenu(Vector2 position, int playerID, Dictionary<Upgrade.UpgradeType, Upgrade> availableUpgrades) : base(position, playerID)
        {
            numberofitems = availableUpgrades.Count;
            offsets = new Vector2[numberofitems];
            items = new MenuItem[numberofitems];
            int i = 0;
            foreach (KeyValuePair<Upgrade.UpgradeType, Upgrade> upgrade in availableUpgrades)
            {
              
                Texture2D t;
                upgradeTextures.TryGetValue(upgrade.Key,out t);
                MenuItem men = new MenuItem(t, position);
                upgrade.Value.Drawer = men;
                items[i] = men;
                i++;
            }
            singleItemWidth = homingTex.Width;
        }
    }
}
