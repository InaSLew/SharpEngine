using GLFW;
using static OpenGL.Gl;

namespace SharpEngine
{
    public class Window
    {
        private readonly GLFW.Window window;
        private Scene scene;

        public Window()
        {
            Glfw.Init();
            Glfw.WindowHint(Hint.ClientApi, ClientApi.OpenGL);
            Glfw.WindowHint(Hint.ContextVersionMajor, 3);
            Glfw.WindowHint(Hint.ContextVersionMinor, 3);
            Glfw.WindowHint(Hint.Decorated, true);
            Glfw.WindowHint(Hint.OpenglProfile, Profile.Core);
            Glfw.WindowHint(Hint.OpenglForwardCompatible, Constants.True);
            Glfw.WindowHint(Hint.Doublebuffer, Constants.False);

            var tempWindow = Glfw.CreateWindow(1024, 768, "SharpEngine", Monitor.None, GLFW.Window.None);
            Glfw.MakeContextCurrent(tempWindow);
            Import(Glfw.GetProcAddress);
            window = tempWindow;
        }

        public bool IsOpen() => !Glfw.WindowShouldClose(window);
        static void ClearScreen() {
            glClearColor(.2f, .05f, .2f, 1);
            glClear(GL_COLOR_BUFFER_BIT);
        }

        public void Render() {
            Glfw.PollEvents(); // react to window changes (position etc.)
            ClearScreen();
            scene?.Render();
            Glfw.SwapBuffers(window);
        }
    }
}