using MaterialSkin;
using MaterialSkin.Controls;
using NewGG.Cheats;
using NewGG.Cheats.GUI;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace NewGG
{
    public partial class Form1 : MaterialForm
    {

        Log log;
        private Glow glow;
        private AimBot aimbot;
        private Bhop bhop;
        private Radar radar;
        private Antiflash nofl;
        private Triger tr;
        private RCS rcs;
        int[] color = new int[6];
      //  Plr plr;
        //float fov=0.5f;
        //    WhWindow whwnd;
        DateTime dt;
        DXwindow Xwindow;
        //AllUsers users = new AllUsers();
        //

        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(Keys vKey);

        internal AimBot Aimbot { get => aimbot; set => aimbot = value; }

        public Form1()
        {
            InitializeComponent();


            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey900,
                Primary.BlueGrey900, Primary.BlueGrey900, Accent.Amber700, TextShade.WHITE);
            dt = new DateTime(2018, 7, 6, 19, 48, 00);
            
            //log.Add("Проинцелизирована главная форма");
        }

        private void CloseThread(Thread cheat)
        {
            try
            {
                cheat.Abort();
                cheat = null;
            }
            catch { }
        }

        private void ConnectProc()
        {
            try
            {
                Process csgo = Process.GetProcessesByName("csgo")[0];

                CheatData.mem = new Memory("csgo");

                foreach (ProcessModule module in csgo.Modules)
                {
                  //  MessageBox.Show(module.FileName);
                    if (module.ModuleName == "client.dll")
                    {
                        CheatData.bClient = (int)module.BaseAddress;
                        CheatData.panorama = false;
                        
                    }
                    else if (module.ModuleName == "client_panorama.dll")
                    {
                        CheatData.bClient = (int)module.BaseAddress;
                        CheatData.panorama = true;

                    }
                    if (module.ModuleName == "engine.dll")
                        CheatData.bEngine = (int)module.BaseAddress;


                }

                GameStatus.Text = "Connected";
                GameStatus.ForeColor = Color.FromArgb(255, 160, 0);
                glow = new Glow(CheatData.mem);
                Aimbot = new AimBot(CheatData.mem);
                bhop = new Bhop(CheatData.mem);
                radar = new Radar(CheatData.mem);
                nofl = new Antiflash(CheatData.mem);
                tr = new Triger(CheatData.mem);
                rcs = new RCS(CheatData.mem);
                Xwindow = new DXwindow();
              //  plr = new Plr();
                //CheatData._plrData = new Thread(plr.Core);
                //if (!CheatData._plrData.IsAlive)
                //{

                //    CheatData._plrData.Start();
                //}
              //  log.Add("Прога удачно нашла CSGO");
            }
            catch
            {

                GameStatus.Text = "Disconnected";
                GameStatus.ForeColor = Color.Red;

                CheatData.mem = null;
                Aimbot = null;
                glow = null;
                bhop = null;
                radar = null;
                nofl = null;
                tr = null;
                rcs = null;
                log.Add("Прога не смогла найти игру. Попробуй открыть от имени админа, если не помогло пиши мне в личку https://vk.com/coolman2000");
            }

        }



        private void Form1_Load(object sender, EventArgs e)
        {
         //   Player player = new Player(CheatData.mem);

            tabPage1.BackColor = Color.FromArgb(38, 50, 56);
            tabPage2.BackColor = Color.FromArgb(38, 50, 56);
            tabPage3.BackColor = Color.FromArgb(38, 50, 56);
            materialLabel1.BackColor = Color.FromArgb(38, 50, 56);
            GameStatus.BackColor = Color.FromArgb(38, 50, 56);

            materialFlatButton3.BackColor = Color.FromArgb(38, 50, 56);
            log = new Log();
            ConnectProc();

            log.Add("Открыта главная ыорма");
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey900,
                Primary.BlueGrey900, Primary.BlueGrey900, Accent.Amber700, TextShade.WHITE);
           // if(DateTime.Now >= dt)
          //  {
         //       MessageBox.Show("BY ME!!!");
         //       Application.Exit();
          //  }
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            Process.Start("https://vk.com/coolman2000");
            Process.Start("http://crazytea.xyz/");
            log.Add("Опана!!!!");
        }

        private void TrackBar1_Scroll(object sender, EventArgs e)
        {
            materialLabel5.Text = trackBar1.Value.ToString();
            try
            {
                color[0] = trackBar1.Value;
                if (glow.enabled)
                {

                    glow.UpdateData(color);
                }

                if (Xwindow.created)
                {
                    Xwindow.ColorUpdate(color);
                }
                log.Add("Измнен красный цвет");
            }
            catch (Exception ex)
            {
                log.Add("не удалось изменить красный цвет " + ex.Message);
            }

        }

        private void TrackBar2_Scroll(object sender, EventArgs e)
        {
            materialLabel6.Text = trackBar2.Value.ToString();
            try
            {
                color[1] = trackBar2.Value;

                if (glow.enabled)
                {

                    glow.UpdateData(color);
                }
                else { }

                if (Xwindow.created)
                {
                    Xwindow.ColorUpdate(color);
                }
                log.Add("Измнен зеленый цвет");

            }
            catch (Exception ex)
            {
                log.Add("не удалось изменить зеленый цвет " + ex.Message);
            }
        }

        private void TrackBar3_Scroll(object sender, EventArgs e)
        {
            materialLabel7.Text = trackBar3.Value.ToString();
            try
            {
                color[2] = trackBar3.Value;
                if (glow.enabled)
                {

                    glow.UpdateData(color);
                }
                else { }

                if (Xwindow.created)
                {
                    Xwindow.ColorUpdate(color);
                }
                log.Add("Измнен голубой цвет");
            }
            catch (Exception ex)
            {
                log.Add("не удалось изменить голубой цвет " + ex.Message);
            }
        }

        private void GameStatus_Click(object sender, EventArgs e)
        {
            ConnectProc();           
        }

        private void MaterialCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (materialCheckBox1.Checked)
            {
                try
                {
                    glow.enabled = true;
                    CheatData.GlowT = new Thread(glow.Core);
                    CheatData.GlowT.Start();
                    log.Add("успешно запустилось ГлоуВх");
                }
                catch (Exception ex)
                {
                    log.Add("Не удалось запустить ГлоуВх так как " + ex.Message);
                }

            }
            else
            {
                try
                {
                    CloseThread(CheatData.GlowT);
                    glow.enabled = false;
                    log.Add("успешно закрылось ГлоуВх");
                }
                catch (Exception ex)
                {
                    log.Add("Не удалось закрыть ГлоуВх так как " + ex.Message);
                }
            }

        }

        private void MaterialCheckBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (materialCheckBox3.Checked)
            {
                try
                {
                    CheatData.box = true;

                    if (!Xwindow.created)
                    {
                        //  Xwindow = new DXwindow();
                        Xwindow._Init_();
                        log.Add("успешно открыто окно и включено бокс вх ");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    log.Add("Не удалось открыть откно прорисовки так как " + ex.Message);
                    //  MessageBox.Show("parece que el juego no se está ejecutando   " + ex.Message, "   atención");
                }
            }
            else
            {
                try
                {
                    CheatData.box = false;
                    if (Xwindow.created && !CheatData.lines_wh && !CheatData.hp_wh && !CheatData.Defusing && !CheatData.ranks)
                        Xwindow.Close();
                    log.Add("боксы выключены ");
                }
                catch (Exception ex)
                {
                    log.Add("Не удалось закрыть боксы так как " + ex.Message);
                    // MessageBox.Show("parece que el juego no se está ejecutando" + ex.Message, "atención");
                }
            }
        }

        private void MaterialCheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (materialCheckBox2.Checked)
            {
                try
                {
                    CheatData.lines_wh = true;

                    if (!Xwindow.created)
                    {
                        //  Xwindow = new DXwindow();
                        Xwindow._Init_();
                        log.Add("успешно открыто окно и включено лайн вх ");
                    }

                }
                catch (Exception ex)
                {
                    log.Add("Не удалось открыть линии так как " + ex.Message);
                }
            }
            else
            {
                try
                {
                    CheatData.lines_wh = false;
                    if (Xwindow.created && !CheatData.hp_wh && !CheatData.box && !CheatData.Defusing && !CheatData.ranks)
                        Xwindow.Close();
                    log.Add("успешно выключено лайн вх ");
                }
                catch (Exception ex)
                {
                    log.Add("Не удалось закрыть линии так как " + ex.Message);
                }
            }
        }

        private void MaterialCheckBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (materialCheckBox4.Checked)
            {
                try
                {
                    CheatData.hp_wh = true;

                    if (!Xwindow.created)
                    {
                        //   Xwindow = new DXwindow();
                        Xwindow._Init_();
                        log.Add("успешно открыто окно и включены жизни ");
                    }
                }
                catch (Exception ex)
                {
                    log.Add("Не удалось открыть жизни так как " + ex.Message);
                }
            }
            else
            {
                try
                {
                    CheatData.hp_wh = false;
                    if (Xwindow.created && !CheatData.lines_wh && !CheatData.box && !CheatData.Defusing && !CheatData.ranks)
                        Xwindow.Close();
                    log.Add("успешно выключены жизни ");
                }
                catch (Exception ex)
                {
                    log.Add("Не удалось закрыть жизни так как " + ex.Message);
                }
            }
        }

        private void MaterialCheckBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (materialCheckBox5.Checked)
            {

                try
                {
                    Aimbot.enabled = true;
                    Aimbot.aim = true;
                    CheatData.AimBotT = new Thread(Aimbot.Core);
                    if (!CheatData.AimBotT.IsAlive)
                    {

                        CheatData.AimBotT.Start();
                    }
                }
                catch (Exception)
                {

                }
                materialCheckBox11.Checked = false;
            }
            else
            {
                if (Aimbot.aim)
                {
                    try
                    {
                        Aimbot.enabled = false;
                        CheatData.AimBotT.Abort();
                        CheatData.AimBotT = null;
                    }
                    catch (Exception)
                    {

                    }
                }
            }
        }



        private void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Aimbot.UpdateData(8);
            }

            catch (Exception)
            {

            }
        }

        private void RadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Aimbot.UpdateData(7);
            }
            catch (Exception)
            {

            }
        }

        private void RadioButton3_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Aimbot.UpdateData(5);
            }
            catch (Exception)
            {

            }
        }

        private void RadioButton4_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Aimbot.UpdateData(0);
            }
            catch (Exception)
            {

            }
        }

        private void MaterialCheckBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (materialCheckBox6.Checked)
            {
                try
                {
                    nofl.enabled = true;
                    CheatData.AntiFlashT = new Thread(nofl.Core);
                    CheatData.AntiFlashT.Start();
                }
                catch (Exception)
                {

                }
            }
            else
            {
                try
                {
                    nofl.enabled = false;

                    CheatData.AntiFlashT.Abort();
                    CheatData.AntiFlashT = null;
                }
                catch (Exception)
                {

                }
            }
        }

        private void MaterialCheckBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (materialCheckBox7.Checked)
            {
                try
                {
                    rcs.enabled = true;
                    CheatData._rcs = new Thread(rcs.Core);
                    CheatData._rcs.Start();
                }
                catch (Exception)
                {

                }
            }
            else
            {
                try
                {
                    rcs.enabled = false;
                    CheatData._rcs.Abort();
                    CheatData._rcs = null;
                }
                catch (Exception)
                {

                }
            }
        }//

        private void MaterialCheckBox8_CheckedChanged(object sender, EventArgs e)
        {
            if (materialCheckBox8.Checked)
            {
                try
                {


                    radar.enabled = true;
                    CheatData.RadarT = new Thread(radar.Core);
                    CheatData.RadarT.Start();
                }
                catch (Exception)
                {

                }
            }
            else
            {
                try
                {
                    radar.enabled = false;
                    CheatData.RadarT.Abort();

                    CheatData.RadarT = null;
                }
                catch (Exception)
                {

                }
            }
        }

        private void MaterialCheckBox9_CheckedChanged(object sender, EventArgs e)
        {
            if (materialCheckBox9.Checked)
            {

                try
                {
                    tr.enabled = true;
                    CheatData.TrigerT = new Thread(tr.Core);
                    CheatData.TrigerT.Start();
                    panel1.Visible = true;

                }
                catch (Exception)
                {

                }
            }
            else
            {
                panel1.Visible = false;
                try
                {

                    tr.enabled = false;
                    CheatData.TrigerT.Abort();

                    CheatData.TrigerT = null;
                }
                catch (Exception)
                {

                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseThread(CheatData.AimBotT);
            CloseThread(CheatData.AntiFlashT);
            CloseThread(CheatData.BHopT);
            CloseThread(CheatData.GlowT);
            CloseThread(CheatData.RadarT);
            CloseThread(CheatData.TrigerT);
            CloseThread(CheatData._rcs);

            try
            {
                Xwindow.Close();
            }
            catch { }
            Application.Exit();
            Environment.Exit(0);
        }

        private void MaterialFlatButton1_Click(object sender, EventArgs e)
        {
            if (trackBar4.Value <= 360)
                trackBar4.Value++;
        }

        private void MaterialFlatButton2_Click(object sender, EventArgs e)
        {
            if (trackBar4.Value != 0)
                trackBar4.Value--;
        }

        private void TrackBar4_ValueChanged(object sender, EventArgs e)
        {
            materialLabel9.Text = (trackBar4.Value).ToString();
            try
            {
                Aimbot.Fovinput(trackBar4.Value);
            }
            catch (Exception)
            {
                //
            }
        }

        private void BunnyHop_CheckedChanged(object sender, EventArgs e)
        {
            if (BunnyHop.Checked)
            {
                try
                {
                    bhop.enabled = true;
                    CheatData.BHopT = new Thread(bhop.Core);
                    CheatData.BHopT.Start();
                    // MessageBox.Show("atención", "uiiiiiiii");
                }
                catch { }
            }
            else
            {
                try
                {
                    tr.enabled = false;
                    CheatData.BHopT.Abort();

                    CheatData.BHopT = null;
                }
                catch { }
            }
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)

            {
                case 0:
                    {
                        CheatData.key = 0x12;
                        break;
                    }
                case 1:
                    {
                        CheatData.key = 0x10;
                        break;
                    }
                case 2:
                    {
                        CheatData.key = 0x2;

                        break;
                    }
                case 3:
                    {
                        CheatData.key = 0x1;

                        break;
                    }
                case 4:
                    {
                        CheatData.key = 0x4;

                        break;
                    }
                default:
                    {
                        CheatData.key = 0x12;
                        break;
                    }
            }
        }

        private void MaterialCheckBox10_CheckedChanged(object sender, EventArgs e)
        {
            if (materialCheckBox10.Checked)
            {
                try
                {
                    CheatData.ranks = true;

                    if (!Xwindow.created)
                    {
                        // Xwindow = new DXwindow();
                        Xwindow._Init_();
                    }


                }
                catch (Exception)
                {

                }
            }
            else
            {
                try
                {
                    CheatData.ranks = false;
                    //if (whwnd.Created && !CheatData.lines_wh && !CheatData.box && !CheatData.Defusing)
                    //    whwnd.Close();
                    if (Xwindow.created && !CheatData.lines_wh && !CheatData.box && !CheatData.Defusing)
                        Xwindow.Close();
                }
                catch (Exception)
                {

                }
            }
        }

        private void MaterialFlatButton3_Click(object sender, EventArgs e)
        {
            if (!log.kek)
            {
                log.kek = true;
                log.Show();
                log.Add("Открыта Консоль");
                log.Location = new Point(Width + 150, Location.Y);
            }
        }

        private void MaterialCheckBox11_CheckedChanged(object sender, EventArgs e)
        {
            if (materialCheckBox11.Checked)
            {

                try
                {
                    Aimbot.enabled = true;
                    Aimbot.aim = false;
                    //materialCheckBox5.Checked = false;
                    CheatData.AimBotT = new Thread(Aimbot.Core);
                    if (!CheatData.AimBotT.IsAlive)
                    {

                        CheatData.AimBotT.Start();
                    }

                }
                catch (Exception)
                {

                }
                materialCheckBox5.Checked = false;
            }
            else
            {
                if (!Aimbot.aim)
                {
                    try
                    {
                        Aimbot.enabled = false;
                        CheatData.AimBotT.Abort();
                        CheatData.AimBotT = null;
                    }
                    catch (Exception)
                    {

                    }
                }
            }

        }

        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
          //  MessageBox.Show("sf");
            switch (comboBox2.SelectedIndex)

            {
                case 0:
                    {
                        CheatData.trkey = 0x12;
                        break;
                    }
                case 1:
                    {
                        CheatData.trkey = 0x10;
                        break;
                    }
                case 2:
                    {
                        CheatData.trkey = 0x2;

                        break;
                    }
                case 3:
                    {
                        CheatData.trkey = 0x1;

                        break;
                    }
                case 4:
                    {
                        CheatData.trkey = 0x4;

                        break;
                    }
                default:
                    {
                        CheatData.trkey = 0x12;
                        break;
                    }
            }
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkBox1.Checked)
                    tr.but = true;
                else
                    tr.but = false;
            }
            catch { }
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                tr.sec = Convert.ToInt32(textBox1.Text);
            }
            catch { }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            short key = GetAsyncKeyState(Keys.F3);
            if ((key & 0x8000) > 0)
            {
                if (!materialCheckBox9.Checked)
                {
                    //gfhf
                    Console.Beep(500, 500);
                    materialCheckBox9.Checked = true;
                }
                else
                {
                    materialCheckBox9.Checked = false;
                    Console.Beep(900, 500);
                }

            }
        }


    }
    
}
