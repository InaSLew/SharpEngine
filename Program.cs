using System.IO;
using GLFW;
using static OpenGL.Gl;

namespace SharpEngine
{
    class Program
    {
        private static float[] vertices =
        {
            -.5f, -.5f, 0f,
            .5f, -.5f, 0f,
            0f, .5f, 0f
        };
        
        static void Main(string[] args)
        {
            var window = CreateWindow();
            LoadTriangleIntoBuffer();
            CreateShaderProgram();

            // Rendering loop
            while (!Glfw.WindowShouldClose(window))
            {
                Glfw.PollEvents(); // reacts to window changes (position etc.)
                glClearColor(0,0,0, 1);
                glClear(GL_COLOR_BUFFER_BIT);
                glDrawArrays(GL_TRIANGLES, 0, 3);
                glFlush();

                // MoveToRight();
                MoveDown();
                
                UpdateTriangleBuffer();
            }
        }

        private static void MoveDown()
        {
            for (var i = 0; i < vertices.Length; i++)
            {
                if (i % 3 == 1) vertices[i] -= 0.001f;
            }
        }

        private static void MoveToRight()
        {
            for (var i = 0; i < vertices.Length; i++)
            {
                if (i % 3 == 0) vertices[i] += 0.001f;
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
            var window = Glfw.CreateWindow(1024, 768, "SharpEngine", Monitor.None, Window.None);
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
            glVertexAttribPointer(0, 3, GL_FLOAT, false, 3 * sizeof(float), NULL);
            glEnableVertexAttribArray(0);
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
            fixed (float* vertex = &vertices[0])
            {
                glBufferData(GL_ARRAY_BUFFER, sizeof(float) * vertices.Length, vertex, GL_STATIC_DRAW);
            }
        }
    }
}
