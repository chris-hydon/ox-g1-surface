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
        private ICollection<ImprovMeter> meters = new List<ImprovMeter>();
        private Vector2[] menuPositions = new Vector2[4];

        public MenuManager()
        {
            foreach (MainGun p in App.Instance.Model.Players)
            {
               p.UpgradeReady+= new EventHandler(p_UpgradeReady);
               p.UpgradeDone += new EventHandler(p_UpgradeDone);
               menuPositions[p.PlayerId] = p.Location;
               meters.Add(new ImprovMeter(p.PlayerId));
            }
           
        }

        public void reset()
        {
            foreach (MainGun p in App.Instance.Model.Players)
            {
                p.UpgradeReady += new EventHandler(p_UpgradeReady);
                p.UpgradeDone += new EventHandler(p_UpgradeDone);
            }

        }

        void p_UpgradeDone(object sender, EventArgs e)
        {
            MainGun m = (MainGun)sender;
            IMenu menu;
            menus.TryGetValue(m.PlayerId, out menu);
            if (menu != null)
            {
                menu.Close();
            }
        }

        void  p_UpgradeReady(object sender, EventArgs e)
        {
            MainGun p = (MainGun)sender;
            try
            {
                menus.Add(p.PlayerId, new UGMenu(menuPositions[p.PlayerId], p.PlayerId, p.Upgrades));
            }
            catch (ArgumentException)
            {

            }
        }   

        public void Draw(SpriteBatch sb)
        {
            Queue<KeyValuePair<int, IMenu>> iterator = new Queue<KeyValuePair<int, IMenu>>(menus);
            KeyValuePair<int, IMenu> im;
            while (iterator.Count > 0)
            {
                im = iterator.Dequeue();
                if (im.Value != null)
                {
                    im.Value.Draw(sb);
                    if (im.Value.Finished())
                    {
                        menus.Remove(im.Key);
                    }
                }
            }
            foreach (ImprovMeter imp in meters)
            {
                imp.Draw(sb);
            }
        }


    }
}
