﻿using Microsoft.Xna.Framework;

class VisibilityTimer : GameObject
{
    protected GameObject target;
    protected float timeleft;
    protected float totaltime;

    //Dit is de timer die je kan gebruiken om tekst tijdelijk om het scherm te tonen
    public VisibilityTimer(GameObject target, int layer=0, string id = "")
        : base(layer, id)
    {
        totaltime = 5;
        timeleft = 0;
        this.target = target;
    }

    public override void Update(GameTime gameTime)
    {
        timeleft -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (timeleft <= 0)
            target.Visible = false;
    }

    //Start de timer
    public void StartVisible()
    {
        timeleft = totaltime;
        target.Visible = true;
    }
}
