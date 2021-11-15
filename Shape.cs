using System;
using System.Runtime.InteropServices;
using static OpenGL.Gl;

namespace SharpEngine
{
    public abstract class Shape
    {
        protected Vertex[] vertices;
        
        private Matrix transform = Matrix.Identity;
        public Vector Translation => new Vector(transform.m14, transform.m24, transform.m34);

        uint vertexArray;
        uint vertexBuffer;
        public float CurrentScale { get; private set; }
        public Material material;
        public Shape(Vertex[] vertices, Material material)
        {
            this.vertices = vertices;
            this.material = material;
            CurrentScale = 1f;
            LoadVerticesIntoBuffer();
        }
        
        public void Move(Vector direction)
        {
            transform *= Matrix.Translate(direction);
        }

        public void Scale(float multiplier)
        {
            // var center = GetCenter();
            // Move(center * -1);
            //
            // for (int i = 0; i < vertices.Length; i++)
            // {
            //     vertices[i].Position *= multiplier;
            // }
            //
            // Move(center);
            //;
            transform *= Matrix.Scale(new Vector(multiplier, multiplier));
            CurrentScale *= multiplier;
        }
        
        public virtual void Rotate(float degree)
        {
            // var center = GetCenter();
            // Move(center * -1);
            // for (var i = 0; i < vertices.Length; i++)
            // {
            //     var currentAngle = MathF.Atan2(vertices[i].Position.y, vertices[i].Position.x);
            //     var currentMagnitude = MathF.Sqrt(MathF.Pow(vertices[i].Position.x, 2) + MathF.Pow(vertices[i].Position.y, 2));
            //     var newPositionX = MathF.Cos(currentAngle + GetRadians(degree)) * currentMagnitude;
            //     var newPositionY = MathF.Sin(currentAngle + GetRadians(degree)) * currentMagnitude;
            //     vertices[i].Position = new Vector(newPositionX, newPositionY);
            // }
            // Move(center);
            transform *= Matrix.Rotate(GetRadians(degree));
        }

        // private Vector GetCenter() => (GetMinBound() + GetMaxBound()) / 2;

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

        public virtual unsafe void Render()
        {
            material.Use();
            material.SetTransform(transform);
            glBindVertexArray(vertexArray);
            glBindBuffer(GL_ARRAY_BUFFER, vertexBuffer);
            fixed (Vertex* vertex = &vertices[0])
            {
                glBufferData(GL_ARRAY_BUFFER, sizeof(Vertex) * vertices.Length, vertex, GL_DYNAMIC_DRAW);
            }
        }

        private unsafe void LoadVerticesIntoBuffer()
        {
            vertexArray = glGenVertexArray();
            vertexBuffer = glGenBuffer();
            glBindVertexArray(vertexArray);
            glBindBuffer(GL_ARRAY_BUFFER, vertexBuffer);
            glVertexAttribPointer(0, 3, GL_FLOAT, false, sizeof(Vertex), Marshal.OffsetOf(typeof(Vertex), nameof(Vertex.Position)));
            glVertexAttribPointer(1, 4, GL_FLOAT, false, sizeof(Vertex), Marshal.OffsetOf(typeof(Vertex), nameof(Vertex.Color)));
            glEnableVertexAttribArray(0);
            glEnableVertexAttribArray(1);
            glBindVertexArray(0);
        }

        protected float GetRadians(float degree) => degree * (MathF.PI / 180f);
        protected float GetArctangentInDegree(Vector v) => MathF.Atan2(v.y, v.x) * 180f / MathF.PI;
    }
}