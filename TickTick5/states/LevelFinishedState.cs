using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class LevelFinishedState : GameObjectList
{
    protected IGameLoopObject playingState;
    SpriteGameObject overlay;

    public LevelFinishedState()
    {
        playingState = GameEnvironment.GameStateManager.GetGameState("playingState");
        overlay = new SpriteGameObject("Overlays/spr_welldone");
        overlay.Position = new Vector2(GameEnvironment.Screen.X, GameEnvironment.Screen.Y) / 2 - overlay.Center;
        this.Add(overlay);
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        //Als er op spatie wordt gedrukt, kan je verder spelen met het volgende level
        if (!inputHelper.KeyPressed(Keys.Space))
            return;
        GameEnvironment.GameStateManager.SwitchTo("playingState");
        (playingState as PlayingState).NextLevel();
    }

    public override void Update(GameTime gameTime)
    {
        playingState.Update(gameTime);
        overlay.Position = new Vector2(-GameEnvironment.Camera.CameraPosition + GameEnvironment.Screen.X / 2, GameEnvironment.Screen.Y / 2) - overlay.Center;
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        playingState.Draw(gameTime, spriteBatch);
        base.Draw(gameTime, spriteBatch);
    }
}