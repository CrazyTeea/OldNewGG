using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace NewGG.Cheats
{
    class Bhop
    {
        public bool enabled;
        public Memory mem;
       


        [DllImport("User32.dll")]
        private static extern short GetAsyncKeyState(System.Windows.Forms.Keys vKey); // Keys enumeration

        [DllImport("User32.dll")]
        private static extern short GetAsyncKeyState(System.Int32 vKey);

        public Bhop(Memory reader)
        {
            mem = reader;
        }
        public void Core()
        {
           
            int jump, MyBaseAdr;
            while (true)
            {
                if (CheatData.panorama)
                {
                    MyBaseAdr = mem.Read<int>(CheatData.bClient + Offsets.New.dwLocalPlayer);
                    jump = mem.Read<int>(CheatData.bClient + Offsets.New.dwForceJump);
                    if (jump == 5)
                    {
                        // MessageBox.Show("jump");
                        while (Convert.ToBoolean(GetAsyncKeyState(0x20) & 0x8000))
                        {

                            // MessageBox.Show("vcicle");
                            int flags = mem.Read<int>(MyBaseAdr + Offsets.New.m_fFlags);
                            if (Convert.ToBoolean(flags & (1 << 0)))
                            {
                                mem.Write((CheatData.bClient + Offsets.New.dwForceJump), 5);
                                Thread.Sleep(1);
                                //   MessageBox.Show("prigaem");
                            }
                            else
                            {
                                mem.Write((CheatData.bClient + Offsets.New.dwForceJump), 4);
                                Thread.Sleep(1);
                            }
                        }
                    }
                }
                else
                {
                    MyBaseAdr = mem.Read<int>(CheatData.bClient + Offsets.Old.dwLocalPlayer);
                    jump = mem.Read<int>(CheatData.bClient + Offsets.Old.dwForceJump);
                    if (jump == 5)
                    {
                        // MessageBox.Show("jump");
                        while (Convert.ToBoolean(GetAsyncKeyState(0x20) & 0x8000))
                        {

                            // MessageBox.Show("vcicle");
                            int flags = mem.Read<int>(MyBaseAdr + Offsets.Old.m_fFlags);
                            if (Convert.ToBoolean(flags & (1 << 0)))
                            {
                                mem.Write((CheatData.bClient + Offsets.Old.dwForceJump), 5);
                                Thread.Sleep(1);
                                //   MessageBox.Show("prigaem");
                            }
                            else
                            {
                                mem.Write((CheatData.bClient + Offsets.Old.dwForceJump), 4);
                                Thread.Sleep(1);
                            }
                        }
                    }
                }
                Thread.Sleep(5);
            }
        }
    }
}
