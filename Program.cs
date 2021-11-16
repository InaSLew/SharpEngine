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
            
            var player = new Triangle(.03f, .03f, new Vector(0, 0), material, Color.Blue);
            player.Transform.CurrentScale = new Vector(.5f, 2.5f, 1f);
            scene.Add(player);

            var cube = new Rectangle(.04f, .06f, new Vector(0, .5f), material, Color.White);
            cube.Transform.CurrentScale = new Vector(1.5f, 1.5f, 1f);
            scene.Add(cube);

            var circle = new Circle(.05f, new Vector(.25f, .5f), material, Color.White);
            scene.Add(circle);

            var ground = new Rectangle(.04f, .05f, new Vector(0, -1), material, new Color(.58f, .29f, 0, 1f));
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
                    if (window.GetKey(Keys.W)) walkDirection += player.Transform.Forward;
                    if (window.GetKey(Keys.S)) walkDirection += player.Transform.Backward;
                    if (window.GetKey(Keys.A)) walkDirection += player.Transform.Left;
                    if (window.GetKey(Keys.D)) walkDirection += player.Transform.Right;
                    
                    if (window.GetKey(Keys.Q))
                    {
                        var rotation = player.Transform.Rotation;
                        rotation.z += MathF.PI * fixedDeltaTime;
                        player.Transform.Rotation = rotation;
                    }
                    if (window.GetKey(Keys.E))
                    {
                        var rotation = player.Transform.Rotation;
                        rotation.z -= MathF.PI * fixedDeltaTime;
                        player.Transform.Rotation = rotation;
                    }

                    var dotProduct = Vector.Dot(player.Transform.Forward, cube.position);
                    if (dotProduct < 0) cube.SetColor(Color.Red);
                    else if (dotProduct > 0) cube.SetColor(Color.Green);

                    walkDirection = walkDirection.Normalize();
                    player.Transform.Position += walkDirection * movementSpeed * fixedDeltaTime;
                }
                
                window.Render();
            }
            Glfw.Terminate();
        }
    }
}
