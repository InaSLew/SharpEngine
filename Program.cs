using System;
using GLFW;
using OpenGL;
using static OpenGL.Gl;

namespace SharpEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            Glfw.Init();
            var window = Glfw.CreateWindow(1024, 768, "SharpEngine", Monitor.None, Window.None);
            Glfw.MakeContextCurrent(window);
            
            // draw a triangle (3d coordinates required)
            var vertices = new float[]
            {
                -.5f, -5f, 0f,
                .5f, -.5f, 0f,
                0f, .5f, 0f
            };

            var vertxArray = glGenVertexArray();

            while (!Glfw.WindowShouldClose(window))
            {
                Glfw.PollEvents(); // start listening for events
            }
        }
    }
}
