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
    public class TamerBard
    {
        List<string> mobNamesToHonor = new List<string>() 
            { 
                "ancient wyrm",
                "shadow wyrm"
            };

        public void Run()
        {
            
            Player.HeadMessage(10, "Autobard starting!");
            Mobile myPet = FindMyPet();
            if (myPet == null)
            { 
                Player.HeadMessage(10, "Pet not found!"); 
                return;
            }

            int petSerial = myPet.Serial;   
            
            List<int> discordedMobs = new List<int>();

            while (true)
            {
                Journal jx = new Journal();
                jx.Clear();
                Mobile pet = Mobiles.FindBySerial(petSerial);
                if (pet == null)
                {
                    Misc.Pause(1000);
                    continue;
                }
                
                Mobiles.Filter mobs = new Mobiles.Filter();
                mobs.Notorieties.Add(3);
                mobs.Notorieties.Add(4);
                mobs.Notorieties.Add(5);
                mobs.Notorieties.Add(6);
                mobs.Enabled = true;
                mobs.RangeMax = 14;
                List<Mobile> enemies = Mobiles.ApplyFilter(mobs);

                // Try honor
                if (!Player.BuffsExist("Honored"))
                {
                    Mobile closeButNotTouchingPet = enemies.Where(x => x.Hits == x.HitsMax
                                                                    && Player.DistanceTo(x) < 13
                                                                    && ShouldHonorByName(x.Name)).FirstOrDefault();
                    if (closeButNotTouchingPet != null)
                    {
                        Player.InvokeVirtue("Honor");
                        Target.WaitForTarget(2000);
                        Target.TargetExecute(closeButNotTouchingPet);
                        Misc.Pause(200);
                    }
                }


                if (enemies.Count > 10) // probably at a champ
                {
                    Mobile closest = enemies.OrderBy(x => Player.DistanceTo(x)).FirstOrDefault();
                    if (closest != null)
                        Player.Attack(closest);
                    Misc.Pause(200);
                }


                List<Mobile> enemiesSurroundingPet = enemies.Where(x => ( x.DistanceTo(pet) <= 1
                                                                        && Player.DistanceTo(x) <= 14
                                                                        && !discordedMobs.Contains(x.Serial))).ToList();

                Mobile currTarget = enemiesSurroundingPet.OrderBy(x=> x.HitsMax).FirstOrDefault();

                if (currTarget == null || enemies.Count > 10)  // if at a champ, don't bother discording
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
                Misc.Pause(300);
            }
        }

        private bool ShouldHonorByName(string name)
        {
            return mobNamesToHonor.Where(x=> name.Contains(x)).Count() > 0;
        }


        private Mobile FindMyPet()
        {
           Mobiles.Filter mobs = new Mobiles.Filter();
            mobs.Notorieties.Add(2);
            mobs.Enabled = true;
            mobs.RangeMax = 13;
            return Mobiles.ApplyFilter(mobs).FirstOrDefault();
        }




    }
}
