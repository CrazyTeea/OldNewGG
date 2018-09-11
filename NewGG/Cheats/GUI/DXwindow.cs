using System;
using System.Runtime.InteropServices;
using System.Drawing;
using Overlay;
using System.Threading;
using System.Numerics;
using System.Windows.Forms;

namespace NewGG.Cheats.GUI
{
    class DXwindow
    {
        public struct RECT
        {
            public int left, top, right, bottom;
        }
        public struct ScreenVector
        {
            public float X;
            public float Y;
            public bool Result;
        }
        OverlayWindow WHwindow;
        private Memory mem;

        private RECT rect;


        public bool created = false;


        int MyBaseAdr, eBaseAdr;
        private int mteam, eteam, hp;
        private Vector3 coords, mycoords, head;
        private bool dormant;
        private Matrix4x4 VMatrix;
        private int Gres;
        private int rank;
        private Font myFont = new Font("Arial", 12);
        public int mt, et;
        private SharpDX.DirectWrite.Factory factory = new SharpDX.DirectWrite.Factory();

        int[] pen = new int[6];
        Thread tr;

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        private const string WINDOW_NAME = "Counter-Strike: Global Offensive";
        private IntPtr handle = FindWindow(null, WINDOW_NAME);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);


        Direct2DRenderer gfx;

        public DXwindow()
        {

        }
        private Vector3 CalcEnemyHead(int entity, int bone)
        {
            
            int bonematrix;
                if (CheatData.panorama)
                {
                    bonematrix = mem.Read<int>(entity + Offsets.New.m_dwBoneMatrix);
                }
                else
                    bonematrix = mem.Read<int>(entity + Offsets.Old.m_dwBoneMatrix);
                if (bonematrix == 0x0)
            {
                return new Vector3(0, 0, 0);
            }
            return new Vector3
            {
                X = mem.Read<float>(bonematrix + 0x30 * bone + 0x0C),
                Y = mem.Read<float>(bonematrix + 0x30 * bone + 0x1C),
                Z = mem.Read<float>(bonematrix + 0x30 * bone + 0x2C)
            };

    }
        private ScreenVector WorldToScreen(float x, float y, float z)
        {
            float w;
            float sx;
            float sy;

            sx = VMatrix.M11 * x +
                VMatrix.M12 * y +
                VMatrix.M13 * z +
                VMatrix.M14;

            sy = VMatrix.M21 * x +
                VMatrix.M22 * y +
                VMatrix.M23 * z +
                VMatrix.M24;


            w = VMatrix.M41 * x +
                VMatrix.M42 * y +
                VMatrix.M43 * z +
                VMatrix.M44;

            if (w < 0.01f)
                return new ScreenVector()
                {
                    Result = false,
                    X = 0,
                    Y = 0
                };

            float invw = 1f / w;
            sx *= invw;
            sy *= invw;

            int width = WHwindow.Width;
            int height = WHwindow.Height;

            float xt = width / 2;
            float yt = height / 2;

            xt += 0.5f * sx * width + 0.5f;
            yt -= 0.5f * sy * height + 0.5f;

            sx = xt;
            sy = yt;


            return new ScreenVector()
            {
                Result = true,
                X = sx,
                Y = sy
            };
        }
        public void _Init_()
        {
            mem = CheatData.mem;
            created = true;
            GetWindowRect(handle, out rect);
            WHwindow = new OverlayWindow(rect.left, rect.top, rect.right, rect.bottom);
          //  WHwindow = new OverlayWindow();
            //WHwindow.MoveWindow( rect.right-rect.top, rect.bottom);
            var rendererOptions = new Direct2DRendererOptions()
            {
                AntiAliasing = true,
                Hwnd = WHwindow.WindowHandle,
                MeasureFps = true,
                VSync = false
            };
            gfx = new Direct2DRenderer(rendererOptions);
            tr = new Thread(Loop);
            tr.Start();
        }
        public string[] Ranks = new string[] {
         "NO_RANK",
        "Silver 1",
        "Silver 2",
        "Silver 3",
        "Silver 4",
        "Silver 5",
        "Silver 6",
        "Gold Nova",
        "Gold Nova 2",
        "Gold Nova 3",
        "Gold Nova 4",
        "Gold Nova 3",
        "Master Guardian 1",
        "Master Guardian 2",
        "Master Guardian 3",
        "DMG",
        "Legendary Eagle",
        "LEM",
        "Supreme",
        "Global Elite",
    };

        public void ColorUpdate(int[] e) => pen = e;
        char[] name = new char[50];
        private void Loop()
        {
            while (true)
            {
               // GC.Collect();
                gfx.BeginScene();
                gfx.ClearScene();

                if (CheatData.panorama)
                {


                    for (int i = 0; i < 100; i++)
                    {
                        MyBaseAdr = mem.Read<int>(CheatData.bClient + Offsets.New.dwLocalPlayer);
                        mteam = mem.Read<int>(MyBaseAdr + Offsets.New.m_iTeamNum);
                        mycoords = mem.Read<Vector3>(MyBaseAdr + Offsets.New.m_vecOrigin);
                        eBaseAdr = mem.Read<int>(CheatData.bClient + Offsets.New.dwEntityList + (i * Offsets.New.Loop_offset));
                        eteam = mem.Read<int>(eBaseAdr + Offsets.New.m_iTeamNum);
                        dormant = mem.Read<bool>(eBaseAdr + Offsets.New.m_bDormant);
                        // glowind = mem.Read<int>(eBaseAdr + Offsets.m_iGlowIndex);
                        hp = mem.Read<int>(eBaseAdr + Offsets.New.m_iHealth);
                        coords = mem.Read<Vector3>(eBaseAdr + Offsets.New.m_vecOrigin);
                        head = CalcEnemyHead(eBaseAdr, 8);
                        Gres = mem.Read<int>(CheatData.bClient + Offsets.New.dwPlayerResource);
                        rank = mem.Read<int>(Gres + Offsets.New.m_iCompetitiveRanking + i * 4);
                        int radar = mem.Read<int>(CheatData.bClient + Offsets.New.dwRadarBase);
                        name = mem.Read<char>(radar + (i * 50) + 0x204, 50);
                        string n = string.Empty;
                        for (int j = 0; j < 49; j++)
                        {
                            n += name[j];
                        }
                        //
                        //    angles = mem.Read<Vector2>(eBaseAdr + Offsets.m_angEyeAngles);

                        mt = mteam;
                        et = eteam;

                        if (MyBaseAdr == 0x0) { continue; }


                        VMatrix = mem.Read<Matrix4x4>(CheatData.bClient + Offsets.New.dwViewMatrix);
                        ScreenVector sv = WorldToScreen(coords.X, coords.Y, coords.Z);
                        ScreenVector svh = WorldToScreen(head.X, head.Y, head.Z);
                        if (dormant) { continue; }
                        if (hp == 0) { continue; }

                        if (!sv.Result)
                        {
                            continue;
                        }
                        int ex = (int)sv.X;
                        int ey = (int)sv.Y;
                        int sx = WHwindow.Width / 2;
                        int sy = WHwindow.Height;

                        float h = (svh.Y - sv.Y);
                        float w = 18500 / (float)Vector3.Distance(mycoords, coords);
                        float x = (int)(sv.X - w / 2);
                        float y = (sv.Y);

                        if ((eteam != mteam) && mteam != 1)

                        {
                            if (CheatData.lines_wh)
                                gfx.DrawLine(sx, sy, ex, ey, 2, new Direct2DColor(pen[0], pen[1], pen[2]));
                            if (CheatData.hp_wh)

                                gfx.DrawHorizontalBar(hp, x, y, 2, h, 1, new Direct2DColor(255, 0, 0), new Direct2DColor(0, 0, 0));

                            if (CheatData.box)
                            {
                                double d = Vector3.Distance(mycoords, coords);
                                if (d < 1f) continue;
                                Drawbox(sv.X, sv.Y, (int)d);
                            }
                            if (CheatData.ranks)
                            {
                                gfx.DrawText(n, svh.X, svh.Y, new Direct2DFont(factory, "Arial", 12), new Direct2DColor(255, 0, 0));


                                gfx.DrawText(Ranks[rank], ex, ey, new Direct2DFont(factory, "Arial", 12), new Direct2DColor(255, 0, 0));
                            }

                        }

                    }
                }
                else
                {
                    for (int i = 0; i < 100; i++)
                    {
                        MyBaseAdr = mem.Read<int>(CheatData.bClient + Offsets.Old.dwLocalPlayer);
                        mteam = mem.Read<int>(MyBaseAdr + Offsets.Old.m_iTeamNum);
                        mycoords = mem.Read<Vector3>(MyBaseAdr + Offsets.Old.m_vecOrigin);
                        eBaseAdr = mem.Read<int>(CheatData.bClient + Offsets.Old.dwEntityList + (i * Offsets.New.Loop_offset));
                        eteam = mem.Read<int>(eBaseAdr + Offsets.Old.m_iTeamNum);
                        dormant = mem.Read<bool>(eBaseAdr + Offsets.New.m_bDormant);
                        // glowind = mem.Read<int>(eBaseAdr + Offsets.m_iGlowIndex);
                        hp = mem.Read<int>(eBaseAdr + Offsets.Old.m_iHealth);
                        coords = mem.Read<Vector3>(eBaseAdr + Offsets.Old.m_vecOrigin);
                        head = CalcEnemyHead(eBaseAdr, 8);
                        Gres = mem.Read<int>(CheatData.bClient + Offsets.Old.dwPlayerResource);
                        rank = mem.Read<int>(Gres + Offsets.Old.m_iCompetitiveRanking + i * 4);
                        int radar = mem.Read<int>(CheatData.bClient + Offsets.Old.dwRadarBase);
                        name = mem.Read<char>(radar + (i * 50) + 0x204, 50);
                        string n = string.Empty;
                        for (int j = 0; j < 49; j++)
                        {
                            n += name[j];
                        }
                        //
                        //    angles = mem.Read<Vector2>(eBaseAdr + Offsets.m_angEyeAngles);

                        mt = mteam;
                        et = eteam;

                        if (MyBaseAdr == 0x0) { continue; }


                        VMatrix = mem.Read<Matrix4x4>(CheatData.bClient + Offsets.Old.dwViewMatrix);
                        ScreenVector sv = WorldToScreen(coords.X, coords.Y, coords.Z);
                        ScreenVector svh = WorldToScreen(head.X, head.Y, head.Z);
                        if (dormant) { continue; }
                        if (hp == 0) { continue; }

                        if (!sv.Result)
                        {
                            continue;
                        }
                        int ex = (int)sv.X;
                        int ey = (int)sv.Y;
                        int sx = WHwindow.Width / 2;
                        int sy = WHwindow.Height;

                        float h = (svh.Y - sv.Y);
                        float w = 18500 / (float)Vector3.Distance(mycoords, coords);
                        float x = (int)(sv.X - w / 2);
                        float y = (sv.Y);

                        if ((eteam != mteam) && mteam != 1)

                        {
                            if (CheatData.lines_wh)
                                gfx.DrawLine(sx, sy, ex, ey, 2, new Direct2DColor(pen[0], pen[1], pen[2]));
                            if (CheatData.hp_wh)

                                gfx.DrawHorizontalBar(hp, x, y, 2, h, 1, new Direct2DColor(255, 0, 0), new Direct2DColor(0, 0, 0));

                            if (CheatData.box)
                            {
                                double d = Vector3.Distance(mycoords, coords);
                                if (d < 1f) continue;
                                Drawbox(sv.X, sv.Y, (int)d);
                            }
                            if (CheatData.ranks)
                            {
                                gfx.DrawText(n, svh.X, svh.Y, new Direct2DFont(factory, "Arial", 12), new Direct2DColor(255, 0, 0));


                                gfx.DrawText(Ranks[rank], ex, ey, new Direct2DFont(factory, "Arial", 12), new Direct2DColor(255, 0, 0));
                            }

                        }

                    }
                }


                gfx.EndScene();
                
                //Thread.Sleep(5);
            }
            
        }
        private void Drawbox(float x, float y, int dist)
        {
            float h, w;

            int flags = 0;
                if(CheatData.panorama)
                flags = mem.Read<int>(eBaseAdr + Offsets.New.m_fFlags);
                else
                flags = mem.Read<int>(eBaseAdr + Offsets.Old.m_fFlags);
            if (flags == 263)
            {
                h = 18500 / dist;
            }
            w = 15000 / dist;
            h = 50000 / dist;
           gfx.DrawRectangle( x - (w / 2), y - h, w, h,2,new Direct2DColor (pen[0], pen[1], pen[2]));
        }
        public void Close()
        {
            created = false;
            tr.Abort();

            WHwindow.HideWindow();
            gfx = null;
            tr = null;
        }
    }
}
