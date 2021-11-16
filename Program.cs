using System;
using GLFW;
using static OpenGL.Gl;

namespace SharpEngine
{
    class Program
    {
        private const string VertexShaderPath = "shaders/Shape.vert";
        private const string FragmentShaderPath = "shaders/Shape.frag";
        
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

        static void Main()
        {
            var window = new Window();
            var material = new Material(VertexShaderPath, FragmentShaderPath);
            var scene = new Scene();
            window.Load(scene);
            
            Triangle newTriangle = new Triangle(.3f, .4f, new Vector(-.3f, 0), material);
            scene.Add(newTriangle);

            // engine rendering loop
            var direction = Vector.One * .01f;
            var multiplier = .95f;
            var rotation = new Vector(0, .05f, 0);
            const int fixedStepNumberPerSecond = 30;
            const double fixedStepUnit = 1.0 / fixedStepNumberPerSecond;
            double previousUpdate = 0.0;

            while (window.IsOpen())
            {
                if (Glfw.Time > previousUpdate + fixedStepUnit)
                {
                    previousUpdate = Glfw.Time;
                    for (var i = 0; i < scene.triangles.Count; i++)
                    {
                        var triangle = scene.triangles[i];
                        triangle.Transform.Scale(multiplier);
                        if (triangle.Transform.CurrentScale.x <= 0.5f) {
                            multiplier = 1.05f;
                        }
                        if (triangle.Transform.CurrentScale.x >= 1f) {
                            multiplier = .95f;
                        }
                    
                        triangle.Transform.Rotate(rotation);
                    
                        if (triangle.GetMaxBound().x >= 1 && direction.x > 0 || triangle.GetMinBound().x <= -1 && direction.x < 0)
                        {
                            direction.x *= -1;
                        }
                        if (triangle.GetMaxBound().y >= 1 && direction.y > 0 || triangle.GetMinBound().y <= -1 && direction.y < 0)
                        {
                            direction.y *= -1;
                        }
                        triangle.Transform.Move(direction);
                    }
                }
                
                window.Render();
            }
            Glfw.Terminate();
        }
    }
}
