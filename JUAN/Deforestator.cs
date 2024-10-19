//#forcedebug
//C#
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows;
using System.Linq;


namespace RazorEnhanced
{
    public class Deforestator
    {
        /*   CONFIG START   */
        int packAnimalSerial = 0x0003DA15; // beetle/horse serial
        int toolSerial = 0x40565AF4; // hatchet serial
        int harvestFieldResolution = 4; // every how many steps it will try to harvest
        int runeBeginningSerial = 0x4036F3E6; // rune marked a few steps away from the "NW corner (start)" below.
        int runeHomeSerial = 0x402CF769; // rune home, within range of the dropoff chest
        int runeToMarkSerial = 0x4036F3E5;  // spare rune that will ve overwritten when unloading.
        int dropOffBoxSerial = 0x40150733;  // serial of the drop off box secured in your home steps probably.

        List<int> harvestedItemIDs = new List<int>() { 0x1BD7, 0x1BDD, 0x318F, 0x3191, 0x2F5F, 0x3190, 0x3199, 0x5738 };
        
        // NW corner (start)
        int startX = 2681;
        int startY = 425;
        // SE corner (finish)
        int finishX = 2745;
        int finishY = 487;

        /*   CONFIG END   */



        // Logs will be deposited here due to weight.
        Item packAnimalBackpack;
        public void Run()
        {
            

            Mobile packie = Mobiles.FindBySerial(packAnimalSerial);
            if (packie == null || packie.Backpack == null)
            {
                Player.HeadMessage(15, "Pack animal not found, or it is not a pack animal.");
                return;
            }
            else
            {
                Items.UseItem(packie.Backpack);
                packAnimalBackpack = packie.Backpack;
            }

            RecallToRune(runeBeginningSerial, "Forest start");

            int direction = 1;
            for (int goY = startY; goY < finishY; goY += 4)
            {
                if (direction == 1) // go
                {
                    for (int goX = startX; goX < finishX; goX += harvestFieldResolution)
                    {
                        WalkTo(goX, goY);
                        Misc.Pause(300);
                        Chophere();
                    }
                }
                else // return
                {
                    for (int goX = finishX; goX > startX; goX -= harvestFieldResolution)
                    {
                        WalkTo(goX, goY);
                        Misc.Pause(300);
                        Chophere();
                    }
                }
                direction *= -1; // invert dir
            }
        }

        public bool Chophere()
        {
            ProcessMaterials();
            Journal j = new Journal();
            j.Clear();
            while (j.GetLineText("There's not enough wood") == "" && j.GetLineText("You can't use an axe") == "")
            {
                if (Player.Weight > Player.MaxWeight * 0.8)
                    ProcessMaterials();

                Target.TargetResource(toolSerial, "wood");
                Misc.Pause(1000);
            }

            ProcessMaterials();
            return true;
        }

        public void ProcessMaterials()
        {
            List<Item> logs = Player.Backpack.Contains.Where(x => x.ItemID == 0x1BDD).ToList();
            foreach (Item item in logs)
            {
                Items.UseItem(toolSerial);
                Target.WaitForTarget(5000);
                Target.TargetExecute(item);
                Misc.Pause(800);
            }

            List<Item> harvested = Player.Backpack.Contains.Where(x => (harvestedItemIDs.Contains(x.ItemID))).ToList();
            foreach (Item item in harvested)
            {
                Misc.SendMessage("Moving " + item.Name + " into pack animal.", 20);
                Items.Move(item, packAnimalBackpack.Serial, -1);
                Misc.Pause(800);
            }

            if (Items.FindBySerial(packAnimalBackpack.Serial).Weight > 1300)
                DoHomeDropoff();
        }

        public void DoHomeDropoff()
        {
            Player.HeadMessage(10, "Marking return rune here...");
            Spells.Cast("Mark");
            Target.WaitForTarget(10000);
            Target.TargetExecute(runeToMarkSerial);
            Misc.Pause(2000);

            RecallToRune(runeHomeSerial, "Home");
            
            Items.UseItem(packAnimalBackpack);
            Misc.Pause(1000);

            foreach (Item i in Items.FindBySerial( packAnimalBackpack.Serial).Contains)
            {
                Player.HeadMessage(10, "Dropping off: " + i.Name + ".");
                Items.Move(i, dropOffBoxSerial, -1);
                Misc.Pause(1000);
            }

            RecallToRune(runeToMarkSerial, "Back to work");
        }

        public void RecallToRune(int serial, string location)
        {
            int x = Player.Position.X;
            int y = Player.Position.Y;
            int attempts = 1;

            while (x == Player.Position.X && y == Player.Position.Y)  // keep trying to recall
            {
                if (attempts > 1)
                    Player.HeadMessage(30, "Attempt Nr: " + attempts.ToString());
                Player.HeadMessage(10, "Recalling: " + location + ".");
                Spells.Cast("Recall");
                Target.WaitForTarget(10000);
                Target.TargetExecute(serial);
                Misc.Pause(3000);
                attempts++;
            }
        }

        public void WalkTo(int x, int y)
        {
            Player.HeadMessage(15, String.Format("Moving to {0}, {1}...", x, y));            
            Player.PathFindTo(x, y, 0);
            int cnt = 0;
            while (cnt < 30 && (Math.Abs(Player.Position.X - x) > 1 || Math.Abs(Player.Position.Y - y) > 1))
            {
                cnt++;
                Misc.Pause(500);
            }
            Misc.Pause(700);
        }


    }
}
