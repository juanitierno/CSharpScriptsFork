//#xxforcedebug
//C#
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using Assistant;

namespace RazorEnhanced
{
    // The class can have any valid class name
    public class TamerSpellweaver
    {
        public void Run()
        {
            Player.HeadMessage(10, "Autobard starting!");
            int petSerial = FindPet().Serial;
            
            if (petSerial <= 0)
            { Player.HeadMessage(10, "Pet not found!"); return; }
            List<int> discordedMobs = new List<int>();
            
            while (true)
            {
                Mobile pet = Mobiles.FindBySerial(petSerial);
                
                Mobiles.Filter mobs = new Mobiles.Filter();
                mobs.Notorieties.Add(3);
                mobs.Notorieties.Add(4);
                mobs.Notorieties.Add(5);
                mobs.Notorieties.Add(6);
                mobs.Enabled = true;
                mobs.RangeMax = 14;
                List<Mobile> enemies = Mobiles.ApplyFilter(mobs);

                //Misc.SendMessage("TamerBard tick! Enemies found: "+enemies.Count.ToString());

                // close to pet and not yet discorded
                List<Mobile> enemiesSurroundingPet = enemies.Where(x => ( x.DistanceTo(pet) <= 1
                                                                        && Player.DistanceTo(x) <= 14
                                                                        && !discordedMobs.Contains(x.Serial))).ToList();
                //if (enemies.Count>0)
                //    Misc.SendMessage("Enemies near pet: "+ enemiesSurroundingPet.Count.ToString());

                Mobile currTarget = enemiesSurroundingPet.OrderBy(x=> x.HitsMax).FirstOrDefault();

                if (currTarget == null)
                {
                    Misc.Pause(1000);
                    continue;
                }
                else 
                {
                    Journal j = new Journal();
                    j.Clear();

                    Player.UseSkill(SkillName.Discordance.ToString());
                    Target.WaitForTarget(1000);
                    Target.TargetExecute(currTarget);

                    int loops = 0;
                    // wait for success to put the mob in the blacklist
                    while (loops < 8 )
                    {
                        loops++;
                        // success
                        if (j.GetLineText("You play jarring music") != "")
                        {
                            discordedMobs.Add(currTarget.Serial);
                            break;
                        }
                        Misc.Pause(250);
                    }
                }

                Misc.Pause(1000);
            }
        }

        private Mobile FindPet()
        {
           Mobiles.Filter mobs = new Mobiles.Filter();
            mobs.Notorieties.Add(2);
            mobs.Enabled = true;
            mobs.RangeMax = 13;
            return Mobiles.ApplyFilter(mobs).FirstOrDefault();
        }




    }
}
