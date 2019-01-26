using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Silkroad
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Packet packet = new Packet((ushort)0x7625, false, enumDestination.Server);
            packet.data.AddBYTE(2);
            packet.data.AddSTRING(textBox1.Text, enumStringType.ASCII);
            Globals.ServerPC.SendPacket(packet);
            this.Hide();
        }
    }
}
