//C#
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RazorEnhanced
{
    // The class can have any valid class name
    //public class ScriptArmorSorter
    //{
    //    public static Random GlobalRand { get; set; }


    //    ArmorSuit _bestSoFar;
    //    List<string> _validLayers = new List<string>() { "InnerTorso",
    //                                                        "Pants",
    //                                                        "Arms",
    //                                                        "Neck",
    //                                                        "Gloves",
    //                                                        "Head" };

    //    public ScriptArmorSorter()
    //    { }

    //    // This method is the entrypoint and is mandatory
    //    public void Run()
    //    {
    //        Script.GlobalRand = new Random(DateTime.Now.Millisecond);
    //        List<ArmorPiece> pieces = new List<ArmorPiece>();

    //        int containerSerial = new Target().PromptTarget("Target a container with gear...", 150);
    //        foreach (Item i in Items.FindBySerial(containerSerial).Contains)
    //        {
    //            if (_validLayers.Contains(i.Layer))
    //                pieces.Add(new ArmorPiece(i));
    //        }

    //        _bestSoFar = ArmorSuit.RandomFromImbuedPieces(pieces);
    //        for (int i = 0; i < 100000; i++)
    //        {
    //            ArmorSuit aSuit = ArmorSuit.RandomFromImbuedPieces(pieces);
    //            //aSuit.ApplyRandomImbues(_rand);  // Apply an imbue to a random resist to 17%.
    //            // mal pq afecta las piezas del pool.

    //            if (aSuit.CalculateRating() > _bestSoFar.CalculateRating())
    //            {
    //                Misc.SendMessage(aSuit.CalculateRating().ToString() + "----" + _bestSoFar.CalculateRating().ToString());
    //                _bestSoFar = aSuit;
    //                Misc.SendMessage("New best: " + _bestSoFar.CalculateRating().ToString());
    //                Misc.Pause(500);
    //            }

    //            if (i % 10000 == 0)
    //            {
    //                Misc.SendMessage("Best so far (out of " + i.ToString() + ": " + _bestSoFar.CalculateRating().ToString());
    //                // Misc.SendMessage( _bestSoFar.ToString() );
    //                Misc.Pause(500);
    //            }
    //        }

    //        DialogResult res = System.Windows.Forms.MessageBox.Show("Highest rated suit found: " + _bestSoFar.CalculateRating().ToString() + "\n"+_bestSoFar.AddResistances().ToString()+ "\n\n" + _bestSoFar.ToString() + "\n Do you want to move the pieces to another container?", "Move pieces to another container?                                                                         .", MessageBoxButtons.YesNo);
    //        if (res == DialogResult.Yes)
    //        {
    //            int destContSerial = new Target().PromptTarget("Where do I put the suit?...", 150);
    //            foreach (ArmorPiece x in _bestSoFar.ArmorPieces.Values)
    //            {
    //                Items.Move(x.Item.Serial, destContSerial, 0);
    //                Misc.Pause(1000);
    //            }
    //        }
    //    }

    //}

    //public enum Layers
    //{
    //    InnerTorso = 0,
    //    Pants,
    //    Arms,
    //    Neck,
    //    Gloves,
    //    Head
    //}

    //public class ArmorPiece
    //{
    //    public Item Item { get; set; }
    //    public int Phys { get; set; }
    //    public int Fire { get; set; }
    //    public int Cold { get; set; }
    //    public int Poison { get; set; }
    //    public int Energy { get; set; }
    //    public string Layer { get; set; }
    //    public string RandomImbue { get; set; }

    //    public ArmorPiece()
    //    { }

    //    public ArmorPiece(Item i)
    //    {
    //        Item = i;
    //        Layer = i.Layer;
    //        foreach (Property p in i.Properties)
    //        {
    //            if (p.ToString().StartsWith("physical resist"))
    //                Phys = Convert.ToInt32(p.Args);
    //            else if (p.ToString().StartsWith("fire resist"))
    //                Fire = Convert.ToInt32(p.Args);
    //            else if (p.ToString().StartsWith("cold resist"))
    //                Cold = Convert.ToInt32(p.Args);
    //            else if (p.ToString().StartsWith("poison resist"))
    //                Poison = Convert.ToInt32(p.Args);
    //            else if (p.ToString().StartsWith("energy resist"))
    //                Energy = Convert.ToInt32(p.Args);
    //        }
    //    }

    //    public ArmorPiece GetRandomImbueCopy()
    //    {
    //        ArmorPiece newPiece = new ArmorPiece()
    //        {
    //            Cold = this.Cold,
    //            Energy = this.Energy,
    //            Layer = this.Layer,
    //            Fire = this.Fire,
    //            Item = this.Item,
    //            Phys = this.Phys,
    //            Poison = this.Poison
    //        };

    //        int resist = Script.GlobalRand.Next(0, 5);

    //        if (resist == 0)
    //        {
    //            newPiece.RandomImbue = "Phys " + (15 - newPiece.Phys).ToString();
    //            newPiece.Phys = 15;
    //        }
    //        else if (resist == 1)
    //        {
    //            newPiece.RandomImbue = "Cold " + (17 - newPiece.Cold).ToString();
    //            newPiece.Cold = 17;
    //        }
    //        else if (resist == 2)
    //        {
    //            newPiece.RandomImbue = "Poison " + (16 - newPiece.Poison).ToString();
    //            newPiece.Poison = 16;
    //        }
    //        else if (resist == 3)
    //        {
    //            newPiece.RandomImbue = "Energy " + (16 - newPiece.Energy).ToString();
    //            newPiece.Energy = 16;
    //        }
    //        else if (resist == 4)
    //        {
    //            newPiece.RandomImbue = "Fire " + (16 - newPiece.Fire).ToString();
    //            newPiece.Fire = 16;
    //        }

    //        return newPiece;

    //    }

    //    public override string ToString()
    //    {
    //        return string.Format("Item:{7} [{0}-{1}-{2}-{3}-{4}] -> Imbue: +{5}", Phys, Fire, Cold, Poison, Energy, RandomImbue, Layer, Item!=null ? Item.Name.Substring(0,Item.Name.IndexOf('(')):"");
    //    }
    //}

    //public class ArmorSuit
    //{
    //    public Dictionary<string, ArmorPiece> ArmorPieces { get; set; }

    //    public ArmorSuit()
    //    {
    //        ArmorPieces = new Dictionary<string, ArmorPiece>();
    //    }

    //    public static ArmorSuit RandomFromPieces(List<ArmorPiece> pieces)
    //    {
    //        ArmorSuit suit = new ArmorSuit();
    //        for (int i = 0; i <= 5; i++)
    //        {
    //            string lay = ((Layers)i).ToString();
    //            suit.ArmorPieces[lay] = GetRandomPieceByLayer(pieces, lay);
    //        }
    //        return suit;
    //    }

    //    public static ArmorSuit RandomFromImbuedPieces(List<ArmorPiece> pieces)
    //    {
    //        ArmorSuit suit = new ArmorSuit();
    //        for (int i = 0; i <= 5; i++)
    //        {
    //            string lay = ((Layers)i).ToString();
    //            ArmorPiece imbued = GetRandomPieceByLayer(pieces, lay).GetRandomImbueCopy();
    //            suit.ArmorPieces[lay] = imbued;
    //        }
    //        return suit;
    //    }

    //    public static ArmorPiece GetRandomPieceByLayer(List<ArmorPiece> items, string layer)
    //    {
    //        try
    //        {
    //            List<ArmorPiece> temp = new List<ArmorPiece>();
    //            foreach (ArmorPiece piece in items)
    //                if (piece.Layer == layer)
    //                    temp.Add(piece);
    //            return temp[Script.GlobalRand.Next(0, temp.Count)];
    //        }
    //        catch
    //        {
    //            Player.HeadMessage(100, "There seem to be no items for layer: " + layer);
    //            Misc.SendMessage("There seem to be no items for layer: " + layer);
    //            throw new Exception("There seem to be no items for layer: " + layer);
    //        }
    //    }

    //    public ArmorPiece AddResistances()
    //    {
    //        ArmorPiece addition = new ArmorPiece() { Layer="Total resists" };
    //        foreach (ArmorPiece piece in ArmorPieces.Values)
    //        {
    //            addition.Cold += piece.Cold;
    //            addition.Phys += piece.Phys;
    //            addition.Poison += piece.Poison;
    //            addition.Energy += piece.Energy;
    //            addition.Fire += piece.Fire;
    //        }
    //        return addition;
    //    }


    //    public int CalculateRating()
    //    {
    //        ArmorPiece addition = AddResistances();
    //        int deviation = 0;
           
    //        deviation += Math.Abs(addition.Cold - 70);
    //        deviation += Math.Abs(addition.Poison - 70);
    //        deviation += Math.Abs(addition.Phys - 70);
    //        deviation += Math.Abs(addition.Energy - 70);
    //        deviation += Math.Abs(addition.Fire - 70);

    //        return 350 - deviation;
    //    }

    //    public override string ToString()
    //    {
    //        string str = ""; //"Score: " + CalculateRating().ToString();
    //        foreach (ArmorPiece p in ArmorPieces.Values)
    //        {
    //            str += p.ToString() + "\n";
    //        }

    //        return str;
    //    }

    //}
}
