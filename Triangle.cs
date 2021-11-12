using System;
using System.Runtime.InteropServices;
using static OpenGL.Gl;

namespace SharpEngine
{
    public class Triangle
    {
        private Vertex[] vertices;
        public float CurrentScale { get; private set; }
        public Vector moveDirection;
        public Triangle(Vertex[] vertices)
        {
            this.vertices = vertices;
            CurrentScale = 1f;
            LoadVerticesIntoBuffer();
        }

        public void Scale(float multiplier)
        {
            var center = GetCenter();
            Move(center * -1);

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].Position *= multiplier;
            }
            
            Move(center);
            
            CurrentScale *= multiplier;
        }

        private Vector GetCenter() => (GetMinBound() + GetMaxBound()) / 2;

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

        public void Move(Vector? direction = null)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].Position += direction ?? moveDirection;
            }
            BounceIfTouchEdge();
        }

        private void BounceIfTouchEdge()
        {
            if (GetMaxBound().x >= 1 && moveDirection.x > 0 || GetMinBound().x <= -1 && moveDirection.x < 0)
            {
                moveDirection.x *= -1;
            }
            if (GetMaxBound().y >= 1 && moveDirection.y > 0 || GetMinBound().y <= -1 && moveDirection.y < 0)
            {
                moveDirection.y *= -1;
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

        private unsafe void LoadVerticesIntoBuffer()
        {
            var vertexArray = glGenVertexArray();
            var vertexBuffer = glGenBuffer();
            glBindVertexArray(vertexArray);
            glBindBuffer(GL_ARRAY_BUFFER, vertexBuffer);
            glVertexAttribPointer(0, 3, GL_FLOAT, false, sizeof(Vertex), Marshal.OffsetOf(typeof(Vertex), nameof(Vertex.Position)));
            glVertexAttribPointer(1, 4, GL_FLOAT, false, sizeof(Vertex), Marshal.OffsetOf(typeof(Vertex), nameof(Vertex.Color)));
            glEnableVertexAttribArray(0);
            glEnableVertexAttribArray(1);
        }

        public void Rotate(float degree)
        {
            var center = GetCenter();
            Move(center * -1);
            for (var i = 0; i < vertices.Length; i++)
            {
                var currentAngle = MathF.Atan2(vertices[i].Position.y, vertices[i].Position.x);
                var currentMagnitude = MathF.Sqrt(MathF.Pow(vertices[i].Position.x, 2) + MathF.Pow(vertices[i].Position.y, 2));
                var newPositionX = MathF.Cos(currentAngle + GetRadians(degree)) * currentMagnitude;
                var newPositionY = MathF.Sin(currentAngle + GetRadians(degree)) * currentMagnitude;
                vertices[i].Position = new Vector(newPositionX, newPositionY, vertices[i].Position.z);
            }
            Move(center);
        }

        private float GetRadians(float angle) => angle * (MathF.PI / 180f);
        public void SetMoveDirection(Vector direction) => moveDirection = direction;
    }
}