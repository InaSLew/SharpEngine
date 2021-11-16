namespace SharpEngine
{
    public struct Color
    {
        public static readonly Color Red = new (1, 0, 0, 1);
        public static readonly Color Green = new (0, 1, 0, 1);
        public static readonly Color Blue = new (0, 0, 1, 1);
        public static readonly Color None = new (0, 0, 0, 0);
        public static readonly Color Black = new (0, 0, 0, 1);
        public static readonly Color White = new(1, 1, 1, 1);
        public float R, G, B, A;

        public Color(float r, float g, float b, float a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }
    }
}