//C#
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Linq;

namespace RazorEnhanced
{
    class ChampSpawn
    {

        public void Run()
        // detect targ, 
        //Find targ name, 
        //Targ HPmax, 
        // use spells or attacts on nearest enemy 
        // use bandage/heal pet if using pet, if not null 
        // detect banges and gives error messege if low
        // grabs weapon/spellbook equips and removes previous to right hand when detects enemy type
        // 
        {

            Player.PathFindTo(1628, 1179, 0);
            Misc.Pause(2000);
        }

        public void Spells()
        { }
    }
}