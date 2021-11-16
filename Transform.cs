using System;

namespace SharpEngine
{
    public class Transform
    {
        public Vector CurrentScale { get; private set; }
        public Vector Position { get; private set; }
        public Vector Rotation { get; private set; }
        public Matrix Matrix =>  Matrix.Translate(Position) * Matrix.Rotate(Rotation) * Matrix.Scale(CurrentScale);
        public Transform()
        {
            CurrentScale = new Vector(1f, 1f, 1f);
        }
        
        public void Move(Vector direction)
        {
            Position += direction;
        }

        public void Scale(float multiplier)
        {
            CurrentScale *= multiplier;
        }
        
        public virtual void Rotate(float zAngle)
        {
            var rotation = Rotation;
            rotation.z += zAngle;
            Rotation = rotation;
        }
    }
}