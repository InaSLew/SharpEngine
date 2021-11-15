using System.IO;
using GLFW;
using static OpenGL.Gl;

namespace SharpEngine
{
    class Program
    {
        // static Shape rectangle = new Rectangle(.3f, .4f, new Vector(.3f, 0));
        // private static Shape circle = new Circle(.3f, new Vector(-.2f, 0));
        private static Shape cone = new Cone(.3f, 45f, new Vector(0, 0));
        private const string vertexShaderPath = "shaders/Shape.vert";
        private const string fragmentShaderPath = "shaders/Shape.frag";

        static void Main(string[] args)
        {
            var window = new Window();
            var material = new Material(vertexShaderPath, fragmentShaderPath);
            
            var multiplier = 0.999f;
            var multiplier2 = 0.985f;
            
            // triangle.SetMoveDirection((Vector.Right + Vector.Down) * 3);
            // triangle2.SetMoveDirection((Vector.Left + Vector.Down) * 3);
            // rectangle.SetMoveDirection((Vector.Right + Vector.Up) * 3);
            // circle.SetMoveDirection((Vector.Left + Vector.Up) * 3);
            
            while (window.IsOpen())
            {
                
                // triangle.Scale(multiplier);
                // if (triangle.CurrentScale <= 0.5f)
                // {
                //     multiplier = 1.0001f;
                // }
                // if (triangle.CurrentScale >= 1f)
                // {
                //     multiplier = 0.9999f;
                // }
                // triangle.Move();
                // triangle.Rotate(.1f);
                //
                // triangle2.Scale(multiplier2);
                // if (triangle2.CurrentScale <= 0.5f)
                // {
                //     multiplier2 = 1.001f;
                // }
                // if (triangle2.CurrentScale >= 1f)
                // {
                //     multiplier2 = 0.999f;
                // }
                // triangle2.Move();
                // triangle2.Rotate(-.08f);
                //
                // rectangle.Scale(multiplier);
                // if (rectangle.CurrentScale <= 0.5f)
                // {
                //     multiplier = 1.0001f;
                // }
                // if (rectangle.CurrentScale >= 1f)
                // {
                //     multiplier = 0.9999f;
                // }
                // rectangle.Move();
                // rectangle.Rotate(-.05f);
                //
                // circle.Scale(multiplier2);
                // if (circle.CurrentScale <= 0.5f)
                // {
                //     multiplier = 1.0001f;
                // }
                // if (circle.CurrentScale >= 1f)
                // {
                //     multiplier = 0.9999f;
                // }
                // circle.Move();
                // circle.Rotate(-.05f);
            }

            Glfw.Terminate();
        }
    }
}
