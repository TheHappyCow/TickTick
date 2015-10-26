using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

    public float CameraPosition
    {
        get { return cameraPosition.X; }
        set { cameraPosition.X = value; }
    }
}