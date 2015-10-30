using System;
using Microsoft.Xna.Framework;

public class Animation : SpriteSheet
{
    protected float frameTime;
    protected bool isLooping;
    protected float time;
    string name;

    public Animation(string assetname, bool isLooping, float frametime = 0.1f) : base(assetname)
    {
        this.frameTime = frametime; //De tijd tussen twee frames van de animatie
        this.isLooping = isLooping;
        name = assetname;
    }

    public void Play()
    {
        this.sheetIndex = 0;
        this.time = 0.0f;
    }

    public void Update(GameTime gameTime)
    {
        time += (float)gameTime.ElapsedGameTime.TotalSeconds;   //Timer: de totaal verstreken tijd sinds het begin van de huidige frame
        while (time > frameTime)    //Hier moet gewisseld worden van frame
        {
            time -= frameTime;
            if (isLooping)
                sheetIndex = (sheetIndex + 1) % this.NumberSheetElements;
            else
                sheetIndex = Math.Min(sheetIndex + 1, this.NumberSheetElements - 1);
        }
    }

    public float FrameTime
    {
        get { return frameTime; }
    }

    public bool IsLooping
    {
        get { return isLooping; }
    }

    public int CountFrames
    {
        get { return this.NumberSheetElements; }
    }

    public bool AnimationEnded
    {
        get { return !this.isLooping && sheetIndex >= NumberSheetElements - 1; }
    }
}

