using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class SpriteGameObject : GameObject
{
    protected SpriteSheet sprite;
    protected Vector2 origin;

    public SpriteGameObject(string assetname, int layer = 0, string id = "", int sheetIndex = 0)
        : base(layer, id)
    {
        if (assetname != "")
            sprite = new SpriteSheet(assetname, sheetIndex);
        else
            sprite = null;
    }    

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (!visible || sprite == null)
            return;
        sprite.Draw(spriteBatch, this.GlobalPosition, origin);
    }

    public SpriteSheet Sprite
    {
        get { return sprite; }
    }

    public Vector2 Center
    {
        get { return new Vector2(Width, Height) / 2; }
    }

    public int Width
    {
        get
        {
            return sprite.Width;
        }
    }

    public int Height
    {
        get
        {
            return sprite.Height;
        }
    }

    //Hiermee kan je de sprite spiegelen
    public bool Mirror
    {
        get { return sprite.Mirror; }
        set { sprite.Mirror = value; }
    }

    public Vector2 Origin
    {
        get { return this.origin; }
        set { this.origin = value; }
    }

    //Een geupdate versie van de property BoundingBox van de GameObject klasse (met nieuwe grote van de rectangle)
    public override Rectangle BoundingBox
    {
        get
        {
            int left = (int)(GlobalPosition.X - origin.X);
            int top = (int)(GlobalPosition.Y - origin.Y);
            return new Rectangle(left, top, Width, Height);
        }
    }

    //Controleert of het object botst/overlapt met een ander object
    public bool CollidesWith(SpriteGameObject obj)
    {
        //Ze botsen/overlappen niet als sowieso een van de twee onzichtbaar is of als de twee boundingboxes niet overlappen
        if (!this.Visible || !obj.Visible || !BoundingBox.Intersects(obj.BoundingBox))
            return false;
        //Roept methode aan die controleert over welk gebied de twee boundingboxes met elkaar overlappen
        Rectangle b = Collision.Intersection(BoundingBox, obj.BoundingBox);
        //Gaat dit gebied af en controleert per pixel of er overlap is
        for (int x = 0; x < b.Width; x++)
            for (int y = 0; y < b.Height; y++)
            {
                int thisx = b.X - (int)(GlobalPosition.X - origin.X) + x;
                int thisy = b.Y - (int)(GlobalPosition.Y - origin.Y) + y;
                int objx = b.X - (int)(obj.GlobalPosition.X - obj.origin.X) + x;
                int objy = b.Y - (int)(obj.GlobalPosition.Y - obj.origin.Y) + y;
                if (sprite.GetPixelColor(thisx, thisy).A != 0
                    && obj.sprite.GetPixelColor(objx, objy).A != 0)
                    return true;
            }
        return false;
    }
}

