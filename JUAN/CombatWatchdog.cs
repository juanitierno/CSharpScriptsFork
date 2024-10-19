//#xxforcedebug
//C#
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;

namespace RazorEnhanced
{
    public class CombatWatchdog
    {
        public void Run()
        {
            Player.HeadMessage(10, "Watchdog enabled.");
            while (true)
            {
                Mobiles.Filter mobs = new Mobiles.Filter();
                mobs.Notorieties.Add(3);
                mobs.Notorieties.Add(4);
                mobs.Notorieties.Add(5);
                mobs.Notorieties.Add(6);
                mobs.Enabled = true;
                mobs.RangeMax = 1;
                List<Mobile> enemies = Mobiles.ApplyFilter(mobs);

                if (!Player.HasPrimarySpecial && enemies.Count >= 2)
                {
                    Player.WeaponPrimarySA();
                    Misc.Pause(100);
                }

                if (enemies.Count > 0) // mobs around
                {
                    if (!Player.BuffsExist("Consecrate Weapon"))
                    {
                        Player.ChatSay("[cs consecrateweapon");
                        Misc.Pause(300);
                    }

                    if (!Player.BuffsExist("Divine Fury"))
                    {
                        Player.ChatSay("[cs divinefury");
                        Misc.Pause(300);
                    }

                    Player.Attack(enemies[0]);
                }

                DoPetSupport();


                

                Misc.Pause(300);
            }




        }

        private void DoPetSupport()
        {
            Mobiles.Filter mobs = new Mobiles.Filter();
            mobs.Notorieties.Add(2);
            mobs.Enabled = true;
            mobs.RangeMax = 13;
            List<Mobile> pets = Mobiles.ApplyFilter(mobs);

            pets = pets.Where(x => x.CanRename).ToList();

            foreach (Mobile pet in pets)
            {
                if (pet.Hits > pet.HitsMax * 0.95) // healthy
                    continue;
                else if (Player.DistanceTo(pet) <= 2 && !Player.BuffsExist("Veterinary")) // bandage
                {
                    Player.ChatSay("[band");
                    Target.WaitForTarget(1000);
                    Target.TargetExecute(pet);
                }
                //else if (Player.DistanceTo(pet) > 2 && Player.Mana > Player.ManaMax / 2)
                //{
                //    Player.ChatSay("[cs GreaterHeal");
                //    Target.WaitForTarget(3000);
                //    Target.TargetExecute(pet);

                //}
            }
        }
    }
}
