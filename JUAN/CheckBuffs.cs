//#forcedebug
//C#
using System;
using Assistant;
using System.Collections.Generic;
using System.Linq;

namespace RazorEnhanced
{
    public class CheckBuffs
    {
        public void Run()
        {
            List<Item> items = Items.FindBySerial(0x4015CF48).Contains.Where(x => (x.ItemID == 0x0E86 && x.Hue == 0)).ToList();

            int xx = 0;
            foreach (Item i in items)
            {
                if (xx > 6) break;
                Target.TargetResource(i, "ore");
                Misc.Pause(100);
                xx++;
            }
                
                
        }
    }
}