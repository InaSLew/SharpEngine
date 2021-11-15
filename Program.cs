using System;
using System.IO;
using GLFW;
using static OpenGL.Gl;

namespace SharpEngine
{
    class Program
    {
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
            var direction = Vector.One * .0001f;
            var multiplier = .999f;
            var rotation = .05f;
            
            while (window.IsOpen())
            {
                
                for (var i = 0; i < scene.triangles.Count; i++)
                {
                    var triangle = scene.triangles[i];
                    
                    // if (triangle.CurrentScale <= 0.5f) {
                    //     multiplier = 1.001f;
                    // }
                    // if (triangle.CurrentScale >= 1f) {
                    //     multiplier = 0.999f;
                    // }
                    //
                    // triangle.Scale(multiplier);
                    // triangle.Rotate(rotation);
                    //
                    // if (triangle.GetMaxBound().x >= 1 && direction.x > 0 || triangle.GetMinBound().x <= -1 && direction.x < 0) {
                    //     direction.x *= -1;
                    // }
                    // if (triangle.GetMaxBound().y >= 1 && direction.y > 0 || triangle.GetMinBound().y <= -1 && direction.y < 0) {
                    //     direction.y *= -1;
                    // }
                    
                    triangle.Move(direction);
                }
                window.Render();
            }
            Glfw.Terminate();
        }
    }
}
