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
            
            var shape = new Triangle(.03f, .03f, new Vector(0, 0), material);
            shape.Transform.CurrentScale = new Vector(1.5f, 1f, 1f);
            scene.Add(shape);

            var ground = new Rectangle(.04f, .05f, new Vector(0, -1), material);
            ground.Transform.CurrentScale = new Vector(20f, 1f, 1f);
            scene.Add(ground);

            // engine rendering loop
            const int fixedStepNumberPerSecond = 30;
            const float fixedDeltaTime = 1.0f / fixedStepNumberPerSecond;
            const float movementSpeed = .5f;
            double previousFixedStep = 0.0;

            while (window.IsOpen())
            {
                if (Glfw.Time > previousFixedStep + fixedDeltaTime)
                {
                    previousFixedStep += fixedDeltaTime;
                    var walkDirection = new Vector();
                    if (window.GetKey(Keys.W)) walkDirection += shape.Transform.Forward;
                    if (window.GetKey(Keys.S)) walkDirection += shape.Transform.Backward;
                    if (window.GetKey(Keys.A)) walkDirection += shape.Transform.Left;
                    if (window.GetKey(Keys.D)) walkDirection += shape.Transform.Right;
                    
                    if (window.GetKey(Keys.Q))
                    {
                        var rotation = shape.Transform.Rotation;
                        rotation.z += MathF.PI * fixedDeltaTime;
                        shape.Transform.Rotation = rotation;
                    }
                    if (window.GetKey(Keys.E))
                    {
                        var rotation = shape.Transform.Rotation;
                        rotation.z -= MathF.PI * fixedDeltaTime;
                        shape.Transform.Rotation = rotation;
                    }

                    walkDirection = walkDirection.Normalize();
                    shape.Transform.Position += walkDirection * movementSpeed * fixedDeltaTime;
                    // for (var i = 0; i < scene.Triangles.Count; i++)
                    // {
                    //     var triangle = scene.Triangles[i];
                    //     triangle.Transform.Scale(multiplier);
                    //     if (triangle.Transform.CurrentScale.x <= 0.5f) {
                    //         multiplier = 1.05f;
                    //     }
                    //     if (triangle.Transform.CurrentScale.x >= 1f) {
                    //         multiplier = .95f;
                    //     }
                    //
                    //     triangle.Transform.Rotate(rotation);
                    //
                    //     if (triangle.GetMaxBound().x >= 1 && direction.x > 0 || triangle.GetMinBound().x <= -1 && direction.x < 0)
                    //     {
                    //         direction.x *= -1;
                    //     }
                    //     if (triangle.GetMaxBound().y >= 1 && direction.y > 0 || triangle.GetMinBound().y <= -1 && direction.y < 0)
                    //     {
                    //         direction.y *= -1;
                    //     }
                    //     triangle.Transform.Move(direction);
                    // }
                    //
                    // var rectangles = scene.Rectangles;
                    // for (var i = 0; i < rectangles.Count; i++)
                    // {
                    //     var rect = rectangles[i];
                    //     rect.Transform.Scale(multiplier);
                    //     if (rect.Transform.CurrentScale.x <= 0.5f) {
                    //         multiplier = 1.05f;
                    //     }
                    //     if (rect.Transform.CurrentScale.x >= 1f) {
                    //         multiplier = .95f;
                    //     }
                    //
                    //     rect.Transform.Rotate(rotation);
                    //
                    //     if (rect.GetMaxBound().x >= 1 && direction.x > 0 || rect.GetMinBound().x <= -1 && direction.x < 0)
                    //     {
                    //         direction.x *= -1;
                    //     }
                    //     if (rect.GetMaxBound().y >= 1 && direction.y > 0 || rect.GetMinBound().y <= -1 && direction.y < 0)
                    //     {
                    //         direction.y *= -1;
                    //     }
                    //     rect.Transform.Move(direction);
                    // }

                    // var circles = scene.Circles;
                    // for (var i = 0; i < circles.Count; i++)
                    // {
                    //     var circle = circles[i];
                    //     circle.Transform.Scale(multiplier);
                    //     if (circle.Transform.CurrentScale.x <= 0.5f) {
                    //         multiplier = 1.05f;
                    //     }
                    //     if (circle.Transform.CurrentScale.x >= 1f) {
                    //         multiplier = .95f;
                    //     }
                    //
                    //     circle.Transform.Rotate(xRotation);
                    //
                    //     if (circle.GetMaxBound().x >= 1 && direction.x > 0 || circle.GetMinBound().x <= -1 && direction.x < 0)
                    //     {
                    //         direction.x *= -1;
                    //     }
                    //     if (circle.GetMaxBound().y >= 1 && direction.y > 0 || circle.GetMinBound().y <= -1 && direction.y < 0)
                    //     {
                    //         direction.y *= -1;
                    //     }
                    //     circle.Transform.Move(direction);
                    // }
                }
                
                window.Render();
            }
            Glfw.Terminate();
        }
    }
}
