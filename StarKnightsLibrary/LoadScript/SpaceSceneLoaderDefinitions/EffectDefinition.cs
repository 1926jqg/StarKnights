using StarKnightsLibrary.Scenes;
using StarKnightsLibrary.Transmissions;
using StarKnightsLibrary.Triggers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StarKnightsLibrary.LoadScript.SpaceSceneLoaderDefinitions
{
    public class EffectDefinition
    {
        public string Type { get; set; }
        public Dictionary<string, string> Variables { get; set; }
        public Dictionary<string, string> DataVariables { get; set; }

        public Effect<ConditionResult> GetEffect()
        {
            return Type switch
            {
                "SendTransmission" => SendTransmission,
                _ => throw new Exception($"Effect Type {Type} is not defined for results with no data.")
            };
        }

        public Effect<ConditionResult<T>> GetEffect<T>()
        {
            return Type switch
            {
                "SendTransmission" => SendTransmission,
                _ => throw new Exception($"Effect Type {Type} is not defined for results of type {typeof(T)}.")
            };
        }

        private void SendTransmission(ISpaceScene enviroment, Func<string, string> messageFunc)
        {
            var duration = ulong.Parse(Variables["Duration"]);
            var message = Variables["Message"];

            enviroment.AddTransmission(new Transmission
            {
                Message = messageFunc(message)
            }, duration);
        }

        private void SendTransmission(ISpaceScene enviroment, ConditionResult conditionResult)
        {
            SendTransmission(enviroment, m => m);
        }

        private void SendTransmission<T>(ISpaceScene enviroment, ConditionResult<T> conditionResult)
        {
            var dataVariables = ExtractDataVariables(conditionResult);

            int count = 0;
            var values = new List<object>();

            while (dataVariables.TryGetValue(count++.ToString(), out object value))
            {
                values.Add(value);
            }

            SendTransmission(enviroment, m => string.Format(m, values.ToArray()));
        }

        private Dictionary<string, object> ExtractDataVariables<T>(ConditionResult<T> conditionResult)
        {
            return DataVariables?.ToDictionary(d => d.Key, d => ExtractDataVariable(d.Value, conditionResult.Data)) ?? new Dictionary<string, object>();
        }

        private object ExtractDataVariable(string variable, object obj)
        {
            var splits = variable.Split('.');

            foreach (var split in splits)
            {
                var type = obj.GetType();
                var prop = type.GetProperty(split);
                obj = prop.GetValue(obj);
            }
            return obj;
        }
    }
}