using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;

class TimerGameObject : TextGameObject
{
    protected TimeSpan timeLeft, originaltimeLeft;
    protected bool running;
    protected double multiplier;

    //Dit is de timer, die bijhoudt hoeveel tijd je nog over hebt om een level te halen
    public TimerGameObject(int seconds, int layer = 0, string id = "")
        : base("Fonts/Hud", layer, id)
    {
        this.multiplier = 1;
        this.timeLeft = TimeSpan.FromSeconds(seconds);
        originaltimeLeft = timeLeft;
        this.running = true;
    }

    public override void Update(GameTime gameTime)
    {
        if (!running)
            return;
        double totalSeconds = gameTime.ElapsedGameTime.TotalSeconds * multiplier;
        timeLeft -= TimeSpan.FromSeconds(totalSeconds);
        if (timeLeft.Ticks < 0)
            return;
        DateTime timeleft = new DateTime(timeLeft.Ticks);
        this.Text = timeleft.ToString("mm:ss");
        this.color = Color.Yellow;
        //Als er nog maar weinig tijd over is, wordt de teller rood
        if (timeLeft.TotalSeconds <= 10 && (int)timeLeft.TotalSeconds % 2 == 0)
            this.color = Color.Red;
    }

    public override void Reset()
    {
        base.Reset();
        this.timeLeft = originaltimeLeft;
        this.running = true;
    }

    public bool Running
    {
        get { return running; }
        set { running = value; }
    }

    //Laat de tijd sneller of langzamer lopen
    public double Multiplier
    {
        get {return multiplier; }
        set { multiplier = value; }
    }

    public bool GameOver
    {
        get { return (timeLeft.Ticks <= 0); }
    }

    public TimeSpan TimeLeft
    {
        get { return timeLeft; }
        set { timeLeft = value; }
    }
}