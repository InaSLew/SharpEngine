using System;

namespace SharpEngine
{
    public class Transform
    {
        public Vector CurrentScale { get; private set; }
        public Vector Position { get; private set; }
        public Matrix Matrix =>  Matrix.Translate(Position) * Matrix.Scale(CurrentScale);
        public Transform()
        {
            CurrentScale = new Vector(1f, 1f, 1f);
        }
        
        public void Move(Vector direction)
        {
            //transform *= Matrix.Translate(direction);
            Position += direction;
        }

        public void Scale(float multiplier)
        {
            CurrentScale *= multiplier;
        }
        
        public virtual void Rotate(float degree)
        {
            // transform *= Matrix.Rotate(GetRadians(degree));
        }
        
        private float GetRadians(float degree) => degree * (MathF.PI / 180f);
    }
}