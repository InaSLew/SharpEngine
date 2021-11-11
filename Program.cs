using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using GLFW;
using GlmNet;
using OpenGL;
using static OpenGL.Gl;

namespace SharpEngine
{
    struct Vector
    {
        internal float x, y, z;

        public Vector(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector(float x, float y)
        {
            this.x = x;
            this.y = y;
            this.z = 0;
        }

        public static Vector operator *(Vector v, float f) => new (v.x * f, v.y * f, v.z * f);
        public static Vector operator /(Vector v, float f) => new (v.x / f, v.y / f, v.z / f);
        public static Vector operator +(Vector v0, Vector v1) => new(v0.x + v1.x, v0.y + v1.y, v0.z + v1.z);
        public static Vector operator -(Vector v0, Vector v1) => new(v0.x - v1.x, v0.y - v1.y, v0.z - v1.z);
        public static Vector Max(Vector a, Vector b) => new (MathF.Max(a.x, b.x),MathF.Max(a.y, b.y));
        public static Vector Min(Vector a, Vector b) => new (MathF.Min(a.x, b.x),MathF.Min(a.y, b.y));
    }
    
    class Program
    {
        // private static float[] vertices =
        // {
        //     -1f, -.5f, 0f, 
        //     0f, -.5f, 0f, 
        //     -.5f, .5f, 0f, 
        //
        //     0f, -.5f, 0f, 
        //     1f, -.5f, 0f, 
        //     .5f, .5f, 0f 
        //     // -0.5f,  0.5f, 0, 1.0f, 0.0f, 0.0f, // Top-left
        //     // 0.5f,  0.5f, 0, 0.0f, 1.0f, 0.0f, // Top-right
        //     // 0.5f, -0.5f, 0, 0.0f, 0.0f, 1.0f, // Bottom-right
        //     // -0.5f, -0.5f, 0, 1.0f, 1.0f, 1.0f, // Bottom-left
        // };

        private static Vector[] vertices =
        {
            // new (-.1f, -.1f),
            // new (.1f, -.1f),
            // new (0f, .1f),
            new (.4f, .4f),
            new (.6f, .4f),
            new (.5f, .6f)
        };

        private const int VertexSize = 3;
        private const int Width = 1024;
        private const int Height = 768;
        private static bool hasTouchRight;
        private static bool hasTouchTop;

        private static Stopwatch timer;

        static void Main(string[] args)
        {
            var window = CreateWindow();
            LoadTriangleIntoBuffer(vertices);
            var program = CreateShaderProgram();
            
            // Rendering loop
            Render(window);

            Glfw.Terminate();
        }

        private static void Render(Window window)
        {
            timer = Stopwatch.StartNew();
            var direction = new Vector(0.0003f, 0f);
            var multiplier = 0.999f;
            var scale = 1f;
            while (!Glfw.WindowShouldClose(window))
            {
                Glfw.PollEvents(); // reacts to window changes (position etc.)
                glClearColor(0,0,0, 1);
                glClear(GL_COLOR_BUFFER_BIT);
                glDrawArrays(GL_TRIANGLES, 0, vertices.Length);
                glFlush();

                // MoveToRight();
                // MoveDown();
                // ShrinkTriangle();
                // ScaleUpTriangle();
                // gonna need sth else here if I remember correctly?
                var min = vertices[0];
                for (var i = 0; i < vertices.Length; i++)
                {
                    min = Vector.Min(min, vertices[i]);
                }

                var max = vertices[0];
                for (var i = 0; i < vertices.Length; i++)
                {
                    max = Vector.Max(max, vertices[i]);
                }

                var center = (min + max) / 2;
                for (int i = 0; i < vertices.Length; i++)
                {
                    vertices[i] -= center;
                }
                for (int i = 0; i < vertices.Length; i++)
                {
                    vertices[i] *= multiplier;
                }
                for (int i = 0; i < vertices.Length; i++)
                {
                    vertices[i] += center;
                }

                scale *= multiplier;
                if (scale <= 0.5f) {
                    multiplier = 1.001f;
                }
                if (scale >= 1f) {
                    multiplier = 0.999f;
                }
                // GoTopRightAndBounceWhenHitBorder(direction);
                UpdateTriangleBuffer();
            }
        }

        // private static unsafe void RotateTriangle(float deltaTime)
        // {
        //     var trans = new mat4(1f);
        //     var tmp = new vec3(0, 0, 1f);
        //     trans = glm.rotate(trans, deltaTime * glm.radians(180f), tmp);
        //     var test = trans.to_array();
        //
        //     fixed (float* t = new float[test.Length])
        //     {
        //         for (int i = 0; i < test.Length; i++)
        //         {
        //             t[i] = test[i];
        //         }
        //         glUniformMatrix4fv(uniTrans, 1, false, t);
        //     }
        // }

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
                vertices[i].y -= 0.001f;
            }
        }

        private static void MoveToRight()
        {
            for (var i = 0; i < vertices.Length; i++)
            {
                vertices[i].x += 0.001f;
            }
        }
        
        private static void GoTopRightAndBounceWhenHitBorder(Vector direction)
        {
            // Refactor so it uses multiply with -1 to revert direction
            // 
            for (var i = 0; i < vertices.Length; i++)
            {
                // vertices[i] += new Vector(hasTouchRight ? -0.0005f : 0.0005f, hasTouchTop ? -0.0005f : 0.0005f);
                vertices[i] += direction;
            }

            for (var i = 0; i < vertices.Length; i++)
            {
                if (vertices[i].x >= 1f || vertices[i].x <= -1f)
                {
                    vertices[i] += direction * -1;
                }
            }
            // if (vertices.Any(v => v.x > 1f)) hasTouchRight = true;
            // if (vertices.Any(v => v.x < -1f)) hasTouchRight = false;
            // if (vertices.Any(v => v.y > 1f)) hasTouchTop = true;
            // if (vertices.Any(v => v.y < -1f)) hasTouchTop = false;
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
        
        private static unsafe void LoadTriangleIntoBuffer(Vector[] vertices)
        {
            // Load vertices into buffer
            var vertexArray = glGenVertexArray();
            var vertexBuffer = glGenBuffer();
            // var elementBuffer = glGenBuffer();
            glBindVertexArray(vertexArray);
            glBindBuffer(GL_ARRAY_BUFFER, vertexBuffer);
            // glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, elementBuffer);
            UpdateTriangleBuffer();
            // UpdateElementBuffer();
            glVertexAttribPointer(0, VertexSize, GL_FLOAT, false, sizeof(Vector), NULL);
            glEnableVertexAttribArray(0);
        }

        private static unsafe void test(uint program)
        {
            var posAttrib = (uint)glGetAttribLocation(program, "pos");
            glVertexAttribPointer(posAttrib, 3, GL_FLOAT, false, 6 * sizeof(float), NULL);
            glEnableVertexAttribArray(posAttrib);

            var colAttrib = (uint) glGetAttribLocation(program, "color");
            glVertexAttribPointer(colAttrib, 3, GL_FLOAT, false, 6 * sizeof(float), (void*)(3 * sizeof(float)));
            glEnableVertexAttribArray(colAttrib);

            // uniTrans = glGetUniformLocation(program, "trans");
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

        private static unsafe void UpdateTriangleBuffer()
        {
            fixed (Vector* vertex = &vertices[0])
            {
                glBufferData(GL_ARRAY_BUFFER, sizeof(Vector) * vertices.Length, vertex, GL_STATIC_DRAW);
            }
        }
    }
}
