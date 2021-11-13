using System;
using static OpenGL.Gl;

namespace SharpEngine
{
    public class Circle : Shape
    {
        public float radius;
        public Vector position;
    
        public Circle(float radius, Vector position) : base(new Vertex[36])
        {
            this.radius = radius;
            this.position = position;
            // vertices = GetVertices();
        }

        // private Vertex[] GetVertices()
        // {
        //     var result = new Vertex[36];
        //     
        //     // result[0] = new Vertex(new Vector(position.x + radius, position.y), Color.Red);
        //     // result[1] = new Vertex(new Vector(position.x, position.y + radius), Color.Red);
        //     // result[2] = new Vertex(position, Color.Red);
        //     //
        //     // this.Rotate(180f);
        //     //
        //     // return result;
        //
        //     var twicePi = 2f * MathF.PI;
        //     
        // }
    }
}