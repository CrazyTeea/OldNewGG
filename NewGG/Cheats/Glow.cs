using System.Threading;

namespace NewGG.Cheats
{
    struct GlowStruct
    {
        public float r;
        public float g;
        public float b;
        public float a;
        public bool rwo;
        public bool rwuo;
    }

    class Glow
    {
        private static Memory mem;
        private GlowStruct ent;

       

        public bool enabled = false;

        public Glow(Memory Reader)
        {
            mem = Reader;
            ent = new GlowStruct()
            {
                r = 0 / 255f,
                g = 0 / 255f,
                b = 0 / 255f,
                a = 255 / 255f,
                rwo = true,
                rwuo = false
            };
           

        }
        public void UpdateData(int[] e)
        {

            ent.r = e[0];
            ent.g = e[1];
            ent.b = e[2];

        }
        private static void DrawValve(int GlowInd, GlowStruct col)
        {

            int GlowObj = 0x0;
            // IntPtr pntr;
            if (CheatData.panorama)
                GlowObj = mem.Read<int>(CheatData.bClient + Offsets.New.dwGlowObjectManager);
            else
                GlowObj = mem.Read<int>(CheatData.bClient + Offsets.Old.dwGlowObjectManager);
            mem.Write((GlowObj + ((GlowInd * 0x38) + 4)), col.r);
            mem.Write((GlowObj + ((GlowInd * 0x38) + 8)), col.g);
            mem.Write((GlowObj + ((GlowInd * 0x38) + 12)), col.b);
            mem.Write((GlowObj + ((GlowInd * 0x38) + 0x10)), 255f);
            mem.Write((GlowObj + ((GlowInd * 0x38) + 0x24)), true);
            mem.Write((GlowObj + ((GlowInd * 0x38) + 0x25)), false);


        }
        public void Core()
        {
            int MyBaseAdr, eBaseAdr;
            int mteam, eteam, hp, glowin;
            bool dormant;
            while (true)
            {
                if (CheatData.panorama)
                {
                    for (int i = 1; i < 200; i++)
                    {
                        eBaseAdr = mem.Read<int>(CheatData.bClient + Offsets.New.dwEntityList + (i * Offsets.New.Loop_offset));
                        MyBaseAdr = mem.Read<int>(CheatData.bClient + Offsets.New.dwLocalPlayer);
                        eteam = mem.Read<int>(eBaseAdr + Offsets.New.m_iTeamNum);
                        mteam = mem.Read<int>(MyBaseAdr + Offsets.New.m_iTeamNum);
                        hp = mem.Read<int>(eBaseAdr + Offsets.New.m_iHealth);
                        dormant = mem.Read<bool>(eBaseAdr + Offsets.New.m_bDormant);
                        glowin = mem.Read<int>(eBaseAdr + Offsets.New.m_iGlowIndex);
                        if (MyBaseAdr == 0x0) { continue; }

                        if (dormant) { continue; }

                        if (hp == 0) { continue; }

                        if (mteam != eteam)
                        {

                            DrawValve(glowin, ent);
                            Thread.Sleep(1);
                        }
                        // Thread.Sleep(5);
                    }
                }
                else
                {
                    for (int i = 1; i < 200; i++)
                    {
                        eBaseAdr = mem.Read<int>(CheatData.bClient + Offsets.Old.dwEntityList + (i * Offsets.New.Loop_offset));
                        MyBaseAdr = mem.Read<int>(CheatData.bClient + Offsets.Old.dwLocalPlayer);
                        eteam = mem.Read<int>(eBaseAdr + Offsets.Old.m_iTeamNum);
                        mteam = mem.Read<int>(MyBaseAdr + Offsets.Old.m_iTeamNum);
                        hp = mem.Read<int>(eBaseAdr + Offsets.Old.m_iHealth);
                        dormant = mem.Read<bool>(eBaseAdr + Offsets.New.m_bDormant);
                        glowin = mem.Read<int>(eBaseAdr + Offsets.Old.m_iGlowIndex);
                        if (MyBaseAdr == 0x0) { continue; }

                        if (dormant) { continue; }

                        if (hp == 0) { continue; }

                        if (mteam != eteam)
                        {

                            DrawValve(glowin, ent);
                            Thread.Sleep(1);
                        }
                        // Thread.Sleep(5);
                    }
                }
                Thread.Sleep(5);
            }
        }

    }
}
