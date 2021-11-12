﻿using System.Runtime.InteropServices;
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
            LoadVerticesIntoBuffer();
        }

        public void Scale(float multiplier)
        {

            var center = GetTriangleCenter();
            
            // Move the whole triangle to center in preparation for scaling
            // Fix your Code to make it work with two separate Triangle instances.
            // Hint: You need to save the return value of glGenVertexArray and then always call glBindVertexArray before Rendering the Triangle in its Render-Method. Important, you need to do that as the first thing in the Render-Method.
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

        private Vector GetTriangleCenter() => (GetMinBound() + GetMaxBound()) / 2;

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

        private unsafe void LoadVerticesIntoBuffer()
        {
            var vertexArray = glGenVertexArray();
            var vertexBuffer = glGenBuffer();
            glBindVertexArray(vertexArray);
            glBindBuffer(GL_ARRAY_BUFFER, vertexBuffer);
            
            // sth like this, gotta check the book again
            // glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, );
            
            glVertexAttribPointer(0, 3, GL_FLOAT, false, sizeof(Vertex), Marshal.OffsetOf(typeof(Vertex), nameof(Vertex.Position)));
            glVertexAttribPointer(1, 4, GL_FLOAT, false, sizeof(Vertex), Marshal.OffsetOf(typeof(Vertex), nameof(Vertex.Color)));
            glEnableVertexAttribArray(0);
            glEnableVertexAttribArray(1);
        }
    }
}