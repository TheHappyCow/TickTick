using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class Lever : SpriteGameObject
{
    bool idle;

    public Lever(int layer=0, string id="") : base("Sprites/spr_lever", layer, id) 
    {
        idle = true;
    }

    public override void Reset()
    {
        idle = true;
        this.Sprite = new SpriteSheet("Sprites/spr_lever", 0);

        PlayingState playingState = GameEnvironment.GameStateManager.GetGameState("playingState") as PlayingState;
        Level level = playingState.CurrentLevel;
        TileField tiles = GameWorld.Find("tiles") as TileField;
        for (int i = 0; i < level.LevelWidth / tiles.CellWidth; i++)
            for (int j = 0; j < level.LevelHeight / tiles.CellHeight; j++)
            {
                Tile current = tiles.Objects[i, j] as Tile;
                if (current.Lever)
                    current.Visible = true;
            }
    }

    public override void Update(GameTime gameTime)
    {
        //Kijkt of de lever wordt geraakt door het projectiel
        Projectile projectile = GameWorld.Find("projectile") as Projectile;
        if (this.idle && this.CollidesWith(projectile))
        {
            PlayingState playingState = GameEnvironment.GameStateManager.GetGameState("playingState") as PlayingState;
            Level level = playingState.CurrentLevel;
            TileField tiles = GameWorld.Find("tiles") as TileField;
            for (int i = 0; i < level.LevelWidth / tiles.CellWidth; i++)
                for (int j = 0; j < level.LevelHeight / tiles.CellHeight; j++)
                {
                    Tile current = tiles.Objects[i, j] as Tile;
                    if (current.Lever)
                        current.Visible = false;
                }
            this.idle = false;
            this.Sprite = new SpriteSheet("Sprites/spr_levershot", 0);
            GameEnvironment.AssetManager.PlaySound("Sounds/snd_combi");
            projectile.Reset();
        }
        if(!this.idle && this.CollidesWith(projectile))
        {
            PlayingState playingState = GameEnvironment.GameStateManager.GetGameState("playingState") as PlayingState;
            Level level = playingState.CurrentLevel;
            TileField tiles = GameWorld.Find("tiles") as TileField;
            for (int i = 0; i < level.LevelWidth / tiles.CellWidth; i++)
                for (int j = 0; j < level.LevelHeight / tiles.CellHeight; j++)
                {
                    Tile current = tiles.Objects[i, j] as Tile;
                    if (current.Lever)
                        current.Visible = true;
                }
            this.idle = true;
            this.Sprite = new SpriteSheet("Sprites/spr_lever", 0);
            GameEnvironment.AssetManager.PlaySound("Sounds/snd_combi");
            projectile.Reset();
        }
    }
}
