//#forcedebug
//C#
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;

namespace RazorEnhanced
{
    // The class can have any valid class name
    public class MoveByProperty
    {
        public void Run()
        {
            Player.HeadMessage(15, "Type part of the property you are looking to move:");
            Journal j = new Journal();
            j.Clear();

            while (j.GetTextByName(Player.Name).Count == 0)
                Misc.Pause(200);

            string prop = j.GetTextByName(Player.Name).FirstOrDefault();
            if (prop == null) return;

            prop = prop.ToLower();

            Player.HeadMessage(5, "Take from where?");
            int sourceCont = new Target().PromptTarget("Take from where?", 150);
            if (sourceCont == -1) return;

            Player.HeadMessage(5, "Deposit where?");
            int destCont = new Target().PromptTarget("Deposit where?", 150);
            if (destCont == -1) return;

            Items.UseItem(sourceCont);
            Misc.Pause(300);
            Items.UseItem(destCont);
            Misc.Pause(300);

            foreach (Item i in Items.FindBySerial(sourceCont).Contains)
            {
                if (Items.GetPropStringList(i).Where(x => x.ToLower().Contains(prop)).Count() > 0) // Any matching property
                {
                    Items.Move(i.Serial, destCont, -1);
                    Misc.Pause(800);
                }
            }
            Player.HeadMessage(15, "Done");
        }
    }
}
