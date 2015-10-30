using Microsoft.Xna.Framework;

class Rocket : AnimatedGameObject
{
    protected double spawnTime;
    protected Vector2 startPosition;

    public Rocket(bool moveToLeft, Vector2 startPosition)
    {
        this.LoadAnimation("Sprites/Rocket/spr_rocket@3", "default", true, 0.2f);
        this.PlayAnimation("default");
        this.Mirror = moveToLeft;
        this.startPosition = startPosition;
        Reset();
    }

    public override void Reset()
    {
        this.Visible = false;
        this.position = startPosition;
        this.velocity = Vector2.Zero;
        this.spawnTime = GameEnvironment.Random.NextDouble() * 5;
    }

    public override void Update(GameTime gameTime)
    {
        PlayingState playingState = GameEnvironment.GameStateManager.GetGameState("playingState") as PlayingState;
        Level level = playingState.CurrentLevel;

        base.Update(gameTime);
        if (spawnTime > 0)
        {
            spawnTime -= gameTime.ElapsedGameTime.TotalSeconds;
            return;
        }
        this.Visible = true;
        this.velocity.X = 600;
        if (Mirror)
            this.velocity.X *= -1f;
        CheckPlayerCollision();
        // check if we are outside the screen
        Rectangle screenBox = new Rectangle(0, 0, (int)level.LevelWidth, (int)level.LevelHeight);
        if (!screenBox.Intersects(this.BoundingBox))
            this.Reset();
    }

    //Controleert of hij de speler raakt
    public void CheckPlayerCollision()
    {
        Player player = GameWorld.Find("player") as Player;
        if (this.CollidesWith(player) && this.Visible)
        {
            if (player.BoundingBox.Bottom + player.GlobalPosition.Y <= BoundingBox.Top + GlobalPosition.Y && player.IsAlive)
            {
                //De raket gaat "dood"
                player.Jump(700);
                this.Reset();
            }
            else
                player.Die(false);
        }
    }
}
