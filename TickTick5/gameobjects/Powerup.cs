using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class Powerup : SpriteGameObject
{
    protected float bounce;

    public Powerup(int layer = 0, string id = "")
        : base("Sprites/spr_powerup", layer, id)
    {
    }

    public override void Update(GameTime gameTime)
    {
        //Laat de powerup op en neer stuiteren
        double t = gameTime.TotalGameTime.TotalSeconds * 3.0f + Position.X;
        bounce = (float)Math.Sin(t) * 0.2f;
        position.Y += bounce;
        //Kijkt of de speler de powerup raakt
        Player player = GameWorld.Find("player") as Player;
        if (this.visible && this.CollidesWith(player))
        {
            //De powerup is opgepakt door de speler
            this.visible = false;

            int p = (int)GameEnvironment.Random.Next(2);
            PlayingState playingState = GameEnvironment.GameStateManager.GetGameState("playingState") as PlayingState;
            Level level = playingState.CurrentLevel;
            TimerGameObject timer = level.Find("timer") as TimerGameObject;
            //Je krijgt er tijd bij
            timer.TimeLeft += TimeSpan.FromSeconds(10);
            GameEnvironment.AssetManager.PlaySound("Sounds/snd_watercollected");
        }
    }
}