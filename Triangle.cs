﻿namespace SharpEngine
{
    public class Triangle : Shape
    {
        public float width, height;
        public Vector position;

        public Triangle(float width, float height, Vector position) : base(new Vertex[3])
        {
            this.width = width;
            this.height = height;
            this.position = position;
            vertices = GetVertices();
        }

        private Vertex[] GetVertices()
        {
            var result = new Vertex[3];
            result[0] = new Vertex(new Vector(position.x + width, position.y), Color.Red);
            result[1] = new Vertex(new Vector(position.x, position.y + height), Color.Green);
            result[2] = new Vertex(position, Color.Blue);
            return result;
        }
    }
}