//#xxforcedebug
//C#
using System.Collections.Generic;
using System.Linq;

namespace RazorEnhanced
{
    public class HealPetMagery
    {

        public void Run()
        {
            Player.HeadMessage(10, "Mageheal Started!");

            while (true)
            {
                Mobiles.Filter petF = new Mobiles.Filter();
                petF.Notorieties.Add(2);
                petF.RangeMax = 9;

                List<Mobile> pets = Mobiles.ApplyFilter(petF);
                pets = (from x in pets where x.CanRename select x).ToList();

                foreach (Mobile m in pets)
                {
                    if (Player.IsGhost || Player.Mana < 25)
                        break;

                    //skip far away, healthy.
                    if (Player.DistanceTo(m) >= 10 || (!m.Poisoned && m.Hits > m.HitsMax * 0.9))
                        continue;

                    if (m.Poisoned && m.Hits < m.HitsMax * 0.8)
                    {
                        Misc.SendMessage("Curing " + m.Name, 5);
                        Spells.Cast("Arch Cure", m);
                        Misc.Pause(1400);
                    }

                    if (!m.Poisoned && m.Hits < m.HitsMax * 0.8)
                    {
                        Misc.SendMessage("Healing " + m.Name, 5);
                        Spells.Cast("Greater Heal", m);
                        Misc.Pause(1400);
                    }
                }
                Misc.Pause(500);
            }
        }
    }
}
