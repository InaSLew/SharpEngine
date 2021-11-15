using System.Collections.Generic;

namespace SharpEngine
{
    public class Scene
    {
        // 2. Move all Triangles to Scene.cs
        // with:
        // public Scene()
        // public void Add(Triangle triangle)
        // public void Render()
        private List<Triangle> triangles;
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