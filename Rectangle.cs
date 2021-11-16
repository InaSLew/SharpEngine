using static OpenGL.Gl;

namespace SharpEngine
{
    public class Rectangle : Shape
    {
        public float width, height;
        public Vector position;

        public Rectangle(float width, float height, Vector position, Material material, Color color) : base(new Vertex[4], material, color)
        {
            this.width = width;
            this.height = height;
            this.position = position;
            vertices = GetVertices();
        }
    
        private Vertex[] GetVertices()
        {
            var result = new Vertex[6];
            result[0] = new Vertex(position, color);
            result[1] = new Vertex(new Vector(position.x + width, position.y), color);
            result[2] = new Vertex(new Vector(position.x, position.y + height), color);
            result[3] = new Vertex(new Vector(position.x + width, position.y + height), color);
            result[4] = new Vertex(new Vector(position.x + width, position.y), color);
            result[5] = new Vertex(new Vector(position.x, position.y + height), color);
            
            return result;
        }
        
        public override void Render()
        {
            base.Render();
            glDrawArrays(GL_TRIANGLES, 0, vertices.Length);
        }
    }
}