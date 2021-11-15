using System;
using System.IO;
using GLFW;
using static OpenGL.Gl;

namespace SharpEngine
{
    class Program
    {
        // static Shape rectangle = new Rectangle(.3f, .4f, new Vector(.3f, 0));
        // private static Shape circle = new Circle(.3f, new Vector(-.2f, 0));
        // private static Shape cone = new Cone(.3f, 45f, new Vector(0, 0));
        private const string vertexShaderPath = "shaders/Shape.vert";
        private const string fragmentShaderPath = "shaders/Shape.frag";
        
        static float Lerp(float from, float to, float t) => from + (to - from) * t;

        static float GetRandomFloat(Random random, float min = 0, float max = 1) =>
            Lerp(min, max, (float) random.Next() / int.MaxValue);

        static void FillSceneWithTriangles(Scene scene, Material material)
        {
            // var random = new Random();
            // for (int i = 0; i < 10; i++)
            // {
            //     Shape triangle = new Triangle(new Vertex[]
            //     {
            //         new Vertex(new Vector(-.1f, 0f), Color.Red),
            //         new Vertex(new Vector(.1f, 0f), Color.Green),
            //         new Vertex(new Vector(0f, .133f), Color.Blue)
            //     }, material);
            //     triangle.Rotate(GetRandomFloat(random));
            //     triangle.Move(new Vector(GetRandomFloat(random, -1, 1), GetRandomFloat(random, -1, 1)));
            //     scene.Add(triangle);
            // }
        }

        static void Main(string[] args)
        {
            var window = new Window();
            var material = new Material(vertexShaderPath, fragmentShaderPath);
            var scene = new Scene();
            window.Load(scene);
            
            Triangle newTriangle = new Triangle(.3f, .4f, new Vector(-.3f, 0), material);
            scene.Add(newTriangle);

            // engine rendering loop
            var direction = new Vector(.0003f, .0003f);
            var multiplier = .999f;
            var rotation = .05f;
            // triangle.SetMoveDirection((Vector.Right + Vector.Down) * 3);
            // triangle2.SetMoveDirection((Vector.Left + Vector.Down) * 3);
            // rectangle.SetMoveDirection((Vector.Right + Vector.Up) * 3);
            // circle.SetMoveDirection((Vector.Left + Vector.Up) * 3);
            
            while (window.IsOpen())
            {
                
                for (var i = 0; i < scene.triangles.Count; i++)
                {
                    var triangle = scene.triangles[i];
                    
                    if (triangle.CurrentScale <= 0.5f) {
                        multiplier = 1.001f;
                    }
                    if (triangle.CurrentScale >= 1f) {
                        multiplier = 0.999f;
                    }
                    
                    triangle.Scale(multiplier);
                    triangle.Rotate(rotation);
                
                    // 4. Check the X-Bounds of the Screen
                    if (triangle.GetMaxBound().x >= 1 && direction.x > 0 || triangle.GetMinBound().x <= -1 && direction.x < 0) {
                        direction.x *= -1;
                    }
                    // 5. Check the Y-Bounds of the Screen
                    if (triangle.GetMaxBound().y >= 1 && direction.y > 0 || triangle.GetMinBound().y <= -1 && direction.y < 0) {
                        direction.y *= -1;
                    }
                    
                    triangle.Move(direction);
                }
                window.Render();
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
