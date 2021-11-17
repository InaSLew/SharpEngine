using System.Collections.Generic;

namespace SharpEngine
{
    public class Scene
    {
        internal List<Shape> Shapes;
        public Scene()
        {
            Shapes = new List<Shape>();
        }

        public void Add(Shape shape) => Shapes.Add(shape);

        public void Render()
        {
            Shapes.ForEach(s => s.Render());
        }
    }
}