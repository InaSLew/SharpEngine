using System;

namespace SharpEngine
{
    public struct Vertex
    {
        public Vector Position;
        public Color Color;

        public Vertex(Vector position, Color color) {
            Position = position;
            Color = color;
        }
    }
    public struct Vector
    {
        internal float x, y, z;

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
        public static Vector operator /(Vector v, float f) => new (v.x / f, v.y / f, v.z / f);
        public static Vector operator +(Vector v0, Vector v1) => new(v0.x + v1.x, v0.y + v1.y, v0.z + v1.z);
        public static Vector operator -(Vector v0, Vector v1) => new(v0.x - v1.x, v0.y - v1.y, v0.z - v1.z);
        public static Vector Max(Vector a, Vector b) => new (MathF.Max(a.x, b.x),MathF.Max(a.y, b.y));
        public static Vector Min(Vector a, Vector b) => new (MathF.Min(a.x, b.x),MathF.Min(a.y, b.y));
    }
}