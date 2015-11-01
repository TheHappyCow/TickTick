using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

public class Camera
{
    Vector2 cameraPosition;
    Matrix camera;
    
    public Camera()
    {
        cameraPosition = Vector2.Zero;
    }

    public Matrix Transformation()
    {
        camera = Matrix.CreateTranslation(new Vector3(cameraPosition.X, cameraPosition.Y, 0)) * GameEnvironment.SpriteScale;
        return camera;
    }

    public float CameraPositionX
    {
        get { return cameraPosition.X; }
        set { cameraPosition.X = value; }
    }

    public float CameraPositionY
    {
        get { return cameraPosition.Y; }
        set { cameraPosition.Y = value; }
    }
}