//#forcedebug
//C#
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;


namespace RazorEnhanced
{
    public class MinocMiner
    {
        int amntPicksToUse = 10;
        int packAnimalSerial = 0x0003DA15;
        int fireBeetleSerial = 0x000055CE;
        int toolBagsSerial = 0x4015CF48; // Place tinkers tools and about 50-100 iron ingots in here.

        // Path to walk, each coord is a point where the script will try to mine.
        List<PointToWalk> locs = new List<PointToWalk>()
        {
            new PointToWalk(2557,498,0),new PointToWalk(2558,493,0),new PointToWalk(2559,487,0),
            new PointToWalk(2562,482,0),new PointToWalk(2565,477,0),new PointToWalk(2571,474,0),
            new PointToWalk(2577,475,0),new PointToWalk(2580,480,0),new PointToWalk(2577,486,0),
            new PointToWalk(2572,488,0),new PointToWalk(2566,490,0),new PointToWalk(2562,496,0),
            new PointToWalk(2561,499,0),new PointToWalk(2565,502,0),new PointToWalk(2572,507,9),
            new PointToWalk(2578,507,6),new PointToWalk(2584,505,1),new PointToWalk(2590,505,3),
            new PointToWalk(2596,503,0),new PointToWalk(2603,501,0),new PointToWalk(2607,497,10),
            new PointToWalk(2607,492,20),new PointToWalk(2610,486,20),new PointToWalk(2610,481,20),
            new PointToWalk(2611,474,20),new PointToWalk(2605,485,20),new PointToWalk(2599,488,20),
            new PointToWalk(2594,494,20),new PointToWalk(2586,499,20),new PointToWalk(2577,498,26),
            new PointToWalk(2576,490,40),new PointToWalk(2583,489,40),new PointToWalk(2589,489,40),
            new PointToWalk(2595,486,40),new PointToWalk(2600,479,40),new PointToWalk(2604,474,40),
            new PointToWalk(2609,467,40),new PointToWalk(2606,464,45),new PointToWalk(2601,464,60),
            new PointToWalk(2597,470,60),new PointToWalk(2594,475,60),new PointToWalk(2587,480,60),
            new PointToWalk(2581,484,60),new PointToWalk(2575,487,60)
        };

        // Ore ids to try to smelt with fire beetle (diff size piles).
        List<int> oreItemIDs = new List<int>() { 0x19B7, 0x19B8, 0x19B9, 0x19BA };

        // IDs of items to move to beetle.
        List<int> miningProductsItemIDs = new List<int>() { 0x1BF2, 0x3193, 0x5732, 0x0F28, 0x3195, 0x3197, 0x3194, 0x3195, 0x3192, 0x3198, 0x1779, 0x0DF8, 0x1726 };

        // serial of a rune to the beginning of the path, to recall to at the beginning.
        int runeTrammelPathStartSerial = 0x4036F3E7;
        int runeFeluccaPathStartSerial = 0x40453516;




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
                while (Mobiles.FindBySerial(packAnimalSerial).Backpack.Weight<1500)
                {

 // Fel minoc
                    RecallToRune(runeFeluccaPathStartSerial, "FELUCCA Mine Start");
                    foreach (PointToWalk pw in locs)
                    {
                        WalkTo(pw);
                        Misc.Pause(1000);
                        if (!MineHere())
                            return;
                    }

                    // Trammel minoc
                    RecallToRune(runeTrammelPathStartSerial, "TRAMMEL Mine Start");
                    foreach (PointToWalk pw in locs)
                    {
                        WalkTo(pw);
                        Misc.Pause(1000);
                        if (!MineHere())
                            return;
                    }

                   
                }
            }
        }

        public bool MineHere()
        {
            Items.UseItem(toolBagsSerial);
            Misc.Pause(200);
            Items.UseItem(Mobiles.FindBySerial(packAnimalSerial).Backpack.Serial);
            Misc.Pause(200);

            ProcessHarvested();
            Journal j = new Journal();
            j.Clear();
            while (j.GetLineText("There is no metal here to mine.") == "" && j.GetLineText("You can't mine there") == "")
            {
                if (Player.Weight > Player.MaxWeight * 0.5)
                    ProcessHarvested();

                FindOrCraftPickInToolsBag();

                List<Item> picks = Items.FindBySerial(toolBagsSerial).Contains.Where(x => (x.ItemID == 0x0E86 && x.Hue == 0)).Take(amntPicksToUse).ToList();
                foreach (Item p in picks)
                {                    
                    Target.TargetResource(p, "ore");
                    Misc.Pause(75);
                }
                Misc.Pause(1000);     
            }
            Misc.Pause(500);

            ProcessHarvested();
            Player.HeadMessage(20, "Done here");
            return true;
        }

        public Item FindOrCraftPickInToolsBag()
        {
            List<Item> picks = Items.FindBySerial(toolBagsSerial).Contains.Where(x => x.ItemID == 0x0E86).ToList();
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
            Gumps.SendAction(949095101, 114); // Craft pickaxe
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
                    Misc.Pause(200);
                }
            }
            //Player.HeadMessage(15, "Done smelting.");
        }

        public void MoveHarvestedToPackie()
        {
            List<Item> list = Player.Backpack.Contains.Where(x => miningProductsItemIDs.Contains(x.ItemID)).ToList();

            if (list.Where(x => (x.ItemID == 0x1726 || x.ItemID == 0x0DF8)).ToList().Count > 0) // found jade
                Player.ChatSay("[e woohoo");

            if (list.Count > 0)
                Player.HeadMessage(20, "Moving materials...");
            
            foreach (Item item in list)
            {
                Items.Move(item, Mobiles.FindBySerial(packAnimalSerial).Backpack.Serial, -1);
                Misc.Pause(800);
            }
            
            //if (list.Count > 0)
            //    Player.HeadMessage(20, "Moving done...");
        }

        public void WalkTo(PointToWalk to)
        {
            Player.PathFindTo(to.X, to.Y, to.Z);
            int cnt = 0;
            while (cnt < 30 && Player.Position.X != to.X && Player.Position.Y != to.Y)
            {
                cnt++;
                Misc.Pause(500);
            }
        }

        public class PointToWalk
        {
            public PointToWalk(int x, int y, int z)
            {
                this.X = x;
                this.Y = y;
                this.Z = z;
            }

            public int X { get; set; }
            public int Y { get; set; }
            public int Z { get; set; }
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
    }
}
