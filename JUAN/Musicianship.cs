//#forcedebug
//C#
using System;
using Assistant;
using System.Collections.Generic;
using System.Linq;

namespace RazorEnhanced
{
    public class Musicianship
    {
        public void Run()
        {
            while (true)
            {
                Item drum = Player.Backpack.Contains.Where(x => x.Name == "tambourine").FirstOrDefault();
                if (drum == null)
                    break;

                Items.UseItem(drum);
                Misc.Pause(200);
                //Player.UseSkill(SkillName.Peacemaking.ToString());
                //Target.WaitForTarget(2000);
                //Target.Self();             

                Misc.Pause(800);
            }


        
                
           
        }
    }
}