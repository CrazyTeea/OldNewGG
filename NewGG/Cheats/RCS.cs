using System.Threading;

namespace NewGG.Cheats
{
    class RCS
    {
        private static Memory mem;
        public bool enabled;


        public RCS(Memory mem1)
        {
            mem = mem1;
        }


        public void Core()
        {
            int MybaseAdr, Eng, Bullet;
            Vector3 Mypunch, cangl, startang = Vector3.Zero;
            while (true)
            {
                if (CheatData.panorama)
                { 
                    MybaseAdr = mem.Read<int>(CheatData.bClient + Offsets.New.dwLocalPlayer);
                    Eng = mem.Read<int>(CheatData.bEngine + Offsets.New.dwClientState);
                    Bullet = mem.Read<int>(MybaseAdr + Offsets.New.m_iShotsFired);
                    if (Bullet > 0)
                    {
                        Mypunch = mem.Read<Vector3>(MybaseAdr + Offsets.New.m_aimPunchAngle);
                        cangl = startang - Mypunch * 2f;
                        if (cangl.Y >= 180.0f)
                            cangl.Y = 180.0f;
                        if (cangl.Y <= -180.0f)
                            cangl.Y = -180.0f;
                        if (cangl.X >= 89.0f)
                            cangl.X = 89.0f;
                        if (cangl.X <= -89.0f)
                            cangl.X = 89.0f;
                        Setang(cangl);
                        Thread.Sleep(1);
                    }
                    else
                        startang = mem.Read<Vector3>(Eng + Offsets.New.dwClientState_ViewAngles);
                    
                }
                else
                {
                    MybaseAdr = mem.Read<int>(CheatData.bClient + Offsets.Old.dwLocalPlayer);
                    Eng = mem.Read<int>(CheatData.bEngine + Offsets.Old.dwClientState);
                    Bullet = mem.Read<int>(MybaseAdr + Offsets.Old.m_iShotsFired);
                    if (Bullet > 0)
                    {
                        Mypunch = mem.Read<Vector3>(MybaseAdr + Offsets.Old.m_aimPunchAngle);
                        cangl = startang - Mypunch * 2f;
                        if (cangl.Y >= 180.0f)
                            cangl.Y = 180.0f;
                        if (cangl.Y <= -180.0f)
                            cangl.Y = -180.0f;
                        if (cangl.X >= 89.0f)
                            cangl.X = 89.0f;
                        if (cangl.X <= -89.0f)
                            cangl.X = 89.0f;
                        Setang(cangl);
                        Thread.Sleep(1);
                    }
                    else
                        startang = mem.Read<Vector3>(Eng + Offsets.Old.dwClientState_ViewAngles);

                }
                Thread.Sleep(5);
            }
        }

        private static void Setang(Vector3 src)
        {
            if (CheatData.panorama)
                mem.Write<Vector3>(mem.Read<int>(CheatData.bEngine + Offsets.New.dwClientState) + Offsets.New.dwClientState_ViewAngles, src);
            else
                mem.Write<Vector3>(mem.Read<int>(CheatData.bEngine + Offsets.Old.dwClientState) + Offsets.Old.dwClientState_ViewAngles, src);
        }
    }
}
