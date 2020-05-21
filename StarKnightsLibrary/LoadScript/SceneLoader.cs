using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using StarKnightsLibrary.Scenes;
using System;
using System.IO;

namespace StarKnightsLibrary.LoadScript
{
    public static class SceneLoader
    {
        private static readonly Random Random = new Random();

        public static IScene Load(Viewport viewport, string file)
        {
            using var streamReader = new StreamReader(file);
            using var jsonReader = new JsonTextReader(streamReader);
            var serializer = new JsonSerializer();

            var definition = serializer.Deserialize<SceneDefinitionContainer>(jsonReader);

            return definition.Get().Build(viewport);
        }

        public static int GetIntFromString(string value)
        {
            if (value?.StartsWith("rand") ?? false)
            {
                value = value.Substring(5, value.Length - 6);
                var split = value.Split(',');
                var max = int.Parse(split[0]);
                var min = int.Parse(split[1]);
                return Random.Next(max - min) + min;
            }
            else
            {
                return !string.IsNullOrEmpty(value) ? int.Parse(value) : 0;
            }
        }

        public static float GetFloatFromString(string value)
        {
            return !string.IsNullOrEmpty(value) ? float.Parse(value) : 0;
        }

    }
}
