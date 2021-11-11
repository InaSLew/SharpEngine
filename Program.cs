using System.IO;
using System.Runtime.InteropServices;
using GLFW;
using OpenGL;
using static OpenGL.Gl;

namespace SharpEngine
{
    class Program
    {
        static Vertex[] vertices = new Vertex[] {
            new (new Vector(0f, 0f), Color.Red),
            new (new Vector(1f, 0f), Color.Green),
            new (new Vector(0f, 1f), Color.Blue)
        };

        private const int VertexSize = 3;
        private const int Width = 1024;
        private const int Height = 768;

        static void Main(string[] args)
        {
            var window = CreateWindow();
            LoadTriangleIntoBuffer();
            CreateShaderProgram();
            
            var direction = new Vector(0.0003f, 0.0003f);
            var multiplier = 0.999f;
            var scale = 1f;
            while (!Glfw.WindowShouldClose(window))
            {
                Glfw.PollEvents(); // reacts to window changes (position etc.)
                ClearScreen();
                Render();
                // MoveToRight();
                // MoveDown();
                // ShrinkTriangle();
                // ScaleUpTriangle();
                // gonna need sth else here if I remember correctly?
                var min = vertices[0].Position;
                for (var i = 0; i < vertices.Length; i++)
                {
                    min = Vector.Min(min, vertices[i].Position);
                }

                var max = vertices[0].Position;
                for (var i = 0; i < vertices.Length; i++)
                {
                    max = Vector.Max(max, vertices[i].Position);
                }

                var center = (min + max) / 2;
                for (int i = 0; i < vertices.Length; i++)
                {
                    vertices[i].Position -= center;
                }
                for (int i = 0; i < vertices.Length; i++)
                {
                    vertices[i].Position *= multiplier;
                }
                for (int i = 0; i < vertices.Length; i++)
                {
                    vertices[i].Position += center;
                }

                scale *= multiplier;
                if (scale <= 0.5f) {
                    multiplier = 1.001f;
                }
                if (scale >= 1f) {
                    multiplier = 0.999f;
                }

                for (int i = 0; i < vertices.Length; i++)
                {
                    vertices[i].Position += direction;
                }

                for (int i = 0; i < vertices.Length; i++)
                {
                    if ((vertices[i].Position.x >= 1 && direction.x > 0) || (vertices[i].Position.x <= -1 && direction.x < 0))
                    {
                        direction *= -1;
                        break;
                    }
                }
                
                for (int i = 0; i < vertices.Length; i++)
                {
                    if ((vertices[i].Position.y >= 1 && direction.y > 0) || (vertices[i].Position.y <= -1 && direction.y < 0))
                    {
                        direction *= -1;
                        break;
                    }
                }
                
                UpdateTriangleBuffer();
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
            glDrawArrays(GL_TRIANGLES, 0, vertices.Length);
            glFlush();
        }

        private static void ScaleUpTriangle()
        {
            for (var i = 0; i < vertices.Length; i++)
            {
                vertices[i].Position *= 1.00009f;
            }
        }
        
        private static void ShrinkTriangle()
        {
            for (var i = 0; i < vertices.Length; i++)
            {
                vertices[i].Position *= 0.9999f;
            }
        }

        private static void MoveDown()
        {
            for (var i = 0; i < vertices.Length; i++)
            {
                vertices[i].Position.y -= 0.001f;
            }
        }

        private static void MoveToRight()
        {
            for (var i = 0; i < vertices.Length; i++)
            {
                vertices[i].Position.x += 0.001f;
            }
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
        
        private static unsafe void LoadTriangleIntoBuffer()
        {
            // Load vertices into buffer
            var vertexArray = glGenVertexArray();
            var vertexBuffer = glGenBuffer();
            glBindVertexArray(vertexArray);
            glBindBuffer(GL_ARRAY_BUFFER, vertexBuffer);
            UpdateTriangleBuffer();
            glVertexAttribPointer(0, 3, GL_FLOAT, false, sizeof(Vertex), Marshal.OffsetOf(typeof(Vertex), nameof(Vertex.Position)));
            glVertexAttribPointer(1, 4, GL_FLOAT, false, sizeof(Vertex), Marshal.OffsetOf(typeof(Vertex), nameof(Vertex.Color)));
            glEnableVertexAttribArray(0);
            glEnableVertexAttribArray(1);
        }
        
        private static void CreateShaderProgram()
        {
            // create vertex shader
            var vertexShader = glCreateShader(GL_VERTEX_SHADER);
            glShaderSource(vertexShader, File.ReadAllText("shaders/red-triangle.vert"));
            glCompileShader(vertexShader);
            
            // create fragment shader
            var fragmentShader = glCreateShader(GL_FRAGMENT_SHADER);
            glShaderSource(fragmentShader, File.ReadAllText("shaders/red-triangle.frag"));
            glCompileShader(fragmentShader);
            
            // Create shader program - rendering pipeline
            var program = glCreateProgram();
            glAttachShader(program, vertexShader);
            glAttachShader(program, fragmentShader);
            glLinkProgram(program);
            glUseProgram(program);
        }

        private static unsafe void UpdateTriangleBuffer()
        {
            fixed (Vertex* vertex = &vertices[0])
            {
                glBufferData(GL_ARRAY_BUFFER, sizeof(Vertex) * vertices.Length, vertex, GL_STATIC_DRAW);
            }
        }
    }
}
