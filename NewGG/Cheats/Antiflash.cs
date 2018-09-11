using System.Threading;

namespace NewGG.Cheats
{
    class Antiflash
    {
        public bool enabled;
        private Memory mem;
       
        public Antiflash(Memory r)
        {
            mem = r;
           
        }
        public void Core()
        {
            int MyBaseAdr;
            float flash;
            while (true)
            {
                if (CheatData.panorama)
                {
                    MyBaseAdr = mem.Read<int>(CheatData.bClient + Offsets.New.dwLocalPlayer);
                    flash = mem.Read<float>(MyBaseAdr + Offsets.New.m_flFlashMaxAlpha);
                    if (flash > 0)
                    {
                        mem.Write<float>(MyBaseAdr + Offsets.New.m_flFlashMaxAlpha, 0f);
                    }
                }
                else
                {
                    MyBaseAdr = mem.Read<int>(CheatData.bClient + Offsets.Old.dwLocalPlayer);
                    flash = mem.Read<float>(MyBaseAdr + Offsets.Old.m_flFlashMaxAlpha);
                    if (flash > 0)
                    {
                        mem.Write<float>(MyBaseAdr + Offsets.Old.m_flFlashMaxAlpha, 0f);
                    }
                }
                Thread.Sleep(5);
            }
        }
    }
}
