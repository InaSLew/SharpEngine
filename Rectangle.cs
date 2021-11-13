using static OpenGL.Gl;

namespace SharpEngine
{
    public class Rectangle : Shape
    {
        public float width, height;
        public Vector position;

        public Rectangle(float width, float height, Vector position) : base(new Vertex[4])
        {
            this.width = width;
            this.height = height;
            this.position = position;
            vertices = GetVertices();
        }

        private Vertex[] GetVertices()
        {
            var result = new Vertex[6];
            result[0] = new Vertex(position, Color.Red);
            result[1] = new Vertex(new Vector(position.x + width, position.y), Color.Green);
            result[2] = new Vertex(new Vector(position.x, position.y + height), Color.Blue);
            result[3] = new Vertex(new Vector(position.x + width, position.y + height), Color.Red);
            result[4] = new Vertex(new Vector(position.x + width, position.y), Color.Green);
            result[5] = new Vertex(new Vector(position.x, position.y + height), Color.Blue);
            
            return result;
        }
        
        public override void Render()
        {
            base.Render();
            glDrawArrays(GL_TRIANGLES, 0, vertices.Length);
        }
    }
}