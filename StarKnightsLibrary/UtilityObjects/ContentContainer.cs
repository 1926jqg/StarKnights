using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace StarKnightsLibrary.UtilityObjects
{
    public interface IContentContainer
    {
        void Add<T>(string contentName, T content) where T : class;
        T Load<T>(string contentName) where T : class;
        Texture2D LoadTexture(string textureName);
        SpriteFont LoadFont(string fontName);
    }

    public class ContentContainer : IContentContainer
    {
        private readonly ContentManager _contentManager;

        private readonly Dictionary<Type, Dictionary<string, object>> _content;

        public ContentContainer(ContentManager contentManager)
        {
            _contentManager = contentManager;
            _content = new Dictionary<Type, Dictionary<string, object>>();
        }

        public void Add<T>(string contentName, T content)
            where T : class
        {
            var typeDictionary = GetTypeDictionary<T>();
            typeDictionary.Add(contentName, content);
        }

        private Dictionary<string, object> GetTypeDictionary<T>()
            where T : class
        {
            var type = typeof(T);
            if (!_content.TryGetValue(type, out Dictionary<string, object> typeDictionary))
            {
                typeDictionary = new Dictionary<string, object>();
                _content.Add(type, typeDictionary);
            }
            return typeDictionary;
        }

        public T Load<T>(string contentName)
            where T : class
        {
            var typeDictionary = GetTypeDictionary<T>();
            if (!typeDictionary.TryGetValue(contentName, out object content))
            {
                content = _contentManager.Load<T>(contentName);
                typeDictionary.Add(contentName, content);
            }

            return content as T;
        }

        public Texture2D LoadTexture(string textureName)
        {
            return Load<Texture2D>(textureName);
        }

        public SpriteFont LoadFont(string fontName)
        {
            return Load<SpriteFont>(fontName);
        }
    }
}
