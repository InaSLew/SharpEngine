namespace SharpEngine
{
    public class Triangle : Shape
    {
        public float width, height;
        public Vector position;

        public Triangle(float width, float height, Vector position) : base(new Vertex[3])
        {
            this.width = width;
            this.height = height;
            this.position = position;
            vertices = GetVertices();
        }

        private Vertex[] GetVertices()
        {
            var result = new Vertex[3];
            for (var i = 0; i < vertices.Length; i++)
            {
                result[i] = i switch
                {
                    0 => new Vertex(new Vector(width, 0), Color.Red),
                    1 => new Vertex(new Vector(0, height), Color.Green),
                    _ => new Vertex(position, Color.Blue)
                };
            }
            return result;
        }
    }
}