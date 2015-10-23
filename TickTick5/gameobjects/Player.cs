using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;

partial class Player : AnimatedGameObject
{
    protected Vector2 startPosition;
    protected bool isOnTheGround;
    protected float previousYPosition;
    protected bool isAlive;
    protected bool exploded;
    protected bool finished;
    protected bool walkingOnIce, walkingOnHot;

    public Player(Vector2 start) : base(2, "player")
    {
        this.LoadAnimation("Sprites/Player/spr_idle", "idle", true); 
        this.LoadAnimation("Sprites/Player/spr_run@13", "run", true, 0.05f);
        this.LoadAnimation("Sprites/Player/spr_jump@14", "jump", false, 0.05f); 
        this.LoadAnimation("Sprites/Player/spr_celebrate@14", "celebrate", false, 0.05f);
        this.LoadAnimation("Sprites/Player/spr_die@5", "die", false);
        this.LoadAnimation("Sprites/Player/spr_explode@5x5", "explode", false, 0.04f); 

        startPosition = start;
        Reset();
    }

    public override void Reset()
    {
        this.position = startPosition;
        this.velocity = Vector2.Zero;
        isOnTheGround = true;
        isAlive = true;
        exploded = false;
        finished = false;
        walkingOnIce = false;
        walkingOnHot = false;
        this.PlayAnimation("idle");
        previousYPosition = BoundingBox.Bottom;
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        float walkingSpeed = 400;
        //Als hij over een ijsblokje heen loopt, gaat hij steeds sneller
        if (walkingOnIce)
            walkingSpeed *= 1.5f;
        if (!isAlive)
            return;
        //Hij beweegt naar links
        if (inputHelper.IsKeyDown(Keys.A))
            velocity.X = -walkingSpeed;
        //Hij beweegt naar rechts
        else if (inputHelper.IsKeyDown(Keys.D))
            velocity.X = walkingSpeed;
        //Als hij niet op ijs staat, kan hij stilstaan
        else if (!walkingOnIce && isOnTheGround)
            velocity.X = 0.0f;
        if (velocity.X != 0.0f)
            Mirror = velocity.X < 0;
        //Hij springt als hij op de grond staat en er op spatie wordt gedrukt
        if ((inputHelper.KeyPressed(Keys.Space) || inputHelper.KeyPressed(Keys.W)) && isOnTheGround)
            Jump();
        if (inputHelper.KeyPressed(Keys.E))
        {
            /*schieten*/
        }
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        if (!finished && isAlive)
        {
            //Laadt verschillende animaties afhankelijk van de beweging dat hij aan het maken is (of juist geen beweging)
            if (isOnTheGround)
                if (velocity.X == 0)
                    this.PlayAnimation("idle");
                else
                    this.PlayAnimation("run");
            else if (velocity.Y < 0)
                this.PlayAnimation("jump");

            TimerGameObject timer = GameWorld.Find("timer") as TimerGameObject;
            //De tijd gaat sneller als je op hete blokjes loopt
            if (walkingOnHot)
                timer.Multiplier = 2;
            //De tijd gaat langzamer als je op ijs blokjes loopt
            else if (walkingOnIce)
                timer.Multiplier = 0.5;
            else
                timer.Multiplier = 1;

            TileField tiles = GameWorld.Find("tiles") as TileField;
            //Als hij uit veld valt, gaat hij dood
            if (BoundingBox.Top >= tiles.Rows * tiles.CellHeight)
                this.Die(true);
        }
        DoPhysics();
    }

    //Laat de speler exploderen
    public void Explode()
    {
        //Hij kan niet exploderen als het level gehaald is
        if (!isAlive || finished)
            return;
        isAlive = false;
        exploded = true;
        velocity = Vector2.Zero;
        position.Y += 15;
        this.PlayAnimation("explode");
    }

    //Laat de speler doodgaan
    public void Die(bool falling)
    {
        //Kan niet doodgaan als het level al gehaald is of als de speler al dood is
        if (!isAlive || finished)
            return;
        isAlive = false;
        velocity.X = 0.0f;
        //Speelt verschillende geluidjes af afhankelijk van de manie van doodgaan
        if (falling)
            GameEnvironment.AssetManager.PlaySound("Sounds/snd_player_fall");
        else
        {
            velocity.Y = -900;
            GameEnvironment.AssetManager.PlaySound("Sounds/snd_player_die");
        }
        this.PlayAnimation("die");
    }

    public bool IsAlive
    {
        get { return isAlive; }
    }

    public bool Finished
    {
        get { return finished; }
    }

    public void LevelFinished()
    {
        finished = true;
        velocity.X = 0.0f;
        this.PlayAnimation("celebrate");
        GameEnvironment.AssetManager.PlaySound("Sounds/snd_player_won");
    }
}
