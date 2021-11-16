using System;

namespace SharpEngine
{
    public class Transform
    {
        public Vector CurrentScale { get; set; }
        public Vector Position { get; set; }
        public Vector Rotation { get; set; }
        public Matrix Matrix =>  Matrix.Translate(Position) * Matrix.Rotate(Rotation) * Matrix.Scale(CurrentScale);
        public Vector Forward => Matrix.Transform(Matrix, Vector.Forward, 0);
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
        
        public virtual void Rotate(Vector r)
        {
            var rotation = Rotation;
            rotation.x += r.x;
            rotation.y += r.y;
            rotation.z += r.z;
            Rotation = rotation;
        }
    }
}