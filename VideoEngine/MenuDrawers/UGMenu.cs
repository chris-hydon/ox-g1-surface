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
        private static Dictionary<Upgrade.UpgradeType, Texture2D> upgradeTextures = new Dictionary<Upgrade.UpgradeType, Texture2D>()
        {

        };

        public UGMenu(Vector2 position, int playerID) : base(position, playerID){
        
            Texture2D tex = App.Instance.Content.Load<Texture2D>("menu");
            for (int i = 0; i < offsets.Count<Vector2>(); i++)
            {
                items[i] = new MenuItem(tex);
            }
            singleItemWidth = tex.Width;
        }
    }
}
