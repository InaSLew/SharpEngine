using System.Collections.Generic;

namespace SharpEngine
{
    public class Scene
    {
        public List<Triangle> triangles;
        public Scene()
        {
            triangles = new List<Triangle>();
        }

        public void Add(Triangle triangle)
        {
            triangles.Add(triangle);
        }

        public void Render()
        {
            triangles.ForEach(t => t.Render());
        }
    }
}