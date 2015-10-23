using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class WaterDrop : SpriteGameObject
{
    protected float bounce;

    public WaterDrop(int layer=0, string id="") : base("Sprites/spr_water", layer, id) 
    {
    }

    public override void Update(GameTime gameTime)
    {
        //Laat de waterdrop op en neer stuiteren
        double t = gameTime.TotalGameTime.TotalSeconds * 3.0f + Position.X;
        bounce = (float)Math.Sin(t) * 0.2f;
        position.Y += bounce;
        //Kijkt of de speler de waterdrop raakt
        Player player = GameWorld.Find("player") as Player;
        if (this.visible && this.CollidesWith(player))
        {
            //De waterdrop is opgepakt door de speler
            this.visible = false;
            GameEnvironment.AssetManager.PlaySound("Sounds/snd_watercollected");
        }
    }
}
