using System;

namespace SharpEngine
{
    public class Physics
    {
        private readonly Scene scene;
        private const float GravitationalAcceleration = 9.819649f;

        public Physics(Scene scene)
        {
            this.scene = scene;
        }

        public void Update(float deltaTime)
        {
            var gravitationalAcceleration = Vector.Down * GravitationalAcceleration / 100;
            
            var shapes = scene.Shapes;
            for (var i = 0; i < shapes.Count; i++)
            {
                var gameObj = shapes[i];
            
                // linear velocity:
                gameObj.Transform.Position += gameObj.velocity * deltaTime;
                
                // a = Fm
                var acceleration = gameObj.linearForce / gameObj.Mass;
                
                // add gravity to acceleration
                acceleration += gravitationalAcceleration * gameObj.gravityScale;
                
                // linear acceleration
                gameObj.Transform.Position += acceleration * deltaTime * deltaTime / 2;
                gameObj.velocity += acceleration * deltaTime;
            }
        }
    }
}