
using System.IO;
using GLFW;
using static OpenGL.Gl;
namespace SharpEngine
{
    public class Material
    {
        private readonly uint program;

        public Material(string vertexShaderPath, string fragmentShaderPath)
        {
            var vertexShader = glCreateShader(GL_VERTEX_SHADER);
            glShaderSource(vertexShader, File.ReadAllText(vertexShaderPath));
            glCompileShader(vertexShader);
            
            var fragmentShader = glCreateShader(GL_FRAGMENT_SHADER);
            glShaderSource(fragmentShader, File.ReadAllText(fragmentShaderPath));
            glCompileShader(fragmentShader);
            
            program = glCreateProgram();
            glAttachShader(program, vertexShader);
            glAttachShader(program, fragmentShader);
            glLinkProgram(program);
            glDeleteShader(vertexShader);
            glDeleteShader(fragmentShader);
        }

        public void Use() => glUseProgram(program);

        public unsafe void SetTransform(Matrix matrix)
        {
            int transformLocation = glGetUniformLocation(program, "transform");
            glUniformMatrix4fv(transformLocation, 1, true, &matrix.m11);
        }
    }
}