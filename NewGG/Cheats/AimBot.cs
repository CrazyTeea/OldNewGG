using System;
//using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace NewGG.Cheats
{
    class AimBot
    {

        public bool enabled;
        public bool aim;

        float fov = 1.3f;
        int key1 = 0x1;
        float m_Best = 999.9f;
        
        [DllImport("User32.dll")]
        private static extern short GetAsyncKeyState(System.Windows.Forms.Keys vKey); // Keys enumeration

        [DllImport("User32.dll")]
        private static extern short GetAsyncKeyState(System.Int32 vKey);

        private static Memory mem;
        private int bonei = 8;

        private static bool IsKeyPushedDown(System.Windows.Forms.Keys vKey)
        {
            return 0 != (GetAsyncKeyState((int)vKey) & 0x8000);
        }

        public AimBot(Memory Reader)
        {
            mem = Reader;
        }
        public void UpdateData(int bone)
        {
            bonei = bone;
        }
        public void Fovinput(float fovi)
        {
            fov = fovi;
        }
        public void Aimkey(Keys k)
        {
            key1 = Convert.ToInt32(k);
        }

        private static Vector3 CalcLocalPos(int playerbase)
        {
            if (CheatData.panorama)
                return mem.Read<Vector3>(playerbase + Offsets.New.m_vecOrigin) + mem.Read<Vector3>(playerbase + Offsets.New.m_vecViewOffset);
            else
                return mem.Read<Vector3>(playerbase + Offsets.Old.m_vecOrigin) + mem.Read<Vector3>(playerbase + Offsets.Old.m_vecViewOffset);
        }

        private static Vector3 CalcEnemyHead(int entity, int bone)
        {
            int bonematrix =0x0;
            if (CheatData.panorama)
                bonematrix = mem.Read<int>(entity + Offsets.New.m_dwBoneMatrix);
            else
                bonematrix = mem.Read<int>(entity + Offsets.Old.m_dwBoneMatrix);
            //ReadProcessMemory(hProcess, (LPCVOID)(entity + m_dwBoneMatrix), &bonematrix, sizeof(bonematrix), NULL);
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

        private static void CalcAngle(ref Vector3 src, ref Vector3 dst, out Vector3 vAngle) // LocalEyePos and EnemyHeadPos
        {
            double[] delta = new double[3] { (src.X - dst.X), (src.Y - dst.Y), (src.Z - dst.Z) };
            double hyp = Math.Sqrt(Math.Pow(delta[0], 2) + Math.Pow(delta[1], 2));
            float[] angles = new float[3];

            angles[0] = (float)(Math.Atan(delta[2] / hyp) * 57.295779513082); //pitch (Verticaal)
            angles[1] = (float)(Math.Atan(delta[1] / delta[0]) * 57.295779513082f); //yaw (Horizontaal)
            angles[2] = 0.0f;

            if (delta[0] >= 0.0)
            {
                angles[1] += 180.0f;
            }
            //hideMyCode();
            if (angles[0] != angles[0])
            {
                angles[0] = 0.0f;
            }

            if (angles[1] != angles[1])
            {
                angles[1] = 0.0f;
            }
            if (angles[2] != angles[2])
            {
                angles[2] = 0.0f;
            }
            vAngle.X = angles[0]; //Verticaal (Pitch)
            vAngle.Y = angles[1]; //Horizontaal (Yaw)
            vAngle.Z = angles[2];
        }
        private Vector3 ClampAngles(Vector3 AngleToNormalize)
        {
            Vector3 vec = AngleToNormalize;
            if (vec.X > 89.0f && vec.X <= 180.0f)
            {
                vec.X = 89.0f;
            }
            if (vec.X > 180.0f)
            {
                vec.X -= 360.0f;
            }
            if (vec.X < -89.0f)
            {
                vec.X = -89.0f;
            }
            //	hideMyCode();
            if (vec.Y > 180.0f)
            {
                vec.Y -= 360.0f;
            }
            if (vec.Y < -180.0f)
            {
                vec.Y += 360.0f;
            }
            if (vec.Z != 0)
            {
                vec.Z = 0.0f;
            }
            return vec;
        }

        private float Fov(Vector3 Angle, Vector3 PlayerAngle)
        {
            return (float)Math.Sqrt(Math.Pow(Angle.X - PlayerAngle.X, 2) + Math.Pow(Angle.Y - PlayerAngle.Y, 2));
        }

        private void Aim()
        {
            int MyBaseAdr, eBaseAdr, angpntr;
            int mteam, eteam, hp;
            bool dormant;
            Vector3 LocalHeadPos, EnemyBonePos, AngleClamped;
            Vector3 CurrentViewAngle;
            while (aim)
            {
                if (CheatData.panorama)
                {
                    angpntr = mem.Read<int>(CheatData.bEngine + Offsets.New.dwClientState);
                    for (int i = 0; i < 200; i++)
                    {
                        MyBaseAdr = mem.Read<int>(CheatData.bClient + Offsets.New.dwLocalPlayer);
                        mteam = mem.Read<int>(MyBaseAdr + Offsets.New.m_iTeamNum);
                        eBaseAdr = mem.Read<int>(CheatData.bClient + Offsets.New.dwEntityList + (i * Offsets.New.Loop_offset));
                        eteam = mem.Read<int>(eBaseAdr + Offsets.New.m_iTeamNum);
                        dormant = mem.Read<bool>(eBaseAdr + Offsets.New.m_bDormant);
                        hp = mem.Read<int>(eBaseAdr + Offsets.New.m_iHealth);
                        if (eBaseAdr == 0x0) { continue; }

                        if (dormant) { continue; }

                        if (hp < 1) { continue; }
                        //cout << "1" << endl;
                        if (mteam == eteam) { continue; }
                        LocalHeadPos = CalcLocalPos(MyBaseAdr);
                        EnemyBonePos = CalcEnemyHead(eBaseAdr, bonei);
                        if (EnemyBonePos.X == 0x0 && EnemyBonePos.Y == 0x0 && EnemyBonePos.Z == 0x0)
                        {
                            continue;
                            //cout << "1" << endl;
                        }
                        //	EnemyBonePos.z -= 1;
                        CalcAngle(ref LocalHeadPos, ref EnemyBonePos, out Vector3 Angle);
                        AngleClamped = ClampAngles(Angle);
                        CurrentViewAngle = mem.Read<Vector3>(angpntr + Offsets.New.dwClientState_ViewAngles);
                        float FOV = Fov(AngleClamped, CurrentViewAngle);
                        if (GetAsyncKeyState(CheatData.key) != 0 && FOV < fov)
                        {
                            //cout << "1" << endl;
                            mem.Write<Vector3>(angpntr + Offsets.New.dwClientState_ViewAngles, AngleClamped);
                            Thread.Sleep(1);
                            //WriteProcessMemory(hProcess, (LPVOID)(angpntr + dw_m_angRotation), &AngleClamped, sizeof(AngleClamped), NULL);
                        }

                        Thread.Sleep(1);
                        //
                    }
                }
                else
                {
                    angpntr = mem.Read<int>(CheatData.bEngine + Offsets.Old.dwClientState);
                    for (int i = 0; i < 200; i++)
                    {
                        MyBaseAdr = mem.Read<int>(CheatData.bClient + Offsets.Old.dwLocalPlayer);
                        mteam = mem.Read<int>(MyBaseAdr + Offsets.Old.m_iTeamNum);
                        eBaseAdr = mem.Read<int>(CheatData.bClient + Offsets.Old.dwEntityList + (i * Offsets.New.Loop_offset));
                        eteam = mem.Read<int>(eBaseAdr + Offsets.Old.m_iTeamNum);
                        dormant = mem.Read<bool>(eBaseAdr + Offsets.New.m_bDormant);
                        hp = mem.Read<int>(eBaseAdr + Offsets.Old.m_iHealth);
                        if (eBaseAdr == 0x0) { continue; }

                        if (dormant) { continue; }

                        if (hp < 1) { continue; }
                        //cout << "1" << endl;
                        if (mteam == eteam) { continue; }
                        LocalHeadPos = CalcLocalPos(MyBaseAdr);
                        EnemyBonePos = CalcEnemyHead(eBaseAdr, bonei);
                        if (EnemyBonePos.X == 0x0 && EnemyBonePos.Y == 0x0 && EnemyBonePos.Z == 0x0)
                        {
                            continue;
                            //cout << "1" << endl;
                        }
                        //	EnemyBonePos.z -= 1;
                        CalcAngle(ref LocalHeadPos, ref EnemyBonePos, out Vector3 Angle);
                        AngleClamped = ClampAngles(Angle);
                        CurrentViewAngle = mem.Read<Vector3>(angpntr + Offsets.New.dwClientState_ViewAngles);
                        float FOV = Fov(AngleClamped, CurrentViewAngle);
                        if (GetAsyncKeyState(CheatData.key) != 0 && FOV < fov)
                        {
                            //cout << "1" << endl;
                            mem.Write<Vector3>(angpntr + Offsets.New.dwClientState_ViewAngles, AngleClamped);
                            Thread.Sleep(1);
                            //WriteProcessMemory(hProcess, (LPVOID)(angpntr + dw_m_angRotation), &AngleClamped, sizeof(AngleClamped), NULL);
                        }

                        Thread.Sleep(1);
                        //
                    }
                }
                Thread.Sleep(5);
            }
        }
       


        public void Core()
        {
            
                Aim();
            
        }

        public void NormalizeAngle(ref Vector3 src)
        {
            if (src.X < -89.0f)
                src.X = -89.0f;
            else if (src.X > 89.0f)
                src.X = 89.0f;
            if (src.Y > 180.0f)
                src.Y -= 360.0f;
            else if (src.Y < -180.0f)
                src.Y += 360.0f;
            if (src.Z != 0)
                src.Z = 0;


        }
        
        
       
        
    }
}
