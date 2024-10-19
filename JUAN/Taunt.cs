//C#
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RazorEnhanced
{
    public class Taunt
    {
        public void Run()
        {
            // Configure this to taste!
            Mobiles.Filter mobs = new Mobiles.Filter();
            mobs.Notorieties.Add(3);
            mobs.Notorieties.Add(4);
            mobs.Notorieties.Add(5);
            mobs.Notorieties.Add(6);
            mobs.RangeMax = 13;

            List<Mobile> enemies = Mobiles.ApplyFilter(mobs);
            Mobile closest = null;

            Player.ChatSay("[e stickouttongue");
            foreach (Mobile m in enemies)
            {
                if (Player.DistanceTo(m) <= 1)
                    closest = m;
                Player.Attack(m);
                Misc.Pause(50);
            }


            // This is so the loop does not end trying to melee attack a mob 5 tiles away while surrounded by targets.
            if (closest != null)
            {
                
                Player.Attack(closest);
            }
        }
    }
}
