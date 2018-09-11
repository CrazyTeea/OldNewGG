using System.Threading;
using NewGG.Cheats;

namespace NewGG
{
    class CheatData
    {
        public static int bClient;
        public static int bEngine;
        public static Memory mem;
        public static Thread GlowT;
        public static Thread AimBotT;
        public static Thread BHopT;
        public static Thread AntiFlashT;
        public static Thread RadarT;
        public static Thread TrigerT;
        public static Thread _rcs;
        public static Thread _plrData;
      //  public static Player[] _plr = new Player[200]; 
        public static bool freind_wh = false;
        public static bool lines_wh = false;
        public static bool ranks = false;
        public static bool hp_wh = false;
        public static bool box = false;
        public static bool Defusing = false;
        public static bool panorama = true;
        public static int key = 0x12;
        public static int trkey = 0x12;
    }
}
