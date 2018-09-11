using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using NewGG;
namespace NewGG.Cheats
{
    class Triger
    {
        public bool enabled;
        private Memory mem;
        public bool but = false;
       
      //  private Form1 form1;
       
        public int sec=1;
        [DllImport("user32.dll")]
        static extern void mouse_event(uint dwFlags, int dx, int dy, uint dwData, int dwExtraInfo);
        [DllImport("User32.dll")]
        private static extern short GetAsyncKeyState(System.Int32 vKey);
        

        public Triger(Memory r)
        {
          //  form1 = new Form1();
            mem = r;
        }
        public void Core()
        {
            int MyBaseAdr, eBaseAdr;
            int mteam, eteam, crosid;
            while (true)
            {
                if (CheatData.panorama)
                {
                    MyBaseAdr = mem.Read<int>(CheatData.bClient + Offsets.New.dwLocalPlayer);

                    for (int i = 0; i < 200; i++)
                    {
                        crosid = mem.Read<int>(MyBaseAdr + Offsets.New.m_iCrosshairId);
                        if (crosid > 0 && crosid < 64)
                        {
                            //  form1.uo(player[0].Team.ToString(), "t=" + player[i].Team.ToString() + " com= " + player[i].CrossId.ToString());
                            eBaseAdr = mem.Read<int>(CheatData.bClient + Offsets.New.dwEntityList + (crosid - 1) * Offsets.New.Loop_offset);
                            mteam = mem.Read<int>(MyBaseAdr + Offsets.New.m_iTeamNum);
                            eteam = mem.Read<int>(eBaseAdr + Offsets.New.m_iTeamNum);
                            if (but)
                            {
                                if (GetAsyncKeyState(CheatData.trkey) != 0 && (mteam != eteam))
                                {

                                    Shoot();
                                }

                            }
                            else if (eteam != mteam)
                            {
                                Shoot();
                            }
                        }
                        Thread.Sleep(1);
                    }
                }
                else
                {
                    MyBaseAdr = mem.Read<int>(CheatData.bClient + Offsets.Old.dwLocalPlayer);

                    for (int i = 0; i < 200; i++)
                    {
                        crosid = mem.Read<int>(MyBaseAdr + Offsets.Old.m_iCrosshairId);
                        if (crosid > 0 && crosid < 64)
                        {
                            //  form1.uo(player[0].Team.ToString(), "t=" + player[i].Team.ToString() + " com= " + player[i].CrossId.ToString());
                            eBaseAdr = mem.Read<int>(CheatData.bClient + Offsets.Old.dwEntityList + (crosid - 1) * Offsets.New.Loop_offset);
                            mteam = mem.Read<int>(MyBaseAdr + Offsets.Old.m_iTeamNum);
                            eteam = mem.Read<int>(eBaseAdr + Offsets.Old.m_iTeamNum);
                            if (but)
                            {
                                if (GetAsyncKeyState(CheatData.trkey) != 0 && (mteam != eteam))
                                {

                                    Shoot();
                                }

                            }
                            else if (eteam != mteam)
                            {
                                Shoot();
                            }
                        }
                        Thread.Sleep(1);
                    }
                }
                Thread.Sleep(5);
            }
        }
        void Shoot()
        {
            Thread.Sleep(sec);
            mouse_event(0x2, 0, 0, 0, 0);
            mouse_event(0x4, 0, 0, 0, 0);
        }
    }
}
