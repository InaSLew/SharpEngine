using System;
using static OpenGL.Gl;

namespace SharpEngine
{
    public class Circle : Shape
    {
        private float radius;
        private Vector position;
        private const int NumberOfTriangles = 36;
        private const float TwicePi = 2f * MathF.PI;
    
        public Circle(float radius, Vector position) : base(new Vertex[NumberOfTriangles])
        {
            this.radius = radius;
            this.position = position;
            vertices = GetVertices();
        }

        private Vertex[] GetVertices()
        {
            var result = new Vertex[NumberOfTriangles];
            
            for (var i = 0; i < vertices.Length; i++)
            {
                result[i].Position = position + new Vector(radius * MathF.Cos(i * TwicePi / NumberOfTriangles), radius * MathF.Sin(i * TwicePi / NumberOfTriangles));
                result[i].Color = Color.Red;
            }

            return result;
        }

        public override unsafe void Render()
        {
            // base.Render();
            fixed (Vertex* vertex = &vertices[0])
            {
                glBufferData(GL_ARRAY_BUFFER, sizeof(Vertex) * vertices.Length, vertex, GL_DYNAMIC_DRAW);
            }
            glDrawArrays(GL_TRIANGLE_FAN, 0, vertices.Length);
        }
    }
}