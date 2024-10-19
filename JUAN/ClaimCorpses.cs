//#xxxforcedebug
//C#
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RazorEnhanced
{
    // The class can have any valid class name
    public class ClaimCorpses
    {
        // This method is the entrypoint and is mandatory
        public void Run()
        {
            Items.Filter corpseFilt = new Items.Filter();
            corpseFilt.RangeMax = 5;
            corpseFilt.IsCorpse = 1;

            List<Item> corpses = Items.ApplyFilter(corpseFilt);

            if (corpses.Count > 0)
            {
                Player.ChatSay("[claim");
                Target.WaitForTarget(1000);

                foreach (Item c in corpses)
                {
                    Target.TargetExecute(c);
                    Target.WaitForTarget(1000);
                }
                Target.Cancel();

            }
        }
    }
}
