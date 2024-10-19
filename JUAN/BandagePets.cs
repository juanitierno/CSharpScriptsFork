//#forcedebug
//C#
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RazorEnhanced
{
    // The class can have any valid class name
    public class BandagePets
    {
        public BandagePets()
        { }

        // This method is the entrypoint and is mandatory
        public void Run()
        {
            Mobiles.Filter petF = new Mobiles.Filter();
            //petF.Friend = 1;
            petF.Notorieties.Add(2);
            petF.Enabled = true;
            petF.RangeMax = 5;

            while (true)
            {
                List<Mobile> pets = Mobiles.ApplyFilter(petF);
                foreach (Mobile m in pets)
                {
                    if (m.Hits < m.HitsMax)
                    {
                        if (Player.DistanceTo(m) > 1)
                            Player.HeadMessage(55, "Get closer!");
                        else
                        {
                            Player.ChatSay("[band");
                            Target.WaitForTarget(1000);
                            Target.TargetExecute(m);
                            Misc.Pause(3500);
                            break;
                        }
                    }
                }
                Misc.Pause(500);
            }


        }



    }
}
