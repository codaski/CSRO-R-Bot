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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Packet packet = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_SELECTCHARACTER, true, enumDestination.Server);
            packet.data.AddSTRING(textBox1.Text, enumStringType.ASCII);
            Globals.ServerPC.SendPacket(packet);
            Globals.CharInsert.Hide();
        }
    }
}
