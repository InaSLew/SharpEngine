using static OpenGL.Gl;

namespace SharpEngine
{
    public class Triangle
    {
        private Vertex[] vertices;
        public float CurrentScale { get; private set; }
        public Triangle(Vertex[] vertices)
        {
            this.vertices = vertices;
            CurrentScale = 1f;
        }

        public void Scale(float multiplier)
        {
            
            var center = (GetMinBound() + GetMaxBound()) / 2;
            
            // Move the whole triangle to center in preparation for scaling
            for (int i = 0; i < vertices.Length; i++)
            {
                Move(center * -1);
            }
            
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].Position *= multiplier;
            }
            
            // Move it back to where it was
            for (int i = 0; i < vertices.Length; i++)
            {
                Move(center);
            }
            
            CurrentScale *= multiplier;
        }

        public Vector GetMaxBound()
        {
            var max = vertices[0].Position;
            for (var i = 0; i < vertices.Length; i++)
            {
                max = Vector.Max(max, vertices[i].Position);
            }
            return max;
        }
        
        public Vector GetMinBound()
        {
            var min = vertices[0].Position;
            for (var i = 0; i < vertices.Length; i++)
            {
                min = Vector.Min(min, vertices[i].Position);
            }

            return min;
        }

        public void Move(Vector direction)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].Position += direction;
            }
        }

        public unsafe void Render()
        {
            glDrawArrays(GL_TRIANGLES, 0, vertices.Length);
            fixed (Vertex* vertex = &vertices[0])
            {
                glBufferData(GL_ARRAY_BUFFER, sizeof(Vertex) * vertices.Length, vertex, GL_STATIC_DRAW);
            }
        }
    }
}