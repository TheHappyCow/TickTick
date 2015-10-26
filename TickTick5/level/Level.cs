using Microsoft.Xna.Framework;
using System.Collections.Generic;

partial class Level : GameObjectList
{
    protected bool locked, solved;
    protected Button quitButton;
    Dictionary<int, Vector2> mountainpositions;

    public Level(int levelIndex)
    {
        // load the backgrounds
        GameObjectList backgrounds = new GameObjectList(0, "backgrounds");
        SpriteGameObject background_main = new SpriteGameObject("Backgrounds/spr_sky", 0, "background");
        background_main.Position = new Vector2(0, GameEnvironment.Screen.Y - background_main.Height);
        backgrounds.Add(background_main);

        // add a few random mountains
        GameObjectList mountainList = new GameObjectList(1, "mountainList");
        mountainpositions = new Dictionary<int, Vector2>(); 
        for (int i = 0; i < 5; i++)
        {
            SpriteGameObject mountain = new SpriteGameObject("Backgrounds/spr_mountain_" + (GameEnvironment.Random.Next(2) + 1), (i % 3) + 1, "mountain" + (i+1));
            mountain.Position = new Vector2((float)GameEnvironment.Random.NextDouble() * GameEnvironment.Screen.X - mountain.Width / 2, GameEnvironment.Screen.Y - mountain.Height);
            mountainpositions.Add(i, mountain.Position);
            mountainList.Add(mountain);
        }
        backgrounds.Add(mountainList);

        //Creëert de wolkjes en plaatst ze in de achtergrond
        Clouds clouds = new Clouds(2);
        backgrounds.Add(clouds);
        this.Add(backgrounds);

        SpriteGameObject timerBackground = new SpriteGameObject("Sprites/spr_timer", 100, "timerBackground");
        timerBackground.Position = new Vector2(10, 10);
        this.Add(timerBackground);
        TimerGameObject timer = new TimerGameObject(101, "timer");
        timer.Position = new Vector2(25, 30);
        this.Add(timer);

        quitButton = new Button("Sprites/spr_button_quit", 100);
        quitButton.Position = new Vector2(GameEnvironment.Screen.X - quitButton.Width - 10, 10);
        this.Add(quitButton);

        this.Add(new GameObjectList(1, "waterdrops"));
        this.Add(new GameObjectList(2, "enemies"));

        //Laadt de tekst waarin het level beschreven staat
        this.LoadTiles("Content/Levels/" + levelIndex + ".txt");

        Player player = GameWorld.Find("player") as Player;
        Vector2 startPosition = player.GlobalPosition - new Vector2(0, player.Center.Y);
        Projectile projectile = new Projectile(startPosition);
        this.Add(projectile);
    }

    //Kijkt of het level voltooid is
    public bool Completed
    {
        get
        {
            //Hij komt bij de finish aan en heeft alle waterdrops
            SpriteGameObject exitObj = this.Find("exit") as SpriteGameObject;
            Player player = this.Find("player") as Player;
            if (!exitObj.CollidesWith(player))
                return false;
            GameObjectList waterdrops = this.Find("waterdrops") as GameObjectList;
            foreach (GameObject d in waterdrops.Objects)
                if (d.Visible)
                    return false;
            return true;
        }
    }

    public bool GameOver
    {
        get
        {
            TimerGameObject timer = this.Find("timer") as TimerGameObject;
            Player player = this.Find("player") as Player;
            return !player.IsAlive || timer.GameOver;
        }
    }

    //Dit level mag nog niet gespeeld worden
    public bool Locked
    {
        get { return locked; }
        set { locked = value; }
    }

    //Dit level is voltooid
    public bool Solved
    {
        get { return solved; }
        set { solved = value; }
    }

    public Dictionary<int, Vector2> MountainPositions
    {
        get { return mountainpositions; }
    }
}

