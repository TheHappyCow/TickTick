using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class Projectile: SpriteGameObject
{
    bool shooting;
    Player player;
    Vector2 startPosition;

    public Projectile(Vector2 start)
        : base("Sprites/Player/spr_ball_blue", 100, "projectile")
    {
        startPosition = start;
        visible = false;
        shooting = false;
        velocity = Vector2.Zero;
    }

    public override void Reset()
    {
        visible = false;
        shooting = false;
        player = GameWorld.Find("player") as Player;
        position = player.GlobalPosition - new Vector2(0, player.Center.Y);
        velocity = Vector2.Zero;
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        if (!shooting)
        {
            if (inputHelper.KeyPressed(Keys.Right))
            {
                //schieten naar rechts
                shooting = true;
                velocity = new Vector2(900, 0);
            }
            if (inputHelper.KeyPressed(Keys.Left))
            {
                //schieten naar links
                shooting = true;
                velocity = new Vector2(-900, 0);
            }
            if (inputHelper.KeyPressed(Keys.Up))
            {
                //schieten naar boven
                shooting = true;
                velocity = new Vector2(0, -900);
            }
        }
    }

    public override void Update(GameTime gameTime)
    {
        player = GameWorld.Find("player") as Player;
        PlayingState playingState = GameEnvironment.GameStateManager.GetGameState("playingState") as PlayingState;
        Level level = playingState.CurrentLevel;
        base.Update(gameTime);
        if (shooting)
        {
            if (!player.BoundingBox.Contains(BoundingBox))
                visible = true;
        }
        else
        {
            position = player.GlobalPosition - new Vector2(0, player.Center.Y);
        }
        Rectangle screenBox = new Rectangle(0, 0, (int)level.LevelWidth, (int)level.LevelHeight);
        if (!screenBox.Intersects(this.BoundingBox))
            this.Reset();
        CheckCollision();
    }

    public void CheckCollision()
    {
        GameObjectList enemies = GameWorld.Find("enemies") as GameObjectList;
        for (int i = 0; i < enemies.Objects.Count; i++ )
            if (this.CollidesWith(enemies.Objects[i] as SpriteGameObject) && this.Visible)
            {
                if (enemies.Objects[i] is Rocket)
                {
                    Rocket rocket = enemies.Objects[i] as Rocket;
                    rocket.Explode = true;
                }
                else
                    enemies.Objects[i].Reset();
                this.Reset();
            }

        TileField tiles = GameWorld.Find("tiles") as TileField;
        int x = (int)this.Position.X / tiles.CellWidth;
        int y = (int)this.Position.Y / tiles.CellHeight;
        if (player.IsAlive && x < tiles.Columns && y < tiles.Rows && x > 0 && y > 0)
        {
            Tile current = tiles.Objects[x, y] as Tile;
            if (current.TileType != TileType.Background && current.TileType != TileType.Platform)
            {
                this.Reset();
            }
        }
    }
}