using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

enum TileType
{
    Background,
    Normal,
    Platform
}

class Tile : SpriteGameObject
{
    protected TileType type;
    protected bool hot;
    protected bool ice;
    protected bool lever;

    public Tile(string assetname = "", TileType tp = TileType.Background, int layer = 0, string id = "")
        : base(assetname, layer, id)
    {
        type = tp;
        hot = false;
        ice = false;
        lever = false;
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (type == TileType.Background)
            return;
        base.Draw(gameTime, spriteBatch);
    }

    public TileType TileType
    {
        get { return type; }
    }

    public bool Hot
    {
        get { return hot; }
        set { hot = value; }
    }

    public bool Ice
    {
        get { return ice; }
        set { ice = value; }
    }

    public bool Lever
    {
        get { return lever; }
        set { lever = value; }
    }
}
