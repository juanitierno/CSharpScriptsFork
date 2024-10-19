//#forcedebug
//C#
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;

namespace RazorEnhanced
{
    // The class can have any valid class name
    public class MoveByName
    {
       
        public void Run()
        {
            int toMoves = new Target().PromptTarget("Move what?", 150);
            Item toMove = Items.FindBySerial(toMoves);

            if (toMove != null && toMove.Container > 0)
            {
                Item SourceBag = Items.FindBySerial(toMove.Container);
                int bags = new Target().PromptTarget("Where?", 150);
                Item bag = Items.FindBySerial(bags);

                if (bag != null/* && bag.IsContainer*/)
                {
                    foreach (Item i in SourceBag.Contains.Where(x => x.Name == toMove.Name))
                    {
                        Items.Move(i, bag.Serial, -1);
                        Misc.Pause(600);
                    }
                    
                }

            }

            Player.HeadMessage(15, "Done");
        }
    }
}
