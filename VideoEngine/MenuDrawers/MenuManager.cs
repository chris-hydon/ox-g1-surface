using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using SurfaceTower.Model.Gun;

namespace SurfaceTower.VideoEngine.MenuDrawers
{ 
    public class MenuManager
    {
        private Dictionary<int, IMenu> menus = new Dictionary<int, IMenu>();

        public MenuManager()
        {
            foreach (MainGun p in App.Instance.Model.Players)
            {
               p.UpgradeReady+= new EventHandler(p_UpgradeReady);
               p.UpgradeDone += new EventHandler(p_UpgradeDone); 
            }
        }

        void p_UpgradeDone(object sender, EventArgs e)
        {
            MainGun m = (MainGun)sender;
            menus.Remove(m.PlayerId);
            //IMenu menu;
            //menus.TryGetValue(m.PlayerId, out menu);
            //menu.Close();
        }

        void  p_UpgradeReady(object sender, EventArgs e)
        {
            MainGun p = (MainGun)sender;
            try
            {
                menus.Add(p.PlayerId, new UGMenu(p.Location, p.PlayerId, p.Upgrades.Keys));
            }
            catch (ArgumentException)
            {

            }
        }   

        public void Draw(SpriteBatch sb)
        {
            foreach (IMenu m in menus.Values)
            {
                if (m != null)
                {
                    m.Draw(sb);
                }
            }
        }


    }
}
