using System.IO;
using GLFW;
using OpenGL;
using static OpenGL.Gl;

namespace SharpEngine
{
    class Program
    {
        private static float[] vertices =
        {
            -.5f, -.5f, 0f, 1f, 0, 0,
            .5f, -.5f, 0f, 0, 1f, 0,
            0f, .5f, 0f, 0, 0, 1
        };

        private static int NumberOfTriangles = 1;

        static void Main(string[] args)
        {
            var window = CreateWindow();
            LoadTriangleIntoBuffer(vertices);
            var program = CreateShaderProgram();
            test(program);

            // var uniColor = glGetUniformLocation(program, "zeldaMeows");
            // glUniform3f(uniColor, 1f, 0, 0);

            // Rendering loop
            while (!Glfw.WindowShouldClose(window))
            {
                Glfw.PollEvents(); // reacts to window changes (position etc.)
                glClearColor(0,0,0, 1);
                glClear(GL_COLOR_BUFFER_BIT);
                glDrawArrays(GL_TRIANGLES, 0, 3 * NumberOfTriangles);
                glFlush();

                // MoveToRight();
                // MoveDown();
                // ShrinkTriangle();
                // ScaleUpTriangle();

                UpdateTriangleBuffer(vertices);
            }
            Glfw.Terminate();
        }

        private static void ScaleUpTriangle()
        {
            for (var i = 0; i < vertices.Length; i++)
            {
                vertices[i] *= 1.00009f;
            }
        }
        
        private static void ShrinkTriangle()
        {
            for (var i = 0; i < vertices.Length; i++)
            {
                vertices[i] *= 0.9999f;
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
        
        private static unsafe void LoadTriangleIntoBuffer(float[] vertices)
        {
            // Load vertices into buffer
            var vertexArray = glGenVertexArray();
            var vertexBuffer = glGenBuffer();
            glBindVertexArray(vertexArray);
            glBindBuffer(GL_ARRAY_BUFFER, vertexBuffer);
            UpdateTriangleBuffer(vertices);
            glVertexAttribPointer(0, 3, GL_FLOAT, false, 3 * sizeof(float), NULL);
            glEnableVertexAttribArray(0);
        }

        private static unsafe void test(uint program)
        {
            var posAttrib = (uint)glGetAttribLocation(program, "pos");
            glVertexAttribPointer(posAttrib, 3, GL_FLOAT, false, 6 * sizeof(float), NULL);
            glEnableVertexAttribArray(posAttrib);

            var colAttrib = (uint) glGetAttribLocation(program, "color");
            glVertexAttribPointer(colAttrib, 3, GL_FLOAT, false, 6 * sizeof(float), (void*)(3*sizeof(float)));
            glEnableVertexAttribArray(colAttrib);
        }
        
        private static uint CreateShaderProgram()
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
            return program;
        }

        private static unsafe void UpdateTriangleBuffer(float[] tempVertices)
        {
            fixed (float* vertex = &tempVertices[0])
            {
                glBufferData(GL_ARRAY_BUFFER, sizeof(float) * vertices.Length, vertex, GL_STATIC_DRAW);
            }
        }
    }
}
