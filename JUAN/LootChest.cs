//#forcedebug
//C#
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;

namespace RazorEnhanced
{
    public class LootChest
    {

        public void Run()
        {
            int lootContainer = 0x402E139A;

            Dictionary<string, int> LootTable = new Dictionary<string, int>()
                {
                    { "map", lootContainer},
                    { "gold", lootContainer},
                    { "lockpick", lootContainer},
                    { "amber", lootContainer},
                    { "citrine", lootContainer},
                    { "ruby", lootContainer},
                    { "tourmaline", lootContainer},
                    { "amethyst", lootContainer},
                    { "emerald", lootContainer},
                    { "sapphire", lootContainer},
                    { "diamond", lootContainer},
                    { "turquoise", lootContainer},
                    { "pearl", lootContainer},
                    { "hides", lootContainer}
                };

            Player.HeadMessage(15, "Which container should I loot?");
            int chestSerial = new Target().PromptTarget("Which container should I loot?", 15);

            Items.UseItem(chestSerial);
            Misc.Pause(300);

            // Artifact warning
            foreach (Item i in Items.FindBySerial(chestSerial).Contains)
            {
                if (i.Properties.Where(x => x.Args.Contains("Artifact")).ToList().Count > 0)
                    Player.HeadMessage(10, "Artifact detected: " + i.Name);
            }

            foreach (string k in LootTable.Keys)
            {
                List<Item> toLoot = Items.FindBySerial(chestSerial).Contains.Where(x => x.Name.ToLower().Contains(k)).ToList();
                foreach (Item i in toLoot)
                {
                    Items.Move(i, LootTable[k], -1);
                    Misc.Pause(700);
                }
            }


            



            Player.HeadMessage(15, "Done!");
        }
    }
}
