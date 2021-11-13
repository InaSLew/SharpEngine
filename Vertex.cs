namespace SharpEngine
{
    public struct Vertex
    {
        public Vector Position;
        public Color Color;

        public Vertex(Vector position, Color color)
        {
            Position = position;
            Color = color;
        }
    }
}