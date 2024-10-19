//#forcedebug
//C#
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;


namespace RazorEnhanced
{
    public class MineSpot
    {
        bool wooHooEnabled = true;
        int amntPicksToUse = 10;
        int packAnimalSerial = 0x0003DA15;
        int fireBeetleSerial = 0x000055CE;
        int toolBagsSerial = 0x4015CF48; // Place tinkers tools and about 50-100 iron ingots in here.

        // Ore ids to try to smelt with fire beetle (diff size piles).
        List<int> oreItemIDs = new List<int>() { 0x19B7, 0x19B8, 0x19B9, 0x19BA };

        // IDs of items to move to beetle (ingots, gems, jade, etc).
        List<int> miningProductsItemIDs = new List<int>() { 0x1BF2, 0x3193, 0x5732, 0x0F28, 0x3195, 0x3197, 0x3194, 0x3192, 0x3198, 0x1779, 0x0DF8, 0x1726 };

        public void Run()
        {
            if (Mobiles.FindBySerial(packAnimalSerial) == null)
                Player.HeadMessage(5, "Can't find pack animal.");
            else if (Mobiles.FindBySerial(fireBeetleSerial) == null)
                Player.HeadMessage(5, "Can't find fire beetle.");
            else if (Items.FindBySerial(toolBagsSerial) == null)
                Player.HeadMessage(5, "Can't find tools bag.");
            else if (Items.FindBySerial(toolBagsSerial).Contains.Where(x => x.ItemID == 0x1EB8).Count() ==0)
                Player.HeadMessage(5, "Can't find tinker tools bag (you need the oblong crafted one, not the purchased 'loose tools' one).");
            else
            {
                MineHere();
            }
        }

        public bool MineHere()
        {
            if (DateTime.Now.Second == 4)
                Player.ChatSay("[e fart");


            Items.UseItem(toolBagsSerial);
            Misc.Pause(200);
            Items.UseItem(Mobiles.FindBySerial(packAnimalSerial).Backpack.Serial);
            Misc.Pause(200);

            ProcessHarvested();

            DoMineLoopCoords(-1, -1);
            DoMineLoopCoords(-1, 1);
            DoMineLoopCoords(1, 1);
            DoMineLoopCoords(1, -1);

            ProcessHarvested();
            Player.HeadMessage(20, "Done here!");
            return true;
        }

        public void DoMineLoopCoords(int xOffset, int yOffset)
        {
            Statics.TileInfo tile = Statics.GetStaticsTileInfo(Player.Position.X + xOffset, Player.Position.Y + yOffset, Player.Map).FirstOrDefault();
            if (tile == null)
            {
                Misc.SendMessage("Invalid tile!");
                return;
            
            }

            Journal j = new Journal();
            j.Clear();
            while (j.GetLineText("There is no metal here to mine.") == "" && j.GetLineText("You can't mine there") == "")
            {
                if (Player.Weight > Player.MaxWeight * 0.5)
                    ProcessHarvested();

                FindOrCraftPickInToolsBag();

                List<Item> picks = Items.FindBySerial(toolBagsSerial).Contains
                                                                    .Where(x => (x.ItemID == 0x0E86 && x.Hue == 0) 
                                                                             || (x.ItemID == 0x0F39 && x.Hue == 0))
                                                                    .Take(amntPicksToUse).ToList();
                foreach (Item p in picks)
                {
                    Items.UseItem(p);
                    Target.WaitForTarget(1000);
                    Target.TargetExecute(Player.Position.X + xOffset, Player.Position.Y + yOffset, Player.Position.Z, tile.StaticID);

                    if (j.GetLineText("There is no metal here to mine.") != "" || j.GetLineText("You can't mine there") != "")
                        break;
                    //Target.TargetResource(p, "ore");
                    //Misc.Pause(75);
                }
                Misc.Pause(1000);     
            }
            Misc.Pause(500);
        }

        public Item FindOrCraftPickInToolsBag()
        {    
            List<Item> picks = Items.FindBySerial(toolBagsSerial).Contains.Where(x => x.ItemID == 0x0E86 || x.ItemID == 0x0F39).ToList();
            if (picks.Count >= amntPicksToUse)
                return picks[0];

            // if less than 3 picks, craft as needed
            Player.HeadMessage(5, "Low on picks, crafting...");
            List<Item> tinkersTools = Items.FindBySerial(toolBagsSerial).Contains.Where(x => x.ItemID == 0x1EB8).ToList();
            if (tinkersTools.Count == 0)
            {
                Player.HeadMessage(5, "Can't find tinkers tools in my tool bag.");
                return null;
            }
            else if (tinkersTools.Count < 3)
            {
                Player.HeadMessage(5, "Low on tinker's tools, crafting...");
                Items.UseItem(tinkersTools[0]);
                Gumps.WaitForGump(949095101, 3000); // tinkers craft menu
                Gumps.SendAction(949095101, 15); // tools sub-menu
                Gumps.WaitForGump(949095101, 3000); // tinkers craft menu
                Gumps.SendAction(949095101, 23); // Craft tinkers tools
                Gumps.WaitForGump(949095101, 3000); // tinkers craft menu
                Gumps.SendAction(949095101, 0); // close menu
                Misc.Pause(1000);
            }

            // Craft pick
            tinkersTools = Items.FindBySerial(toolBagsSerial).Contains.Where(x => x.ItemID == 0x1EB8).ToList(); // refresh the list
            Items.UseItem(tinkersTools[0]);
            Gumps.WaitForGump(949095101, 3000); // tinkers craft menu
            Gumps.SendAction(949095101, 15); // tools sub-menu
            Gumps.WaitForGump(949095101, 3000); // tinkers craft menu
            Gumps.SendAction(949095101, 72);  // Craft pickaxe (114) or shovel (72)
            Gumps.WaitForGump(949095101, 3000); // tinkers craft menu
            Gumps.SendAction(949095101, 0); // close menu
            Misc.Pause(1000);

            return Items.FindByID(0x0E86, 0, toolBagsSerial);
        }

        public void ProcessHarvested()
        {
            SmeltOre();
            MoveHarvestedToPackie();
        }

        public void SmeltOre()
        {
            if (Player.Backpack.Contains.Where(x => (oreItemIDs.Contains(x.ItemID) && x.Amount >= 2)).FirstOrDefault() == null) // no ore to smelt
                return;

            Player.HeadMessage(15, "Smelting ore...");
            while (true) //cycle
            {
                Item ore = Player.Backpack.Contains.Where(x => (oreItemIDs.Contains(x.ItemID) && x.Amount >= 2)).FirstOrDefault();
                if (ore == null) break;
                Items.UseItem(ore);
                if (Target.WaitForTarget(1000))
                {
                    Target.TargetExecute(fireBeetleSerial);
                    Misc.Pause(300);
                }
            }
        }

        public void MoveHarvestedToPackie()
        {       
            List<Item> list = Player.Backpack.Contains.Where(x => miningProductsItemIDs.Contains(x.ItemID)).ToList();

            if (wooHooEnabled && list.Where(x=> (x.ItemID == 0x1726 || x.ItemID == 0x0DF8)).ToList().Count > 0 ) // found jade
                Player.ChatSay("[e woohoo");

            if (list.Count > 0)
                Player.HeadMessage(20, "Moving materials...");
            
            foreach (Item item in list)
            {
                Items.Move(item, Mobiles.FindBySerial(packAnimalSerial).Backpack.Serial, -1);
                Misc.Pause(800);
            }
        }
    }
}
