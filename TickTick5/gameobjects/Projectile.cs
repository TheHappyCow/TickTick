using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class Projectile: SpriteGameObject
{
    bool shooting;
    Player player;
    Vector2 startPosition;

    public Projectile(Vector2 start)
        : base("Sprites/Player/spr_ball", 100, "projectile")
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
            //Zo overlappen de speler en het projectiel elkaar niet
            if (!player.BoundingBox.Contains(BoundingBox))
                visible = true;
        }
        else
        {   
            //Als er niet geschoten wordt, moet het balletje onder de speler sprite blijven
            position = player.GlobalPosition - new Vector2(0, player.Center.Y);
        }
        //Controleren of het uit het scherm gevlogen is
        Rectangle screenBox = new Rectangle(0, 0, (int)level.LevelWidth, (int)level.LevelHeight);
        if (!screenBox.Intersects(this.BoundingBox))
            this.Reset();
        CheckCollision();
    }

    //Kijkt of het projectiel iets geraakt heeft (vijanden of tiles)
    public void CheckCollision()
    {
        //Controleert enemy collision
        GameObjectList enemies = GameWorld.Find("enemies") as GameObjectList;
        for (int i = 0; i < enemies.Objects.Count; i++ )
            if (this.CollidesWith(enemies.Objects[i] as SpriteGameObject) && this.Visible)
            {
                if (enemies.Objects[i] is Rocket)
                {
                    //Een raket moet exploderen als hij geraakt wordt door een projectiel
                    Rocket rocket = enemies.Objects[i] as Rocket;
                    rocket.Explode = true;
                }
                else
                    enemies.Objects[i].Reset();
                this.Reset();
            }

        //Controleert of het een walltile raakt
        TileField tiles = GameWorld.Find("tiles") as TileField;
        int x = (int)this.Position.X / tiles.CellWidth;
        int y = (int)this.Position.Y / tiles.CellHeight;
        if (player.IsAlive && x < tiles.Columns && y < tiles.Rows && x > 0 && y > 0)
        {
            Tile current = tiles.Objects[x, y] as Tile;
            if (current.TileType != TileType.Background && current.TileType != TileType.Platform && current.Visible)
            {
                this.Reset();
            }
        }
    }
}