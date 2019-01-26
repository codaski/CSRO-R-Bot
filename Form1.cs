using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Silkroad
{
    public partial class Form1 : Form
    {
        [DllImport("kernel32.dll")]
        private static extern IntPtr CreateMutex(IntPtr lpMutexAttributes, bool bInitialOwner, string lpName);
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(UInt32 dwDesiredAccess, Int32 bInheritHandle, Int32 dwProcessId);
        [DllImport("kernel32.dll")]
        static extern IntPtr OpenThread(Globals.ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);
        [DllImport("kernel32.dll")]
        static extern uint SuspendThread(IntPtr hThread);
        [DllImport("kernel32.dll")]
        static extern int ResumeThread(IntPtr hThread);
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);
        [DllImport("kernel32.dll")]
        static extern IntPtr WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, string lpBuffer, UIntPtr nSize, out IntPtr lpNumberOfBytesWritten);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);
        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true)]
        public static extern UIntPtr GetProcAddress(IntPtr hModule, string procName);
        [DllImport("kernel32")]
        public static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, UIntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, out IntPtr lpThreadId);
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CloseHandle(IntPtr hObject);

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Globals.MainWindow = this;
            Form2 frm = new Form2();
            Form3 frm1 = new Form3();
            this.Text = Globals.botTitle;
            Data.InitializeTypes();
            Data.LoadShopTabData();
            LoadTXT.LoadData();
            LoadTXT.CheckScripts();
            LoadTXT.LoadServers();
            LoadTXT.LoadTablets();
            Globals.Init();
            System.IO.MemoryStream ms = new System.IO.MemoryStream(Data.img);
            Image temp = Image.FromStream(ms);
            Globals.MainWindow.captcha_box.Image = temp;
            SilkroadSecurity.Initialize();
            Globals.SecundaryPass = frm;
            Globals.SecundaryPass.Show();
            Globals.SecundaryPass.Hide();
            Globals.CharInsert = frm1;
            Globals.CharInsert.Show();
            Globals.CharInsert.Hide();
            Globals.MainWindow.gold_drop.Text = "Ignore";
            Globals.MainWindow.wep_drop.Text = "Ignore";
            Globals.MainWindow.armor_drop.Text = "Ignore";
            Globals.MainWindow.acc_drop.Text = "Ignore";
            Globals.MainWindow.bolt_drop.Text = "Ignore";
            Globals.MainWindow.arrow_drop.Text = "Ignore";
            Globals.MainWindow.hp_drop.Text = "Ignore";
            Globals.MainWindow.mp_drop.Text = "Ignore";
            Globals.MainWindow.return_drop.Text = "Ignore";
            Globals.MainWindow.vigorp_drop.Text = "Ignore";
            Globals.MainWindow.vigorg_drop.Text = "Ignore";
            Globals.MainWindow.uni_drop.Text = "Ignore";
            Globals.MainWindow.materials_drop.Text = "Ignore";
            Globals.MainWindow.tablets_drop.Text = "Store";
            Globals.MainWindow.wepe_drop.Text = "Ignore";
            Globals.MainWindow.prote_drop.Text = "Ignore";
            Globals.MainWindow.acce_drop.Text = "Ignore";
            Globals.MainWindow.shielde_drop.Text = "Ignore";
            Globals.MainWindow.comboBox2.Text = "Ignore";

            for (int i = 0; i < tabControl1.TabPages.Count; i++)
            {
                tabControl1.SelectedIndex = i;
            }
            tabControl1.SelectedIndex = 0;
            for (int i = 0; i < tabControl2.TabPages.Count; i++)
            {
                tabControl2.SelectedIndex = i;
            }
            tabControl2.SelectedIndex = 0;
            for (int i = 0; i < tabControl3.TabPages.Count; i++)
            {
                tabControl3.SelectedIndex = i;
            }
            tabControl3.SelectedIndex = 0;
            for (int i = 0; i < tabs.TabPages.Count; i++)
            {
                tabs.SelectedIndex = i;
            }
            tabs.SelectedIndex = 0;

            hp_buy.SelectedIndex = 0;
            mp_buy.SelectedIndex = 0;
            scroll_buy.SelectedIndex = 0;
            horse_buy.SelectedIndex = 0;
            uni_buy.SelectedIndex = 0;
            speed_buy.SelectedIndex = 0;
            php_buy.SelectedIndex = 0;
            using (StreamReader streamReader = new StreamReader(@"configs/server.cfg", Encoding.UTF8))
            {
                streamReader.ReadLine();
                BotData.LoginServer.ip = streamReader.ReadLine();
                streamReader.ReadLine();
                BotData.LoginServer.port = Convert.ToInt32(streamReader.ReadLine());
                streamReader.ReadLine();
                BotData.LoginServer.version = Convert.ToUInt32(streamReader.ReadLine());
                streamReader.ReadLine();
                BotData.sro_path = streamReader.ReadLine();
            }
            if (!File.Exists(BotData.sro_path))
            {
                select_path.Enabled = true;
            }
            
            BotData.sro_server = "CSRo-R";
            //BotData.LoginServer.ip = "192.99.119.239";
            BotData.LoginServer.locale = 34;
            //BotData.LoginServer.version = 6;
            //use_clientless.Enabled = true;
            use_client.Enabled = true;
            xpPercentLabel1.BackColor = Color.Transparent;
            hpshow1.BackColor = Color.Transparent;
            MPShow1.BackColor = Color.Transparent;
            zerkn1.BackColor = Color.Transparent;
            Globals.UpdateLogs("Welcome " + Globals.botTitle + " Silkroad Version: " + Globals.SilkroadVersion);
            Globals.UpdateLogs("updates are posted in www.srodev.com");
            Globals.UpdateLogs("PLEASE, IF YOU GET ANY ERROR UPLOAD ARCHIVES FROM FOLDER DEBUG TO SRODEV.COM");
            use_clientless.Enabled = false;
            BotData.use_client = true;
            server_name.Enabled = false;
            start_client.Enabled = true;
            //select_path.Enabled = true;
        }

        private void Start()
        {
            Globals.Connection.Connect(BotData.LoginServer.ip, BotData.LoginServer.port);
        }

        private void server_name_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < BotData.Servers.Length; i++)
            {
                if (BotData.Servers[i].name == server_name.Text)
                {
                    BotData.sro_server = BotData.Servers[i].name;
                    BotData.LoginServer.ip = BotData.Servers[i].ip;
                    BotData.LoginServer.port = 15779;
                    BotData.LoginServer.locale = BotData.Servers[i].locale;
                    BotData.LoginServer.version = BotData.Servers[i].version;
                    use_clientless.Enabled = true;
                    use_client.Enabled = true;
                    break;
                }
            }
        }

        private void login_Click(object sender, EventArgs e)
        {
            Packet packet = new Packet((ushort)LoginServerOpcodes.CLIENT_OPCODES.LOGIN, false, enumDestination.Server);
            packet.data.AddBYTE(BotData.LoginServer.locale);
            packet.data.AddSTRING(username.Text, enumStringType.ASCII);
            packet.data.AddSTRING(password.Text, enumStringType.ASCII);
            packet.data.AddWORD(BotData.Server_ID[BotData.Server_Name.IndexOf(in_game_server_name.Text)]);
            Globals.ServerPC.SendPacket(packet);
        }

        private void captcha_confirm_Click(object sender, EventArgs e)
        {
            Packet packet = new Packet((ushort)0x7625, false, enumDestination.Server);
            packet.data.AddBYTE(2);
            packet.data.AddSTRING(captcha_text.Text, enumStringType.ASCII);
            Globals.ServerPC.SendPacket(packet);
        }

        private void use_client_CheckedChanged(object sender, EventArgs e)
        {
            use_clientless.Enabled = false;
            BotData.use_client = true;
            server_name.Enabled = false;
            start_client.Enabled = true;
            select_path.Enabled = true;
        }
        private void use_clientless_CheckedChanged(object sender, EventArgs e)
        {
           
            use_client.Enabled = false;
            server_name.Enabled = false;
            System.Threading.Thread connection_thread = new System.Threading.Thread(Start);
           connection_thread.Start();
           Globals.Connection.ClientListen("192.168.1.17", 15777);
        }

        private void select_path_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FileDialog fdialog = new System.Windows.Forms.OpenFileDialog();
            fdialog.Filter = "sro_client.exe | *.exe";
            fdialog.ShowDialog();
            if (File.Exists(fdialog.FileName) && fdialog.FileName.ToUpper().Contains("SRO_CLIENT.EXE"))
            {
                BotData.sro_path = fdialog.FileName;
                //Globals.Configs.WriteBotConfigs();
                  if (Directory.Exists(Application.StartupPath + @"/configs/" + @"\"))
                {
                    TextWriter config_writer = new StreamWriter(Application.StartupPath + @"/configs/server" + ".cfg");
                    //TrainingSettings
                    config_writer.WriteLine("//ip");
                    config_writer.WriteLine(BotData.LoginServer.ip);
                    config_writer.WriteLine("//port");
                    config_writer.WriteLine(BotData.LoginServer.port);
                    config_writer.WriteLine("//version");
                    config_writer.WriteLine(BotData.LoginServer.version);
                    config_writer.WriteLine("// folder client");
                    config_writer.WriteLine(BotData.sro_path);
                    config_writer.Close();
                }
            }
        }

        #region Start Client
        private Process sro_process;
        private void start_client_Click(object sender, EventArgs e)
        {
            if (LoginServer.port_sro == -1)
            {
                if (BotData.sro_server != null && BotData.sro_server != "")
                {
                    for (int i = 1700; i < 2000; i++)
                    {
                        if (Globals.CheckPort(i) == true)
                        {
                           /* if (MessageBox.Show("Use edxLoader ?" + Environment.NewLine + Environment.NewLine + "Press Yes to get port." + Environment.NewLine + "Press No to use Built-In loader (single client)", "Client Options", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                            {
                               LoginServer.port_sro = i;
                               Globals.Connection.ClientListen("127.0.0.1", LoginServer.port_sro);
                                MessageBox.Show("Use 127.0.0.1 IP and " + i + " port.");
                            }
                            else
                            {*/
                                if (BotData.sro_path == "" || BotData.sro_path == null || !File.Exists(BotData.sro_path))
                                {
                                    MessageBox.Show("Please select sro_client.exe !");
                                }
                                else
                                {
                                        try
                                        {
                                            LoginServer.port_sro = i;
                                            Globals.Connection.ClientListen("127.0.0.1", LoginServer.port_sro);
                                            TextWriter tw = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\zbot_loader.ini", false);
                                            tw.WriteLine("[loader]");
                                            tw.WriteLine("ip=127.0.0.1");
                                            tw.WriteLine("port=" + LoginServer.port_sro);
                                            tw.Close();
                                            CreateMutex(IntPtr.Zero, false, "Silkroad Online Launcher");
                                            CreateMutex(IntPtr.Zero, false, "Ready");
                                            sro_process = Process.Start(BotData.sro_path, "0 /" + BotData.LoginServer.locale + " 0 0"); //38
                                            SuspendProcess(sro_process.Id);
                                            IntPtr hProcess = OpenProcess(0x1F0FFF, 1, sro_process.Id);
                                            InjectDLL(hProcess, Environment.CurrentDirectory + @"\loader.dll");
                                            ResumeProcess(sro_process.Id);
                                            start_client.Enabled = false;
                                            select_path.Enabled = false;
                                        }
                                        catch (Exception a)
                                        {
                                            MessageBox.Show("Cannot Launch Silkroad ! " + a.Message);
                                        }
                                }
                           // }
                            break;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please Select Server");
                }
            }
            else
            {
                MessageBox.Show("Use 127.0.0.1 IP and " + LoginServer.port_sro + " port.");
            }
        }

        public static void InjectDLL(IntPtr hProcess, string DLLPath)
        {
            IntPtr bytesout;
            if (File.Exists(DLLPath))
            {
                Int32 bufferSize = DLLPath.Length + 1;
                IntPtr AllocMem = VirtualAllocEx(hProcess, IntPtr.Zero, (uint)bufferSize, 4096, 4);
                WriteProcessMemory(hProcess, AllocMem, DLLPath, (UIntPtr)bufferSize, out bytesout);
                UIntPtr Injector = (UIntPtr)GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");
                IntPtr hThread = (IntPtr)CreateRemoteThread(hProcess, (IntPtr)null, 0, Injector, AllocMem, 0, out bytesout);
                CloseHandle(hThread);
            }
            else
            {
                MessageBox.Show("loader.dll Not Found !");
            }
        }

        private void SuspendProcess(int PID)
        {
            Process proc = Process.GetProcessById(PID);

            if (proc.ProcessName == string.Empty)
                return;

            foreach (ProcessThread pT in proc.Threads)
            {
                IntPtr pOpenThread = OpenThread(Globals.ThreadAccess.SUSPEND_RESUME, false, (uint)pT.Id);

                if (pOpenThread == IntPtr.Zero)
                {
                    break;
                }

                SuspendThread(pOpenThread);
            }
        }

        public void ResumeProcess(int PID)
        {
            Process proc = Process.GetProcessById(PID);

            if (proc.ProcessName == string.Empty)
                return;

            foreach (ProcessThread pT in proc.Threads)
            {
                IntPtr pOpenThread = OpenThread(Globals.ThreadAccess.SUSPEND_RESUME, false, (uint)pT.Id);

                if (pOpenThread == IntPtr.Zero)
                {
                    break;
                }

                ResumeThread(pOpenThread);
            }
        }
        #endregion

        private void captcha_text_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && captcha_confirm.Enabled == true)
            {
                Packet packet = new Packet((ushort)0x6323, false, enumDestination.Server);
                packet.data.AddSTRING(captcha_text.Text, enumStringType.ASCII);
                Globals.ServerPC.SendPacket(packet);
                captcha_text.Text = "";
            }
        }

        private void select_Click(object sender, EventArgs e)
        {
            Packet packet = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_SELECTCHARACTER, true, enumDestination.Server);
            packet.data.AddSTRING(char_name.Text, enumStringType.ASCII);
            Globals.ServerPC.SendPacket(packet);
        }

        #region STR INT Up Buttons
        private void bt_up_str_Click(object sender, EventArgs e)
        {
            Globals.ServerPC.SendPacket(new Packet((ushort)0x7050, false, enumDestination.Server));
        }
        private void bt_up_int_Click(object sender, EventArgs e)
        {
            Globals.ServerPC.SendPacket(new Packet((ushort)0x7051, false, enumDestination.Server));
        }
        #endregion
        #region Tablets Pick Settings
        private void tablet_dont_click(object sender, EventArgs e)
        {
            if (tablet_dont.SelectedIndex != -1)
            {
                tablet_pick.Items.Add(tablet_dont.Items[tablet_dont.SelectedIndex]);
                tablet_dont.Items.RemoveAt(tablet_dont.SelectedIndex);
                tablet_dont.ContextMenuStrip = null;
            }
        }
        private void tablet_dont_MouseClick(object sender, MouseEventArgs e)
        {
            if (tablet_dont.SelectedIndex != -1)
            {
                ContextMenuStrip cs = new ContextMenuStrip();
                cs.Items.Add(tablet_dont.Items[tablet_dont.SelectedIndex].ToString());
                cs.Items[0].Enabled = false;
                cs.Items.Add("Pick");
                cs.Items[1].Click += new EventHandler(tablet_dont_click);
                tablet_dont.ContextMenuStrip = cs;
            }
        }
        private void tablet_pick_MouseClick(object sender, MouseEventArgs e)
        {
            if (tablet_pick.SelectedIndex != -1)
            {
                ContextMenuStrip cs = new ContextMenuStrip();
                cs.Items.Add(tablet_pick.Items[tablet_pick.SelectedIndex].ToString());
                cs.Items[0].Enabled = false;
                cs.Items.Add("Don't Pick");
                cs.Items[1].Click += new EventHandler(tablet_pick_click);
                tablet_pick.ContextMenuStrip = cs;
            }
        }
        private void tablet_pick_click(object sender, EventArgs e)
        {
            if (tablet_pick.SelectedIndex != -1)
            {
                tablet_dont.Items.Add(tablet_pick.Items[tablet_pick.SelectedIndex]);
                tablet_pick.Items.RemoveAt(tablet_pick.SelectedIndex);
                tablet_pick.ContextMenuStrip = null;
            }
        }
        #endregion

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (settings_tray.Checked)
            {
                if (FormWindowState.Minimized == WindowState)
                {
                    Hide();
                }
            }
        }
        private void tray_DoubleClick(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void start_button_Click(object sender, EventArgs e)
        {
            string mariana = Char_Data.f_wep_name = Char_Data.inventorytype[Char_Data.inventoryslot[6]]; 
            string weapon2 = Char_Data.inventorytype[Char_Data.inventoryslot[9]];
                if (start_button.Text == "Start Bot")
                {
                    if (Char_Data.f_wep_name != mariana)
                    {
                        Char_Data.f_wep_name = Char_Data.inventorytype[Char_Data.inventoryslot[6]];
                        if (weaponwich.Checked = true)
                        {
                            if (Char_Data.s_wep_name != weapon2)
                            {
                                Char_Data.s_wep_name = Char_Data.inventorytype[Char_Data.inventoryslot[9]];
                            }
                        }
                    }
                    trainx.Text = Character.X.ToString();
                    trainy.Text = Character.Y.ToString();
                    start_button.Text = "Stop Bot";
                    StartLooping.CheckStart();
                   // BotData.bot = true;
                    timer1.Enabled = true;
                }
               else
                {
                    start_button.Text = "Start Bot";
                    Globals.UpdateLogs("Bot Stoped");
                    BotData.bot = false;
                    BotData.loop = false;
                    BotData.returning = 0;
               }
          //  }
            //else
            //{
              //  Globals.UpdateLogs("Please Select First (And) Second Weapon !");
            //}
        }



        private void skills_list_MouseClick(object sender, MouseEventArgs e)
        {
            if (skills_list.SelectedIndex != -1)
            {
                ContextMenuStrip cs = new ContextMenuStrip();
                cs.Items.Add(skills_list.Items[skills_list.SelectedIndex].ToString()).Enabled = false;
                cs.Items.Add("-").Enabled = false;
                cs.Items.Add("Add Imbue").Click += new EventHandler(skills_list_add_imbue);
                cs.Items.Add("Add To General Skill").Click += new EventHandler(skills_list_add_general);
                //cs.Items.Add("Add To Giant / Party Skill").Click += new EventHandler(skills_list_add_party);
                cs.Items.Add("Add 1st Weapon Buff").Click += new EventHandler(skills_list_add_1weapon);
                cs.Items.Add("Add 2nd Weapon Buff").Click += new EventHandler(skills_list_add_2weapon);
                //cs.Items.Add("Add 1st Giant / Party Buff").Click += new EventHandler(skills_list_add_1weapon_gp);
                //cs.Items.Add("Add 2nd Giant / Party Buff").Click += new EventHandler(skills_list_add_2weapon_gp);
                cs.Items[1].Click += new EventHandler(skills_general_list_remove);
                skills_list.ContextMenuStrip = cs;
            }
        }
        private void skills_list_add_imbue(object sender, EventArgs e)
        {
            if (skills_list.SelectedIndex != -1)
            {
                imbue_name.Text = skills_list.Items[skills_list.SelectedIndex].ToString();
                skills_list.ContextMenu = null;
            }
        }
        private void skills_list_add_general(object sender, EventArgs e)
        {
            if (skills_list.SelectedIndex != -1)
            {
                skills_general_list.Items.Add(skills_list.Items[skills_list.SelectedIndex]);
                skills_list.ContextMenu = null;
            }
        }
      /*  private void skills_list_add_party(object sender, EventArgs e)
        {
            if (skills_list.SelectedIndex != -1)
            {
                skills_party_list.Items.Add(skills_list.Items[skills_list.SelectedIndex]);
                skills_list.ContextMenu = null;
            }
        }*/
        private void skills_list_add_1weapon(object sender, EventArgs e)
        {
            if (skills_list.SelectedIndex != -1 && buffs_list1.Items.IndexOf(skills_list.Items[skills_list.SelectedIndex]) == -1)
            {
                buffs_list1.Items.Add(skills_list.Items[skills_list.SelectedIndex]);
                skills_list.ContextMenu = null;
                if (Char_Data.skillonname.IndexOf(skills_list.SelectedItem.ToString()) == -1)
                {
                    Char_Data.skillnamewaiting.Add(skills_list.SelectedItem.ToString());
                    Char_Data.skilltipwaiting.Add(1);
                    Buffas.buff_waiting = true;
                }
            }
        }
        private void skills_list_add_2weapon(object sender, EventArgs e)
        {
            if (skills_list.SelectedIndex != -1 && buffs_list2.Items.IndexOf(skills_list.Items[skills_list.SelectedIndex]) == -1)
            {
                buffs_list2.Items.Add(skills_list.Items[skills_list.SelectedIndex]);
                skills_list.ContextMenu = null;
                if (Char_Data.skillonname.IndexOf(skills_list.SelectedItem.ToString()) == -1)
                {
                    Char_Data.skillnamewaiting.Add(skills_list.SelectedItem.ToString());
                    Char_Data.skilltipwaiting.Add(2);
                    Buffas.buff_waiting = true;
                }
            }
        }

      /*  private void skills_list_add_1weapon_gp(object sender, EventArgs e)
        {
            if (skills_list.SelectedIndex != -1 && buffs_list3.Items.IndexOf(skills_list.Items[skills_list.SelectedIndex]) == -1)
            {
                buffs_list3.Items.Add(skills_list.Items[skills_list.SelectedIndex]);
                skills_list.ContextMenu = null;
                if (Char_Data.skillonname.IndexOf(skills_list.SelectedItem.ToString()) == -1)
                {
                    Char_Data.skillnamewaiting.Add(skills_list.SelectedItem.ToString());
                    Char_Data.skilltipwaiting.Add(3);
                }
            }
        }*/
    /*    private void skills_list_add_2weapon_gp(object sender, EventArgs e)
        {
            if (skills_list.SelectedIndex != -1 && buffs_list4.Items.IndexOf(skills_list.Items[skills_list.SelectedIndex]) == -1)
            {
                buffs_list4.Items.Add(skills_list.Items[skills_list.SelectedIndex]);
                skills_list.ContextMenu = null;
                if (Char_Data.skillonname.IndexOf(skills_list.SelectedItem.ToString()) == -1)
                {
                    Char_Data.skillnamewaiting.Add(skills_list.SelectedItem.ToString());
                    Char_Data.skilltipwaiting.Add(4);
                }
            }
        }*/


        private void skills_general_list_MouseClick(object sender, MouseEventArgs e)
        {
            if (skills_general_list.SelectedIndex != -1)
            {
                ContextMenuStrip cs = new ContextMenuStrip();
                cs.Items.Add(skills_general_list.Items[skills_general_list.SelectedIndex].ToString()).Enabled = false;
                cs.Items.Add("-").Enabled = false;
                cs.Items.Add("Remove").Click += new EventHandler(skills_general_list_remove);
                skills_general_list.ContextMenuStrip = cs;
            }
        }
       /* private void skills_party_list_MouseClick(object sender, MouseEventArgs e)
        {
            if (skills_party_list.SelectedIndex != -1)
            {
                ContextMenuStrip cs = new ContextMenuStrip();
                cs.Items.Add(skills_party_list.Items[skills_party_list.SelectedIndex].ToString()).Enabled = false;
                cs.Items.Add("-").Enabled = false;
                cs.Items.Add("Remove").Click += new EventHandler(skills_party_list_remove);
                skills_party_list.ContextMenuStrip = cs;
            }
        }*/

        private void skills_general_list_remove(object sender, EventArgs e)
        {
            if (skills_general_list.SelectedIndex != -1)
            {
                skills_general_list.Items.RemoveAt(skills_general_list.SelectedIndex);
                skills_general_list.ContextMenuStrip = null;
            }
        }
     /*   private void skills_party_list_remove(object sender, EventArgs e)
        {
            if (skills_party_list.SelectedIndex != -1)
            {
                skills_party_list.Items.RemoveAt(skills_party_list.SelectedIndex);
                skills_party_list.ContextMenuStrip = null;
            }
        }
        */
        private void imbue_name_MouseClick(object sender, MouseEventArgs e)
        {
            ContextMenuStrip cs = new ContextMenuStrip();
            cs.Items.Add(imbue_name.Text).Enabled = false;
            cs.Items.Add("-").Enabled = false;
            cs.Items.Add("Remove").Click += new EventHandler(imbue_name_remove);
            imbue_name.ContextMenuStrip = cs;
        }
        private void imbue_name_remove(object sender, EventArgs e)
        {
            imbue_name.Text = "";
        }

        private void buffs_list1_MouseClick(object sender, MouseEventArgs e)
        {
            if (buffs_list1.SelectedIndex != -1)
            {
                ContextMenuStrip cs = new ContextMenuStrip();
                cs.Items.Add(buffs_list1.Items[buffs_list1.SelectedIndex].ToString()).Enabled = false;
                cs.Items.Add("-").Enabled = false;
                cs.Items.Add("Remove").Click += new EventHandler(buffs_list1_remove);
                buffs_list1.ContextMenuStrip = cs;
            }
        }
        private void buffs_list2_MouseClick(object sender, MouseEventArgs e)
        {
            if (buffs_list2.SelectedIndex != -1)
            {
                ContextMenuStrip cs = new ContextMenuStrip();
                cs.Items.Add(buffs_list2.Items[buffs_list2.SelectedIndex].ToString()).Enabled = false;
                cs.Items.Add("-").Enabled = false;
                cs.Items.Add("Remove").Click += new EventHandler(buffs_list2_remove);
                buffs_list2.ContextMenuStrip = cs;
            }
        }
       /* private void buffs_list3_MouseClick(object sender, MouseEventArgs e)
        {
            if (buffs_list3.SelectedIndex != -1)
            {
                ContextMenuStrip cs = new ContextMenuStrip();
                cs.Items.Add(buffs_list3.Items[buffs_list3.SelectedIndex].ToString()).Enabled = false;
                cs.Items.Add("-").Enabled = false;
                cs.Items.Add("Remove").Click += new EventHandler(buffs_list3_remove);
                buffs_list3.ContextMenuStrip = cs;
            }
        }
        private void buffs_list4_MouseClick(object sender, MouseEventArgs e)
        {
            if (buffs_list4.SelectedIndex != -1)
            {
                ContextMenuStrip cs = new ContextMenuStrip();
                cs.Items.Add(buffs_list4.Items[buffs_list4.SelectedIndex].ToString()).Enabled = false;
                cs.Items.Add("-").Enabled = false;
                cs.Items.Add("Remove").Click += new EventHandler(buffs_list4_remove);
                buffs_list4.ContextMenuStrip = cs;
            }
        }*/
        private void buffs_list1_remove(object sender, EventArgs e)
        {
            if (buffs_list1.SelectedIndex != -1)
            {
                int index = Char_Data.skillnamewaiting.IndexOf(buffs_list1.SelectedItem.ToString());
                if (index != -1)
                {
                    Char_Data.skillnamewaiting.RemoveAt(index);
                    Char_Data.skilltipwaiting.RemoveAt(index);
                }
                buffs_list1.Items.RemoveAt(buffs_list1.SelectedIndex);
                buffs_list1.ContextMenuStrip = null;
            }
        }
        private void buffs_list2_remove(object sender, EventArgs e)
        {
            if (buffs_list2.SelectedIndex != -1)
            {
                int index = Char_Data.skillnamewaiting.IndexOf(buffs_list2.SelectedItem.ToString());
                if (index != -1)
                {
                    Char_Data.skillnamewaiting.RemoveAt(index);
                    Char_Data.skilltipwaiting.RemoveAt(index);
                }
                buffs_list2.Items.RemoveAt(buffs_list2.SelectedIndex);
                buffs_list2.ContextMenuStrip = null;
            }
        }
       /* private void buffs_list3_remove(object sender, EventArgs e)
        {
            if (buffs_list3.SelectedIndex != -1)
            {
                int index = Char_Data.skillnamewaiting.IndexOf(buffs_list3.SelectedItem.ToString());
                if (index != -1)
                {
                    Char_Data.skillnamewaiting.RemoveAt(index);
                    Char_Data.skilltipwaiting.RemoveAt(index);
                }
                buffs_list3.Items.RemoveAt(buffs_list3.SelectedIndex);
                buffs_list3.ContextMenuStrip = null;
            }
        }

       private void buffs_list4_remove(object sender, EventArgs e)
        {
            if (buffs_list4.SelectedIndex != -1)
            {
                int index = Char_Data.skillnamewaiting.IndexOf(buffs_list4.SelectedItem.ToString());
                if (index != -1)
                {
                    Char_Data.skillnamewaiting.RemoveAt(index);
                    Char_Data.skilltipwaiting.RemoveAt(index);
                }
                buffs_list4.Items.RemoveAt(buffs_list4.SelectedIndex);
                buffs_list4.ContextMenuStrip = null;
            }
        }*/

        private void trainr_control_ValueChanged(object sender, EventArgs e)
        {
            trainr.Text = trainr_control.Value.ToString();
        }
        private void set_train_xy_Click(object sender, EventArgs e)
        {
            trainx.Text = Character.X.ToString();
            trainy.Text = Character.Y.ToString();
        }

        private void config_button_Click(object sender, EventArgs e)
        {
            Configs.WriteConfigs();
        }


        private void set_second_Click(object sender, EventArgs e)
        {
            if (inventory_list.SelectedIndex != -1)
            {
                Char_Data.s_wep_name = Char_Data.inventorytype[inventory_list.SelectedIndex];
            }
        }
        private void set_first_Click(object sender, EventArgs e)
        {
            if (inventory_list.SelectedIndex != -1)
            {
                Char_Data.f_wep_name = Char_Data.inventorytype[inventory_list.SelectedIndex];
            }
        }

        private void button_dropitem_Click(object sender, EventArgs e)
        {
            if (inventory_list.SelectedIndex != -1)
            {
                Packet packet = new Packet((ushort)WorldServerOpcodes.CLIENT_OPCODES.CLIENT_INVENTORYMOVEMENT, false, enumDestination.Server);
                packet.data.AddBYTE(0x07); // Drop Item
                packet.data.AddBYTE(Char_Data.inventoryslot[inventory_list.SelectedIndex]);
                Globals.ServerPC.SendPacket(packet);
            }
        }

        private void script_start_Click(object sender, EventArgs e)
        {
            script_record_box.Items.Clear();
            BotData.loopaction = "record";
            script_stop.Enabled = true;
            script_start.Enabled = false;
            script_save.Enabled = false;
        }

        private void script_stop_Click(object sender, EventArgs e)
        {
            BotData.loopaction = null;
            script_stop.Enabled = false;
            script_start.Enabled = true;
            script_save.Enabled = true;
        }

        private void script_save_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.SaveFileDialog save = new System.Windows.Forms.SaveFileDialog();
            save.Title = "Save TrainScript";
            save.Filter = "Text File|*.txt";
            save.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (save.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string tmppath = save.FileName.ToString();
                TextWriter tw = new StreamWriter(tmppath);
                for (int i = 0; i < Globals.MainWindow.script_record_box.Items.Count; i++)
                {
                    tw.WriteLine(Globals.MainWindow.script_record_box.Items[i].ToString());
                }
                tw.WriteLine("end");
                tw.Close();
            }
            else
            {
                MessageBox.Show("Unable To Save");
            }
        }

        private void walkscript_select_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog fDialog = new System.Windows.Forms.OpenFileDialog();
            fDialog.Title = "Select WalkScript";
            fDialog.Filter = "Text File|*.txt";
            fDialog.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (fDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string tmppath = fDialog.FileName.ToString();
                BotData.walkscriptpath = tmppath;
                walkscript_path.Text = tmppath;
            }
            else
            {
                MessageBox.Show("Unable To Load");
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private System.Timers.Timer chater;
        private void button1_Click(object sender, EventArgs e)
        {
            chater = new System.Timers.Timer();
            chater.Interval = 2000;
            chater.Elapsed += new System.Timers.ElapsedEventHandler(chater_Elapsed);
            chater.Start();
            chater.Enabled = true;
        }

        void chater_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Action.Chat(textBox3.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                chater.Stop();
                chater.Dispose();
            }
            catch { }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Globals.SecundaryPass.Show();
        }

        private void lb_gold1_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                sro_process.Kill();
            }
            catch { }
            button5.Enabled = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                if (start_button.Text == "Stop Bot")
                {
                    if (lb_mob_hp.Text == "0")
                        timer2.Enabled = true;
                    else
                    {
                        timer2.Enabled = false;
                    }
                }
                else
                {
                }
            }
           
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            start_button.Text = "Start Bot";
            Globals.UpdateLogs("Bot Stoped because no atack after 5 minutes");
            BotData.bot = false;
            BotData.loop = false;
            BotData.returning = 0;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
           // Action.UseDrugSpeed();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Action.UseDrugSpeed();
        }

        private void groupBox16_Enter(object sender, EventArgs e)
        {

        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

       

      

     

             
    }
}
