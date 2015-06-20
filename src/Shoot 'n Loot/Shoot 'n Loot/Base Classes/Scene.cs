using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shoot__n_Loot
{
    class Scene
    {
        public List<GameObject> objects;
        private List<GameObject> objectsToAdd, objectsToRemove;

        public Scene()
        {
            Initialize();
        }

        public virtual void Initialize()
        {
            objects = new List<GameObject>();
            objectsToAdd = new List<GameObject>();
            objectsToRemove = new List<GameObject>();
        }

        public virtual void OnResume() { }

        public virtual void Update()
        {
            foreach (GameObject g in objectsToAdd) objects.Add(g);
            objectsToAdd.Clear();

            foreach (GameObject g in objectsToRemove) objects.Remove(g);
            objectsToRemove.Clear();

            for (int i = objects.Count - 1; i >= 0; i--)
            {
                if (objects[i].Dead) objects.RemoveAt(i);
                else objects[i].Update();
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (GameObject o in objects) o.Draw(spriteBatch);
        }

        public void AddObject(GameObject o)
        {
            objectsToAdd.Add(o);
        }

        public void RemoveObject(GameObject o)
        {
            objectsToRemove.Add(o);
        }
    }
}
