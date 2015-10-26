using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

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

        GameObjectList hintfield = this.Find("hintfield") as GameObjectList;
        SpriteGameObject hint_frame = hintfield.Find("hint_frame") as SpriteGameObject;
        SpriteGameObject timerBackground = this.Find("timerBackground") as SpriteGameObject;
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

        GameEnvironment.Camera.CameraPosition = -MathHelper.Clamp(player.GlobalPosition.X - (GameEnvironment.Screen.X - player.Width) / 2, 0, levelwidth - GameEnvironment.Screen.X);
        //Laat bepaalde dingen met de camera meebewegen
        quitButton.Position = new Vector2(-GameEnvironment.Camera.CameraPosition + GameEnvironment.Screen.X - quitButton.Width - 10, 10);
        hintfield.Position = new Vector2(-GameEnvironment.Camera.CameraPosition + (GameEnvironment.Screen.X - hint_frame.Width) / 2, 10);
        timerBackground.Position = new Vector2(-GameEnvironment.Camera.CameraPosition + 10, 10);
        timer.Position = new Vector2(-GameEnvironment.Camera.CameraPosition + 25, 30);
    }

    public override void Reset()
    {
        base.Reset();
        VisibilityTimer hintTimer = this.Find("hintTimer") as VisibilityTimer;
        hintTimer.StartVisible();
    }
}
