using Microsoft.Xna.Framework;
using System.Collections.Generic;

partial class Level : GameObjectList
{

    public override void HandleInput(InputHelper inputHelper)
    {
        base.HandleInput(inputHelper);
        if (quitButton.Pressed)
        {
            this.Reset();
            GameEnvironment.GameStateManager.SwitchTo("levelMenu");
        }      
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        TimerGameObject timer = this.Find("timer") as TimerGameObject;
        Player player = this.Find("player") as Player;

        // check if we died
        if (!player.IsAlive)
            timer.Running = false;

        // check if we ran out of time
        if (timer.GameOver)
            player.Explode();
                       
        // check if we won
        if (this.Completed && timer.Running)
        {
            player.LevelFinished();
            timer.Running = false;
        }
        FollowCamera();
    }

    public override void Reset()
    {
        base.Reset();
        VisibilityTimer hintTimer = this.Find("hintTimer") as VisibilityTimer;
        hintTimer.StartVisible();
    }

    //Zorgt ervoor dat de dingen in het beeld de camera op de juiste manier volgen en dat de camerapositie wordt geupdate
    public void FollowCamera()
    {
        Player player = this.Find("player") as Player;
        GameObjectList hintfield = this.Find("hintfield") as GameObjectList;
        SpriteGameObject hint_frame = hintfield.Find("hint_frame") as SpriteGameObject;
        SpriteGameObject timerBackground = this.Find("timerBackground") as SpriteGameObject;
        TimerGameObject timer = this.Find("timer") as TimerGameObject;
        GameOverState gameOver = GameEnvironment.GameStateManager.GetGameState("gameOverState") as GameOverState;
        LevelFinishedState levelFinished = GameEnvironment.GameStateManager.GetGameState("levelFinishedState") as LevelFinishedState;
        GameObjectList backgroundList = this.Find("backgrounds") as GameObjectList;
        SpriteGameObject background = backgroundList.Find("background") as SpriteGameObject;

        GameEnvironment.Camera.CameraPositionX = -MathHelper.Clamp(player.GlobalPosition.X - (GameEnvironment.Screen.X - player.Width) / 2, 0, levelwidth - GameEnvironment.Screen.X);
        GameEnvironment.Camera.CameraPositionY = -MathHelper.Clamp(player.GlobalPosition.Y - (GameEnvironment.Screen.Y - player.Height) / 2, 0, levelheight - GameEnvironment.Screen.Y);
        //Laat bepaalde dingen met de camera meebewegen
        quitButton.Position = new Vector2(-GameEnvironment.Camera.CameraPositionX + GameEnvironment.Screen.X - quitButton.Width - 10, -GameEnvironment.Camera.CameraPositionY + 10);
        hintfield.Position = new Vector2(-GameEnvironment.Camera.CameraPositionX + (GameEnvironment.Screen.X - hint_frame.Width) / 2, -GameEnvironment.Camera.CameraPositionY + 10);
        timerBackground.Position = new Vector2(-GameEnvironment.Camera.CameraPositionX + 10, -GameEnvironment.Camera.CameraPositionY + 10);
        timer.Position = new Vector2(-GameEnvironment.Camera.CameraPositionX + 25, -GameEnvironment.Camera.CameraPositionY + 30);
        gameOver.Overlay.Position = new Vector2(-GameEnvironment.Camera.CameraPositionX + GameEnvironment.Screen.X / 2, -GameEnvironment.Camera.CameraPositionY + GameEnvironment.Screen.Y / 2) - gameOver.Overlay.Center;
        levelFinished.Overlay.Position = new Vector2(-GameEnvironment.Camera.CameraPositionX + GameEnvironment.Screen.X / 2, -GameEnvironment.Camera.CameraPositionY + GameEnvironment.Screen.Y / 2) - levelFinished.Overlay.Center;
        background.Position = new Vector2(-GameEnvironment.Camera.CameraPositionX, -GameEnvironment.Camera.CameraPositionY + GameEnvironment.Screen.Y - background.Height);
    }
}
