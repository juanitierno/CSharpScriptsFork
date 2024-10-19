//#xxforcedebug
//C#
using System.Collections.Generic;
using System.Linq;

namespace RazorEnhanced
{
    public class BunnyHealer
    {
        List<int> FriendSerials = new List<int>()
        {
            0x0005A117,
            0x00066270,
            0x0005A112,
            0x0005A110
        };

        public void Run()
        {
            Player.HeadMessage(10, "Bunny Heal Started!");

            while (true)
            {
                if (Player.IsGhost || Player.Mana < 25)
                {
                    Misc.Pause(2000);
                    continue;
                }

                List<Mobile> foundMobiles = (from m in FriendSerials
                                             select Mobiles.FindBySerial(m))
                                              .Where(x => x != null).ToList();

                foreach (Mobile mobile in foundMobiles)
                {
                    if (Player.IsGhost || Player.Mana < 25)
                        break;

                    //skip far away, healthy.
                    if (Player.DistanceTo(mobile) >= 10 || (!mobile.Poisoned && mobile.Hits > mobile.HitsMax * 0.9))
                        continue;

                    if (mobile.Poisoned && mobile.Hits < mobile.HitsMax * 0.9)
                    {
                        Misc.SendMessage("Curing " + mobile.Name, 5);
                        Spells.Cast("Arch Cure", mobile);
                        Misc.Pause(1000);
                    }

                    if (!mobile.Poisoned && mobile.Hits < mobile.HitsMax * 0.9)
                    {
                        Misc.SendMessage("Healing " + mobile.Name, 5);
                        Spells.Cast("Greater Heal", mobile);
                        Misc.Pause(1000);
                    }
                }

                Misc.Pause(1000);
                continue;
            }
        }
    }
}
