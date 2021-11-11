using System.IO;
using System.Runtime.InteropServices;
using GLFW;
using static OpenGL.Gl;

namespace SharpEngine
{
    class Program
    {
        static Triangle triangle = new Triangle(new Vertex[] {
            new (new Vector(0f, 0f), Color.Red),
            new (new Vector(1f, 0f), Color.Green),
            new (new Vector(0f, 1f), Color.Blue)
        });

        private const int VertexSize = 3;
        private const int Width = 1024;
        private const int Height = 768;

        static void Main(string[] args)
        {
            var window = CreateWindow();
            triangle.LoadVerticesIntoBuffer();
            CreateShaderProgram();
            
            var direction = new Vector(0.0003f, 0.0003f);
            var multiplier = 0.999f;
            while (!Glfw.WindowShouldClose(window))
            {
                Glfw.PollEvents(); // reacts to window changes (position etc.)
                ClearScreen();
                Render();
                triangle.Scale(multiplier);
                if (triangle.CurrentScale <= 0.5f)
                {
                    multiplier = 1.001f;
                }
                if (triangle.CurrentScale >= 1f)
                {
                    multiplier = 0.999f;
                }

                triangle.Move(direction);
                if (triangle.GetMaxBound().x >= 1 && direction.x > 0 || triangle.GetMinBound().x <= -1 && direction.x < 0) {
                    direction.x *= -1;
                }
                if (triangle.GetMaxBound().y >= 1 && direction.y > 0 || triangle.GetMinBound().y <= -1 && direction.y < 0) {
                    direction.y *= -1;
                }
            }

            Glfw.Terminate();
        }

        private static void ClearScreen()
        {
            glClearColor(0,0,0, 1);
            glClear(GL_COLOR_BUFFER_BIT);;
        }

        private static void Render()
        {
            triangle.Render();
            glFlush();
        }

        // private static void ScaleUpTriangle()
        // {
        //     for (var i = 0; i < vertices.Length; i++)
        //     {
        //         vertices[i].Position *= 1.00009f;
        //     }
        // }
        //
        // private static void ShrinkTriangle()
        // {
        //     for (var i = 0; i < vertices.Length; i++)
        //     {
        //         vertices[i].Position *= 0.9999f;
        //     }
        // }
        //
        // private static void MoveDown()
        // {
        //     for (var i = 0; i < vertices.Length; i++)
        //     {
        //         vertices[i].Position.y -= 0.001f;
        //     }
        // }
        //
        // private static void MoveToRight()
        // {
        //     for (var i = 0; i < vertices.Length; i++)
        //     {
        //         vertices[i].Position.x += 0.001f;
        //     }
        // }

        private static Window CreateWindow()
        {
            // Initialize and configure
            Glfw.Init();
            Glfw.WindowHint(Hint.ClientApi, ClientApi.OpenGL);
            Glfw.WindowHint(Hint.ContextVersionMajor, 3);
            Glfw.WindowHint(Hint.ContextVersionMinor, 3);
            Glfw.WindowHint(Hint.Decorated, true);
            Glfw.WindowHint(Hint.OpenglProfile, Profile.Core);
            Glfw.WindowHint(Hint.OpenglForwardCompatible, Constants.True);
            Glfw.WindowHint(Hint.Doublebuffer, Constants.False);

            // create and launch window
            var window = Glfw.CreateWindow(Width, Height, "SharpEngine", Monitor.None, Window.None);
            Glfw.MakeContextCurrent(window);
            Import(Glfw.GetProcAddress);
            return window;
        }
        
        private static void CreateShaderProgram()
        {
            var vertexShader = glCreateShader(GL_VERTEX_SHADER);
            glShaderSource(vertexShader, File.ReadAllText("shaders/red-triangle.vert"));
            glCompileShader(vertexShader);
            
            var fragmentShader = glCreateShader(GL_FRAGMENT_SHADER);
            glShaderSource(fragmentShader, File.ReadAllText("shaders/red-triangle.frag"));
            glCompileShader(fragmentShader);
            
            var program = glCreateProgram();
            glAttachShader(program, vertexShader);
            glAttachShader(program, fragmentShader);
            glLinkProgram(program);
            glUseProgram(program);
        }
    }
}
