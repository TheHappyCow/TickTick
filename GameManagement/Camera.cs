using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Camera
{
    Vector2 cameraPosition;
    Viewport viewport;
    Matrix camera;
    
    public Camera(Viewport viewp)
    {
        this.viewport = viewp;
        cameraPosition = Vector2.Zero;
    }
    
    public void MovePosition(Vector2 value)
    {
        cameraPosition += value;
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