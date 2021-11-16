using System.Collections.Generic;

namespace SharpEngine
{
    public class Scene
    {
        public List<Triangle> Triangles;
        public List<Rectangle> Rectangles;
        public List<Circle> Circles;
        // more Lists of shapes can be put here
        public Scene()
        {
            Triangles = new List<Triangle>();
            Rectangles = new List<Rectangle>();
            Circles = new List<Circle>();
        }

        public void Add(Triangle triangle) => Triangles.Add(triangle);
        public void Add(Rectangle rectangle) => Rectangles.Add(rectangle);
        public void Add(Circle circle) => Circles.Add(circle);

        public void Render()
        {
            Triangles.ForEach(t => t.Render());
            Rectangles.ForEach(r => r.Render());
            Circles.ForEach(c => c.Render());
        }
    }
}