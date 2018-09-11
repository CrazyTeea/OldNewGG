using System;


namespace NewGG.Cheats
{
    struct Vector3
    {
        public float X;
        public float Y;
        public float Z;

        public static Vector3 Zero => new Vector3(0, 0, 0);

        public Vector3(int X, int Y, int Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }
        public static Vector3 operator +(Vector3 a, Vector3 b) => new Vector3
        {
            X = a.X + b.X,
            Y = a.Y + b.Y,
            Z = a.Z + b.Z
        };
        public static Vector3 operator -(Vector3 a, Vector3 b) => new Vector3
        {
            X = a.X - b.X,
            Y = a.Y - b.Y,
            Z = a.Z - b.Z
        };


        public static float Distance(Vector3 ot, Vector3 to) => 
            (float)Math.Sqrt(Math.Pow(to.X - ot.X, 2) + Math.Pow(to.Y - ot.Y, 2) + Math.Pow(to.Z - ot.Z, 2));
        public static Vector3 operator *(Vector3 left, float right) => new Vector3
        {
            X = left.X * right,
            Y = left.Y * right,
            Z = left.Z * right,
        };
    }


    struct Vector2
    {
        public float X;
        public float Y;
        public Vector2(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }
        public Vector2(float value)
        {
            X = value;
            Y = value;
        }
        public static Vector2 Zero => new Vector2(0, 0);
        public static Vector2 One => new Vector2(1, 1);
        public static Vector2 UnitX => new Vector2(0, 1);
        public static Vector2 UnitY => new Vector2(1, 0);


    }

}
