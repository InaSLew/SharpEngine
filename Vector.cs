using System;

namespace SharpEngine
{
    public struct Vector
    {
        internal float x, y, z;
        
        public static Vector Forward => new Vector(0, 1);
        public static Vector Backward => new Vector(0, -1);
        public static Vector Down => new(0, -1);
        public static Vector Left => new Vector(-1, 0);
        public static Vector Right => new Vector(1, 0);
        public static Vector Zero => new Vector(0, 0);

        public Vector(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector(float x, float y)
        {
            this.x = x;
            this.y = y;
            this.z = 0;
        }

        public static Vector operator *(Vector v, float f) => new (v.x * f, v.y * f, v.z * f);
        // missing overload of * on flat f, Vector v
        public static Vector operator /(Vector v, float f) => new (v.x / f, v.y / f, v.z / f);
        public static Vector operator +(Vector v0, Vector v1) => new(v0.x + v1.x, v0.y + v1.y, v0.z + v1.z);
        public static Vector operator -(Vector v0, Vector v1) => new(v0.x - v1.x, v0.y - v1.y, v0.z - v1.z);
        public static Vector operator -(Vector v) => new (-v.x, -v.y, -v.z);
        public static Vector Max(Vector a, Vector b) => new (MathF.Max(a.x, b.x),MathF.Max(a.y, b.y));
        public static Vector Min(Vector a, Vector b) => new (MathF.Min(a.x, b.x),MathF.Min(a.y, b.y));
        public static float Dot(Vector lhs, Vector rhs) => lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;
        public static float Angle(Vector v) => MathF.Atan2(v.y, v.x);
        private float GetMagnitude() => MathF.Sqrt(x * x + y * y + z * z);
        public Vector Normalize() => GetMagnitude() > 0 ? this / GetMagnitude() : this;
        
    }
}