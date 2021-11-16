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
    
        public Circle(float radius, Vector position, Material material) : base(new Vertex[NumberOfTriangles], material, Color.Blue)
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
                result[i] = new Vertex(GetVectorOnCircumference(i), color);
            }
            return result;
        }
    
        private Vector GetVectorOnCircumference(int i) => position + new Vector(
            radius * MathF.Cos(i * TwicePi / NumberOfTriangles), radius * MathF.Sin(i * TwicePi / NumberOfTriangles));
    
        public override void Render()
        {
            base.Render();
            glDrawArrays(GL_TRIANGLE_FAN, 0, vertices.Length);
        }
    }
}