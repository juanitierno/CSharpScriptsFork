//#forcedebug
//C#
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RazorEnhanced
{
    class ArmorBuilderForm : Form
    {
        private Button button1;
        private DataGridView dgProps;
        private Button button2;
        List<ItemRepresentation> allItems;
        Random randGen = new Random(DateTime.Now.Millisecond);

        public ArmorBuilderForm()
        {
            InitializeComponent();
        }

        public void Run()
        {
            Application.EnableVisualStyles();
            Application.Run(this);
        }

        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.dgProps = new System.Windows.Forms.DataGridView();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgProps)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(104, 40);
            this.button1.TabIndex = 0;
            this.button1.Text = "Begin";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dgProps
            // 
            this.dgProps.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dgProps.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgProps.Location = new System.Drawing.Point(12, 58);
            this.dgProps.Name = "dgProps";
            this.dgProps.Size = new System.Drawing.Size(351, 435);
            this.dgProps.TabIndex = 1;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(122, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(104, 40);
            this.button2.TabIndex = 2;
            this.button2.Text = "Build Suit";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // ArmorBuilderForm
            // 
            this.ClientSize = new System.Drawing.Size(936, 505);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dgProps);
            this.Name = "ArmorBuilderForm";
            ((System.ComponentModel.ISupportInitialize)(this.dgProps)).EndInit();
            this.ResumeLayout(false);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            allItems = new List<ItemRepresentation>();

            Item container = Items.FindBySerial(new Target().PromptTarget("Select container:", 15));

            Items.UseItem(container.Serial);
            Misc.Pause(1000);


            foreach (Item i in container.Contains)
            {
                allItems.Add(new ItemRepresentation(i));
                Misc.SendMessage(i.Name);
                Misc.Pause(2);
            }

            List<string> propNames = new List<string>();
            foreach (ItemRepresentation i in allItems)
                propNames.AddRange(from x in i.Properties select x.Property);
            propNames = propNames.Distinct().OrderBy(x=> x).ToList();

            dgProps.DataSource = (from x in propNames select new ItemProp() { Property = x, Value = 0 }).ToList();                        
        }


        private class ItemRepresentation
        {
            public int Serial { get; set; }
            public string Layer { get; set; }
            public List<ItemProp> Properties { get; set; }

            public ItemRepresentation(Item i)
            {
                Properties = new List<ItemProp>();
                Serial = i.Serial;
                Layer = i.Layer;
                foreach (Property s in i.Properties)
                {
                    ItemProp ip = new ItemProp();
                    ip.Property = s.ToString();
                    ip.Value = 1;                     
                       
                    try
                    {
                        ip.Property = s.ToString().Replace(s.Args.ToString(), "").TrimEnd('%').TrimEnd(' ');
                        ip.Value = int.Parse(s.Args);
                    }
                    catch { }
                    Properties.Add(ip);
                }
            }
        }

        private class ItemProp
        {
            public string Property { get; set; }
            public int Value { get; set; }
            public override string ToString()
            {
                return String.Format("{0} {1}",Property,Value);
            }
        }

        private class Suit
        {
            List<ItemRepresentation> Items { get; set; }

            public Suit()
            {
                Items = new List<ItemRepresentation>();
            }

            public Suit(List<ItemRepresentation> items)
            {
                Items = items;
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            Random random = new Random(DateTime.Now.Millisecond);

            List<ItemProp> requirements = new List<ItemProp>();
            requirements = (dgProps.DataSource as List<ItemProp>).Where(x => x.Value > 0).ToList();

            List<List<ItemRepresentation>> nestedArrays = new List<List<ItemRepresentation>>();

            foreach (string l in allItems.Select(x => x.Layer).Distinct())
                nestedArrays.Add(allItems.Where(x => x.Layer == l).ToList());

            //List<List<ItemRepresentation>> combinations = GenerateCombinations(nestedArrays);


            //Dictionary<string,int> indexes = new Dictionary<string,int>();
            //Dictionary<string, List<ItemRepresentation>> items = new Dictionary<string, List<ItemRepresentation>>();

            //foreach (string l in allItems.Select(x=> x.Layer).Distinct())
            //    indexes.Add(l, 0);

            //foreach (string l in allItems.Select(x => x.Layer).Distinct())
            //    items.Add(l, allItems.Where(x=> x.Layer == l).ToList() );




            int a = 2;

            // recorrer
            // validar
            // gen coleccion suits


        }

        static List<List<ItemRepresentation>> GenerateCombinations(List<List<ItemRepresentation>> lists)
        {
            List<List<ItemRepresentation>> combinations = new List<List<ItemRepresentation>>();
            GenerateCombinationsHelper(lists, 0, new List<ItemRepresentation>(), combinations);
            return combinations;
        }

        static void GenerateCombinationsHelper(List<List<ItemRepresentation>> lists, int depth, List<ItemRepresentation> current, List<List<ItemRepresentation>> combinations)
        {
            if (depth == lists.Count)
            {
                combinations.Add(new List<ItemRepresentation>(current));
                return;
            }

            foreach (var item in lists[depth])
            {
                current.Add(item);
                GenerateCombinationsHelper(lists, depth + 1, current, combinations);
                current.RemoveAt(current.Count - 1);
            }
        }
    }
}
