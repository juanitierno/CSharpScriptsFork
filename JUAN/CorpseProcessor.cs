//#forcedebug
//C#
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;

namespace RazorEnhanced
{
    public class CorpseProcessor
    {
        Dictionary<string, int> EngravedBags = new Dictionary<string, int>();


        public void Run()
        {
            BuildBagsDictionary();
            int packAnimalSerial = -1;
            Mobile packAnimal = FindMyPet();
            if (packAnimal != null && packAnimal.Backpack != null)
                packAnimalSerial = packAnimal.Serial;


            // Filters by part of the name of the item.
            Dictionary<string, string> LootTable = new Dictionary<string, string>()
            {
                { "map", "-LOOT-"},
                { "gold", "-LOOT-"},
                { "lockpick", "-LOOT-"},
                { "dragon scales", "-LOOT-"},
                { "amber", "-GEMS-"},
                { "citrine", "-GEMS-"},
                { "ruby", "-GEMS-"},
                { "tourmaline", "-GEMS-"},
                { "amethyst", "-GEMS-"},
                { "emerald", "-GEMS-"},
                { "sapphire", "-GEMS-"},
                { "diamond", "-GEMS-"},
                { "turquoise", "-GEMS-"},
                { "hides", "-LOOT-"}
            };


            // which items get passed on to the packie/beetle.
            List<string> toTransferToPackie = new List<string>() { "gold", "cut leather", "dragon scales", "Dragon's Blood", "raw ribs" };

            Player.HeadMessage(50, "Processing!");
            Item harvestersBlade = Items.FindByName("Harvester's Blade", -1, Player.Backpack.Serial, 2);

            Items.Filter corpseFilt = new Items.Filter();
            corpseFilt.RangeMax = 2;
            corpseFilt.IsCorpse = 1;
            List<Item> corpses = Items.ApplyFilter(corpseFilt);
            Misc.Pause(100);

            foreach (Item c in corpses)
            {
                if (c.Hue == 80)
                {
                    Items.Message(c.Serial, 80, "Skipping");
                    continue;
                }

                // skin only if hunting with a pack animal
                if (harvestersBlade != null && Mobiles.FindBySerial(packAnimalSerial) != null)
                {
                    Items.UseItem(harvestersBlade);
                    Target.WaitForTarget(1000);
                    Target.TargetExecute(c);
                    Misc.Pause(200);
                }
                Misc.Pause(500);

                // Open the corpse
                Items.UseItem(c);
                Misc.Pause(800);

                // loot
                foreach (string k in LootTable.Keys)
                {
                    List<Item> toLoot = c.Contains.Where(x => x.Name.ToLower().Contains(k)).ToList();
                    foreach (Item i in toLoot)
                    {
                        FastMoveItem(i.Serial, EngravedBags[LootTable[k]]);
                        Misc.Pause(200);

                    }
                }
                Items.SetColor(c.Serial, 80);
                Misc.Pause(800);
            }

            // Move heavy looted items to packie, if close enough.
            Mobile packie = Mobiles.FindBySerial(packAnimalSerial);
            if (packie != null && Player.DistanceTo(packie) <= 2)
            {
                Items.UseItem(packie.Backpack.Serial);

                if (packie.Backpack.Weight > 1000)
                    Player.HeadMessage(15, "Packie almost full: " + packie.Backpack.Weight + " stones.");

                // loot bags
                foreach (int bagS in EngravedBags.Values)
                {
                    foreach (string tot in toTransferToPackie)
                    {
                        List<Item> toTransfer = Items.FindBySerial(bagS).Contains.Where(x => x.Name.Contains(tot)).ToList();
                        foreach (Item i in toTransfer)
                        {
                            FastMoveItem(i.Serial, packie.Backpack.Serial);
                            Misc.Pause(100);
                        }
                    }
                }

                // main backpack (mainly harvester's blade stuff)
                foreach (string tot in toTransferToPackie)
                {
                    List<Item> toTransfer = Player.Backpack.Contains.Where(x => x.Name.Contains(tot)).ToList();
                    foreach (Item i in toTransfer)
                    {
                        FastMoveItem(i.Serial, packie.Backpack.Serial);
                        Misc.Pause(100);
                    }
                }
                Misc.Pause(1000);
            }

            // drop meat
            Item meat = Player.Backpack.Contains.Where(x => x.Name.Contains("raw ribs")).FirstOrDefault();
            if (meat != null)
            {
                //Items.DropItemGroundSelf(meat.Serial);
                Items.MoveOnGround(meat.Serial, -1, Player.Position.X + 1, Player.Position.Y + 1, Player.Position.Z);
                Misc.Pause(800);

            }

            Player.HeadMessage(50, "All done!");
        }

        private void BuildBagsDictionary()
        {
            List<Item> bags = Player.Backpack.Contains.Where(x => !String.IsNullOrEmpty(Items.GetPropValueString(x.Serial, "Engraved"))).ToList();
            foreach (Item b in bags)
            {
                EngravedBags.Add(Items.GetPropValueString(b.Serial, "Engraved"), b.Serial);
            }

            //int a = 2;
        }

        public void FastMoveItem(int itemSerial, int destSerial)
        {
            Item item = Items.FindBySerial(itemSerial);
            Item destContainer = Items.FindBySerial(destSerial);
            // see if it's a stackable and already exists in destination.
            Item sameItemInDest = destContainer.Contains.Where(x => (/*x.Name == item.Name &&*/ x.ItemID == item.ItemID && x.Hue == item.Hue)).FirstOrDefault();
            int sameItemAmount = 0;
            if (sameItemInDest != null)
                sameItemAmount = sameItemInDest.Amount;

            Items.Move(itemSerial, destSerial, -1);

            int loops = 0;
            while (true)
            {
                if (loops >= 40) // tinout
                {
                    Player.HeadMessage(5, "Timout");
                    break;
                }
                else if (Items.FindBySerial(destSerial).Contains.Where(x => x.Serial == itemSerial).Count() > 0) // found the item in the destination
                    break;
                else if (sameItemInDest != null && sameItemInDest.Amount != sameItemAmount)  // Same item in destimnation got their amount changed (increased)
                    break;

                Misc.Pause(50);
                loops++;
            }
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
