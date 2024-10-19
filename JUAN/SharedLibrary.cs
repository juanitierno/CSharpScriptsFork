//#forcedebug
//C#
using System;
using System.Collections.Generic;
using System.Linq;


namespace RazorEnhanced
{
    // The class can have any valid class name
    public class Transportation
    {
        public static void RecallToRune(int runeSerial, string locationName)
        {
            int x = Player.Position.X;
            int y = Player.Position.Y;
            int attempts = 1;

            while (x == Player.Position.X && y == Player.Position.Y)  // keep trying to recall
            {
                if (attempts > 1)
                    Player.HeadMessage(30, "Attempt Nr: " + attempts.ToString());
                Player.HeadMessage(10, "Recalling: " + locationName + ".");
                Spells.Cast("Recall");
                Target.WaitForTarget(10000);
                Target.TargetExecute(runeSerial);
                Misc.Pause(3000);
                attempts++;
            }
        }



    }
}
