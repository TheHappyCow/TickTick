using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class GameObjectList : GameObject
{
    protected List<GameObject> gameObjects;

    public GameObjectList(int layer = 0, string id = "") : base(layer, id)
    {
        gameObjects = new List<GameObject>();
    }

    //Voegt een gameobject toe aan de lijst
    public void Add(GameObject obj)
    {
        obj.Parent = this;
        //Ordent de gameobjects in de lijst op basis van hun layers: lage layers eerst en daarna loopt layernummer op
        for (int i = 0; i < gameObjects.Count; i++)
        {
            if (gameObjects[i].Layer > obj.Layer)
            {
                gameObjects.Insert(i, obj);
                return;
            }
        }
        //Als het het eerste object is dat wordt toegevoegd aan de lijst, 
        //dan zijn er geen andere layers om te controleren, maar moet het object wel toegevoegd worden
        gameObjects.Add(obj);
    }

    //Verwijdert een gameobject uit de lijst
    public void Remove(GameObject obj)
    {
        gameObjects.Remove(obj);
        obj.Parent = null;
    }

    //Zoekt een gameobject in de lijst dmv een gegeven ID
    public GameObject Find(string id)
    {
        foreach (GameObject obj in gameObjects)
        {
            if (obj.ID == id)
                return obj;
            if (obj is GameObjectList)  //Als een object in de lijst zelf ook een lijst is, moet die ook afgegaan worden om te kijken of daar het object in zit met de gegeven ID
            {
                GameObjectList objlist = obj as GameObjectList;
                GameObject subobj = objlist.Find(id);
                if (subobj != null)
                    return subobj;
            }
        }
        return null;
    }

    public List<GameObject> Objects
    {
        get { return gameObjects; }
    }

    public override void HandleInput(InputHelper inputHelper)
    {
        for (int i = gameObjects.Count - 1; i >= 0; i--)
            gameObjects[i].HandleInput(inputHelper);
    }

    public override void Update(GameTime gameTime)
    {
        foreach (GameObject obj in gameObjects)
            obj.Update(gameTime);
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (!visible)
            return;
        List<GameObject>.Enumerator e = gameObjects.GetEnumerator();        
        while (e.MoveNext())
            e.Current.Draw(gameTime, spriteBatch);
    }

    public override void Reset()
    {
        base.Reset();
        foreach (GameObject obj in gameObjects)
            obj.Reset();
    }
}
