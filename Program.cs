using System;
using System.IO;
using GLFW;
using OpenGL;
using static OpenGL.Gl;

namespace SharpEngine
{
    class Program
    {
        static Shape triangle = new Triangle(.2f, .2f, new Vector(0, 0));
        static Shape triangle2 = new Triangle(.3f, .4f, new Vector(-.3f, 0));
        static Shape rectangle = new Rectangle(.3f, .4f, new Vector(.3f, 0));
        private static Shape circle = new Circle(.3f, new Vector(-.2f, 0));

        private const int Width = 1024;
        private const int Height = 768;

        static void Main(string[] args)
        {
            var window = CreateWindow();
            CreateShaderProgram();
            
            var multiplier = 0.999f;
            var multiplier2 = 0.985f;
            
            triangle.SetMoveDirection((Vector.Right + Vector.Down) * 3);
            triangle2.SetMoveDirection((Vector.Left + Vector.Down) * 3);
            rectangle.SetMoveDirection((Vector.Right + Vector.Up) * 3);
            circle.SetMoveDirection((Vector.Left + Vector.Up) * 3);
            
            while (!Glfw.WindowShouldClose(window))
            {
                Glfw.PollEvents(); // reacts to window changes (position etc.)
                ClearScreen();
                Render();
                
                triangle.Scale(multiplier);
                if (triangle.CurrentScale <= 0.5f)
                {
                    multiplier = 1.0001f;
                }
                if (triangle.CurrentScale >= 1f)
                {
                    multiplier = 0.9999f;
                }
                triangle.Move();
                triangle.Rotate(.1f);
                
                triangle2.Scale(multiplier2);
                if (triangle2.CurrentScale <= 0.5f)
                {
                    multiplier2 = 1.001f;
                }
                if (triangle2.CurrentScale >= 1f)
                {
                    multiplier2 = 0.999f;
                }
                triangle2.Move();
                triangle2.Rotate(-.08f);
                
                rectangle.Scale(multiplier);
                if (rectangle.CurrentScale <= 0.5f)
                {
                    multiplier = 1.0001f;
                }
                if (rectangle.CurrentScale >= 1f)
                {
                    multiplier = 0.9999f;
                }
                rectangle.Move();
                rectangle.Rotate(-.05f);
                
                circle.Scale(multiplier2);
                if (circle.CurrentScale <= 0.5f)
                {
                    multiplier = 1.0001f;
                }
                if (circle.CurrentScale >= 1f)
                {
                    multiplier = 0.9999f;
                }
                circle.Move();
                circle.Rotate(-.05f);
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
            triangle2.Render();
            rectangle.Render();
            circle.Render();
            glFlush();
        }

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
            glShaderSource(vertexShader, File.ReadAllText("shaders/triangle.vert"));
            glCompileShader(vertexShader);
            
            var fragmentShader = glCreateShader(GL_FRAGMENT_SHADER);
            glShaderSource(fragmentShader, File.ReadAllText("shaders/triangle.frag"));
            glCompileShader(fragmentShader);
            
            var program = glCreateProgram();
            glAttachShader(program, vertexShader);
            glAttachShader(program, fragmentShader);
            glLinkProgram(program);
            glUseProgram(program);
        }
    }
}
