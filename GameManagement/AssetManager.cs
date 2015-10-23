using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

public class AssetManager
{
    protected ContentManager contentManager;

    public AssetManager(ContentManager Content)
    {
        this.contentManager = Content;
    }

    //Laadt een sprite en geeft die door aan degene die de methode aanroept
    public Texture2D GetSprite(string assetName)
    {
        if (assetName == "")
            return null;
        return contentManager.Load<Texture2D>(assetName);
    }

    //Laadt een geluidseffect en geeft die door aan degene die de methode aanroept
    public void PlaySound(string assetName)
    {
        SoundEffect snd = contentManager.Load<SoundEffect>(assetName);
        snd.Play();
    }

    //Laadt een muzieknummer en geeft die door aan degene die de methode aanroept met de optie om het nummer te loopen
    public void PlayMusic(string assetName, bool repeat = true)
    {
        MediaPlayer.IsRepeating = repeat;
        MediaPlayer.Play(contentManager.Load<Song>(assetName));
    }

    public ContentManager Content
    {
        get { return contentManager; }
    }
}