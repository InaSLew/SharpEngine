using System;
using static OpenGL.Gl;

namespace SharpEngine
{
    public class Cone : Shape
    {
        // public Cone(float radius, float angle, Vector position)
        private float radius, angle;
        private Vector position;
        private const int NumberOfTriangles = 36;
        private const float TwicePi = 2f * MathF.PI;

        public Cone(float radius, float angle, Vector position) : base(new Vertex[NumberOfTriangles])
        {
            this.radius = radius;
            this.angle = angle;
            this.position = position;
            vertices = GetVertices();
        }

        private Vertex[] GetVertices()
        {
            var result = new Vertex[NumberOfTriangles];
            var red = 1f;
            var green = 0f;
            var blue = 0f;
            for (var i = 0; i < vertices.Length; i++)
            {
                result[i] = new Vertex(GetVectorOnCircumference(i), Color.None)
                {
                    Color = new Color(red, green, blue, 1)
                };
            
                if (red > 0) red -= .084f;
                else green = 1f;
                
                if (green > 0)
                {
                    red = 0;
                    green -= .084f;
                }
                else blue = 1f;
                
                if (blue > 0)
                {
                    green = 0;
                    blue -= .09f;
                }
            }
            return result;
        }

        private Vector GetVectorOnCircumference(int i) => position + new Vector(
            radius * MathF.Cos(i * TwicePi / NumberOfTriangles), radius * MathF.Sin(i * TwicePi / NumberOfTriangles));

        public override void Render()
        {
            base.Render();
            glDrawArrays(GL_TRIANGLES, 0, vertices.Length);
        }
    }
}