using System.Threading;

namespace NewGG.Cheats
{
    class Radar
    {
        public bool enabled;
        public static Memory mem;
      

        public Radar(Memory reader)
        {
            mem = reader;
           
        }
        public void Core()
        {
            int eBaseAdr; bool spot;
            while (true)
            {
                if (CheatData.panorama)
                {

                    for (int i = 1; i < 200; i++)
                    {
                        eBaseAdr = mem.Read<int>(CheatData.bClient + Offsets.New.dwEntityList + (i * Offsets.New.Loop_offset));
                        spot = mem.Read<bool>(eBaseAdr + Offsets.New.m_bSpotted);
                        if (!spot)
                        {
                            mem.Write(eBaseAdr + Offsets.New.m_bSpotted, true);
                            Thread.Sleep(1);
                        }
                        Thread.Sleep(1);
                    }
                }
                else
                {
                    for (int i = 1; i < 200; i++)
                    {
                        eBaseAdr = mem.Read<int>(CheatData.bClient + Offsets.Old.dwEntityList + (i * Offsets.New.Loop_offset));
                        spot = mem.Read<bool>(eBaseAdr + Offsets.Old.m_bSpotted);
                        if (!spot)
                        {
                            mem.Write(eBaseAdr + Offsets.Old.m_bSpotted, true);
                            Thread.Sleep(1);
                        }
                        Thread.Sleep(1);
                    }
                }
                Thread.Sleep(5);
            }
        }
    }
}
