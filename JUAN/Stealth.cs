//#xxxforcedebug
//C#
using System;
using Assistant;
using System.Collections.Generic;
namespace RazorEnhanced
{
    public class Stealth
    {
        public void Run()
        {
            double stealthValue=0;
            DateTime lastGain = DateTime.Now;

            while (true)
            {
                if (stealthValue != Player.GetSkillValue(SkillName.Stealth.ToString()))
                {
                    stealthValue = Player.GetSkillValue(SkillName.Stealth.ToString());
                    lastGain = DateTime.Now;
                }
                
                 Player.HeadMessage(50, "Last gain " + (int)(DateTime.Now - lastGain).TotalSeconds + "s ago.");   
                
                
                Hide();
                
                Player.PathFindTo(2711,584, 0);
                Misc.Pause(2000);

                Hide();
                Player.PathFindTo(2705, 584, 0);
                Misc.Pause(2000);
                
            }
        }

        public void Hide()
        {
            while (Player.Visible)
            {
                Misc.Pause(500);
                Player.UseSkill(SkillName.Hiding.ToString());
                Misc.Pause(500);
                
            }
        }

      

    }
}