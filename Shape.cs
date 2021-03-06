using System;
using System.Runtime.InteropServices;
using static OpenGL.Gl;

namespace SharpEngine
{
    public abstract class Shape
    {
        protected Vertex[] vertices;
        private uint vertexArray;
        private uint vertexBuffer;
        public Transform Transform { get; }
        private Material material;
        
        // physics implementation-related
        public Vector velocity;
        public Vector linearForce;
        public float gravityScale = 1f;
        private float mass;
        private float massInverse;

        public float Mass
        {
            get => mass;
            set
            {
                mass = value;
                massInverse = float.IsPositiveInfinity(value) ? 0f : 1f / value;
            }
        }

        public float MassInverse => massInverse;

        protected Color color { get; set; }
        
        public Shape(Vertex[] vertices, Material material, Color color)
        {
            this.vertices = vertices;
            this.material = material;
            LoadVerticesIntoBuffer();
            Transform = new Transform();
            this.color = color;
        }

        public Vector GetMaxBound()
        {
            var max =  Transform.Matrix * vertices[0].Position;
            for (var i = 0; i < vertices.Length; i++)
            {
                max = Vector.Max(max, Transform.Matrix * vertices[i].Position);
            }
            return max;
        }
        
        public Vector GetMinBound()
        {
            var min = Transform.Matrix * vertices[0].Position;
            for (var i = 0; i < vertices.Length; i++)
            {
                min = Vector.Min(min, Transform.Matrix * vertices[i].Position);
            }

            return min;
        }

        public Vector GetCenter() => (GetMinBound() + GetMaxBound()) / 2;

        public void SetColor(Color newColor)
        {
            color = newColor;
            for (var i = 0; i < vertices.Length; i++)
            {
                vertices[i].Color = color;
            }
        }

        public virtual unsafe void Render()
        {
            material.Use();
            material.SetTransform(Transform.Matrix);
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
    }
}