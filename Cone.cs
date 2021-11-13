namespace SharpEngine
{
    public class Cone : Shape
    {
        // public Cone(float radius, float angle, Vector position)
        private float radius, angle;
        private Vector position;

        public Cone(Vertex[] vertices, float radius, float angle, Vector position) : base(vertices)
        {
            this.radius = radius;
            this.angle = angle;
            this.position = position;
        }
    }
}