﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class GameEnvironment : Game
{
    protected GraphicsDeviceManager graphics;
    protected SpriteBatch spriteBatch;
    protected InputHelper inputHelper;
    protected static Matrix spriteScale;
    
    protected static Point screen;
    protected static GameStateManager gameStateManager;
    protected static Random random;
    protected static AssetManager assetManager;
    protected static GameSettingsManager gameSettingsManager;
    protected static Camera camera;

    public GameEnvironment()
    {
        graphics = new GraphicsDeviceManager(this);

        inputHelper = new InputHelper();
        gameStateManager = new GameStateManager();
        spriteScale = Matrix.CreateScale(1, 1, 1);
        random = new Random();
        assetManager = new AssetManager(Content);
        gameSettingsManager = new GameSettingsManager();
    }

    public static Point Screen
    {
        get { return GameEnvironment.screen; }
        set { screen = value; }
    }

    public static Random Random
    {
        get { return random; }
    }

    public static AssetManager AssetManager
    {
        get { return assetManager; }
    }

    public static GameStateManager GameStateManager
    {
        get { return gameStateManager; }
    }

    public static GameSettingsManager GameSettingsManager
    {
        get { return gameSettingsManager; }
    }

    public static Camera Camera
    {
        get { return camera; }
    }

    //Zet het scherm op fullscreen en schaalt de sprites dusdanig dat ze correct op het scherm worden getekend
    public void SetFullScreen(bool fullscreen = true)
    {
        //Berekent de huidige schaal van het scherm
        float scalex = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / (float)screen.X;
        float scaley = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / (float)screen.Y;
        float finalscale = 1f;

        if (!fullscreen)
        {
            if (scalex < 1f || scaley < 1f)
                finalscale = Math.Min(scalex, scaley);
        }
        else
        {
            finalscale = scalex;
            if (Math.Abs(1 - scaley) < Math.Abs(1 - scalex))
                finalscale = scaley;
        }

        graphics.PreferredBackBufferWidth = (int)(finalscale * screen.X);
        graphics.PreferredBackBufferHeight = (int)(finalscale * screen.Y);
        graphics.IsFullScreen = fullscreen;
        graphics.ApplyChanges();
        //Zorgt ervoor dat de muis ook goed geschaald wordt
        inputHelper.Scale = new Vector2((float)GraphicsDevice.Viewport.Width / screen.X,
                                        (float)GraphicsDevice.Viewport.Height / screen.Y);
        spriteScale = Matrix.CreateScale(inputHelper.Scale.X, inputHelper.Scale.Y, 1);
    }

    protected override void LoadContent()
    {
        DrawingHelper.Initialize(this.GraphicsDevice);
        spriteBatch = new SpriteBatch(GraphicsDevice);
        camera = new Camera();
    }

    protected void HandleInput()
    {
        inputHelper.Update();
        //Zorgt ervoor dat we het scherm kunnen sluiten ook al staat hij op fullscreen
        if (inputHelper.KeyPressed(Keys.Escape))
            this.Exit();
        //Wisselt tussen fullscreen en windowmode
        if (inputHelper.KeyPressed(Keys.F5))
            SetFullScreen(!graphics.IsFullScreen);
        gameStateManager.HandleInput(inputHelper);
    }

    protected override void Update(GameTime gameTime)
    {
        HandleInput();
        gameStateManager.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        IGameLoopObject currentGameState = GameEnvironment.GameStateManager.CurrentGameState;
        IGameLoopObject playingState = GameEnvironment.GameStateManager.GetGameState("playingState") as IGameLoopObject;
        IGameLoopObject gameOverState = GameEnvironment.GameStateManager.GetGameState("gameOverState") as IGameLoopObject;
        IGameLoopObject levelFinishedState = GameEnvironment.GameStateManager.GetGameState("levelFinishedState") as IGameLoopObject;
        
        GraphicsDevice.Clear(Color.Black);
        if (currentGameState == playingState || currentGameState == gameOverState || currentGameState == levelFinishedState)
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.Transformation());
        else
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, spriteScale);
        gameStateManager.Draw(gameTime, spriteBatch);
        spriteBatch.End();
    }

    public static Matrix SpriteScale
    {
        get { return spriteScale; }
    }
}