//#forcedebug
//C#
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;

namespace RazorEnhanced
{
    // The class can have any valid class name
    public class MoveAllItems
    {
        public void Run()
        {
            int moved = 0;
            DateTime start = DateTime.Now;
            Player.HeadMessage(5, "Take from where?");
            int sourceCont = new Target().PromptTarget("Take from where?", 150);
            if (Mobiles.FindBySerial(sourceCont) != null && Mobiles.FindBySerial(sourceCont).Backpack != null)
                sourceCont = Mobiles.FindBySerial(sourceCont).Backpack.Serial;
            if (sourceCont == -1)
                return;

            Player.HeadMessage(5, "Deposit where?");
            int destCont = new Target().PromptTarget("Deposit where?", 150);
            if (destCont == -1)
                return;

            Items.UseItem(sourceCont);
            Misc.Pause(800);
            Items.UseItem(destCont);
            Misc.Pause(800);

            foreach (Item i in Items.FindBySerial(sourceCont).Contains)
            {
                if (i.ItemID == 0) continue;
                if (!FastMoveItem(i.Serial, destCont))
                {
                    Player.HeadMessage(5, "There was a problem moving items!");
                    break;
                }
                moved++;
            }
            TimeSpan ts = DateTime.Now - start;
            Player.HeadMessage(15, String.Format("Moved {0} items in {1} seconds.", moved, Math.Round(ts.TotalSeconds, 1)));
        }

        public bool FastMoveItem(int itemSerial, int destSerial)
        {
            Item item = Items.FindBySerial(itemSerial);
            Misc.SendMessage("Moving " + item.Name);

            Item destContainer = Items.FindBySerial(destSerial);
            
            // see if it's a stackable and already exists in destination.
            int sameItemInDestAmnt = destContainer.Contains.Where(x => (x.ItemID == item.ItemID && x.Hue == item.Hue)).Sum(x => x.Amount);
            int sameItemInDestAmnt2 = sameItemInDestAmnt;

            Items.Move(itemSerial, destSerial, -1);

            int loops = 0;
            while (true)
            {
                sameItemInDestAmnt = destContainer.Contains.Where(x => (x.ItemID == item.ItemID && x.Hue == item.Hue)).Sum(x => x.Amount);

                if (loops >= 40) // tinout
                    return false;
                else if (Items.FindBySerial(destSerial).Contains.Where(x => x.Serial == itemSerial).Count() > 0) // found the item in the destination
                    break;
                else if (sameItemInDestAmnt != sameItemInDestAmnt2)  // Same item in destimnation got their amount changed (increased)
                    break;
                Misc.Pause(50);
                loops++;
            }
            Misc.Pause(100);
            return true;
        }

    }
}
