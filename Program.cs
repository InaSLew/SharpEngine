using System.Diagnostics;
using System.IO;
using GLFW;
using GlmNet;
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
            // -0.5f,  0.5f, 0, 1.0f, 0.0f, 0.0f, // Top-left
            // 0.5f,  0.5f, 0, 0.0f, 1.0f, 0.0f, // Top-right
            // 0.5f, -0.5f, 0, 0.0f, 0.0f, 1.0f, // Bottom-right
            // -0.5f, -0.5f, 0, 1.0f, 1.0f, 1.0f, // Bottom-left
        };

        private static uint[] elements = new uint[6] { 0, 1, 2, 2, 3, 0 };

        private static int NumberOfTriangles = 1;
        private static int uniTrans;
        private static Stopwatch timer;
        private static float angle;

        static void Main(string[] args)
        {
            var window = CreateWindow();
            LoadTriangleIntoBuffer(vertices);
            var program = CreateShaderProgram();
            test(program);
            timer = Stopwatch.StartNew();

            // Rendering loop
            while (!Glfw.WindowShouldClose(window))
            {
                timer.Stop();
                var deltaTime = timer.ElapsedMilliseconds / 1000f;
                timer.Restart();
                
                Glfw.PollEvents(); // reacts to window changes (position etc.)
                glClearColor(0,0,0, 1);
                glClear(GL_COLOR_BUFFER_BIT);

                RotateTriangle(deltaTime);
                
                
                glDrawArrays(GL_TRIANGLES, 0, 3 * NumberOfTriangles);
                // Draw2TrianglesWithArrayElementBuffer();
                glFlush();

                // MoveToRight();
                // MoveDown();
                // ShrinkTriangle();
                // ScaleUpTriangle();

                UpdateTriangleBuffer(vertices);
            }
            //Glfw.Terminate();
        }

        private static unsafe void RotateTriangle(float deltaTime)
        {
            var trans = new mat4(1f);
            var tmp = new vec3(0, 0, 1f);
            trans = glm.rotate(trans, deltaTime * glm.radians(180f), tmp);
            var test = trans.to_array();

            fixed (float* t = new float[test.Length])
            {
                for (int i = 0; i < test.Length; i++)
                {
                    t[i] = test[i];
                }
                glUniformMatrix4fv(uniTrans, 1, false, t);
            }
        }

        private static unsafe void Draw2TrianglesWithArrayElementBuffer()
        {
            fixed (uint* element = &elements[0])
            {
                glDrawElements(GL_TRIANGLES, 6, GL_UNSIGNED_INT, element);
            }
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
            // var elementBuffer = glGenBuffer();
            glBindVertexArray(vertexArray);
            glBindBuffer(GL_ARRAY_BUFFER, vertexBuffer);
            // glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, elementBuffer);
            UpdateTriangleBuffer(vertices);
            // UpdateElementBuffer();
            //glVertexAttribPointer(0, 3, GL_FLOAT, false, 6 * sizeof(float), NULL);
            //glEnableVertexAttribArray(0);
        }

        private static unsafe void test(uint program)
        {
            var posAttrib = (uint)glGetAttribLocation(program, "pos");
            glVertexAttribPointer(posAttrib, 3, GL_FLOAT, false, 6 * sizeof(float), NULL);
            glEnableVertexAttribArray(posAttrib);

            var colAttrib = (uint) glGetAttribLocation(program, "color");
            glVertexAttribPointer(colAttrib, 3, GL_FLOAT, false, 6 * sizeof(float), (void*)(3*sizeof(float)));
            glEnableVertexAttribArray(colAttrib);

            uniTrans = glGetUniformLocation(program, "trans");
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

        private static unsafe void UpdateElementBuffer()
        {
            fixed (uint* element = &elements[0])
            {
                glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(uint) * elements.Length, element, GL_STATIC_DRAW);
            }
        }
    }
}
