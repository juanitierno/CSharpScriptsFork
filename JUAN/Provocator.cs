//#xxforcedebug
//C#
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using Assistant;

namespace RazorEnhanced
{
    public class Provocator
    {
        public void Run()
        {
            Mobile myPet = FindMyPet(); 
            Random rand = new Random();            

            List<Mobile> enemies = ScanForEnemies();
            if (enemies.Count < 2)
            {
                Player.HeadMessage(15, "Not enough mobs to provoke.");
                return;
            }

            Mobile firstEnemy = enemies[rand.Next(0, enemies.Count)];
            Mobile secondEnemy = enemies.Where(x => x.Serial != firstEnemy.Serial).OrderBy(x => rand.Next()).FirstOrDefault();

            Player.HeadMessage(15, "Provoking "+firstEnemy.Name+ " onto "+secondEnemy.Name);

            Player.UseSkill(SkillName.Provocation.ToString());
            Target.WaitForTarget(1500);
            Target.TargetExecute(firstEnemy);
            Target.WaitForTarget(1500);
            Target.TargetExecute(secondEnemy);
        }

            private List<Mobile> ScanForEnemies()
        {
            Mobiles.Filter mobs = new Mobiles.Filter();
            mobs.Notorieties.Add(3);
            mobs.Notorieties.Add(4);
            mobs.Notorieties.Add(5);
            mobs.RangeMax = 15;
            return Mobiles.ApplyFilter(mobs).ToList();
        }

        private Mobile FindMyPet()
        {
            Mobiles.Filter mobs = new Mobiles.Filter();
            mobs.Notorieties.Add(2);
            mobs.RangeMax = 13;
            return Mobiles.ApplyFilter(mobs).FirstOrDefault();
        }




    }
}
