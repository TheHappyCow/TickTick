using Microsoft.Xna.Framework;

class Button : SpriteGameObject
{
    protected bool pressed;

    public Button(string imageAsset, int layer = 0, string id = "")
        : base(imageAsset, layer, id)
    {
        pressed = false;
    }

    //Controleer of iemand op de button drukt
    public override void HandleInput(InputHelper inputHelper)
    {
        pressed = inputHelper.MouseLeftButtonPressed() &&
            BoundingBox.Contains((int)inputHelper.MousePosition.X - (int)GameEnvironment.Camera.CameraPositionX, (int)inputHelper.MousePosition.Y - (int)GameEnvironment.Camera.CameraPositionY);
    }

    public override void Reset()
    {
        base.Reset();
        pressed = false;
    }

    public bool Pressed
    {
        get { return pressed; }
    }
}
