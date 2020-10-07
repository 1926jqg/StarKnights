using System;
using System.Collections.Generic;
using System.Linq;

namespace StarKnightsLibrary.SpaceObjects.Ships.Modules
{
    public class PowerManagementSystem
    {
        public int MaxPowerToModule { get; }
        public int TotalPower { get; }
        public int FreePower => TotalPower - Modules.Values.Sum(v => v.Power);

        private int SwitchRecharge { get; set; }

        private Dictionary<Type, IModule> Modules { get; }

        public PowerManagementSystem(int maxPowerToModule, int totalPower)
        {
            MaxPowerToModule = maxPowerToModule;
            TotalPower = totalPower;
            Modules = new Dictionary<Type, IModule>();
            SwitchRecharge = 0;
        }

        public bool TryAddModule<T>(T module)
            where T : IModule
        {
            Type type = typeof(T);
            bool returnVal = !Modules.ContainsKey(type);

            if (returnVal)
            {
                Modules.Add(typeof(T), module);
            }
            return returnVal;
        }

        public void ResetPower()
        {
            int levelPower = TotalPower / Modules.Count;
            foreach (IModule module in Modules.Values)
            {
                module.Power = levelPower;
            }
        }

        public bool TryAddPowerToModule<T>()
            where T : IModule
        {
            Type type = typeof(T);
            if (SwitchRecharge > 0 || !Modules.TryGetValue(type, out IModule module) || module.Power == MaxPowerToModule)
                return false;

            if (FreePower == 0)
            {
                IModule highestPowerModule = Modules
                    .Where(m => m.Key != type)
                    .OrderByDescending(m => m.Value.Power)
                    .Select(m => m.Value)
                    .FirstOrDefault();
                if (highestPowerModule == null || highestPowerModule.Power == 0)
                    return false;
                highestPowerModule.Power -= 1;
            }
            module.Power += 1;
            SwitchRecharge = 15;
            return true;
        }

        public void Update()
        {
            if (SwitchRecharge > 0)
            {
                SwitchRecharge--;
            }

            foreach (IModule module in Modules.Values)
            {
                module.Update();
            }
        }
    }
}
