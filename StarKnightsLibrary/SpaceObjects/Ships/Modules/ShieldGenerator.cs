namespace StarKnightsLibrary.SpaceObjects.Ships.Modules
{
    public class ShieldGenerator : IModule
    {
        public int Power { get; set; }

        public int MaxShield { get; private set; }

        private int _shield_value;
        private int ShieldValue
        {
            get => _shield_value;
            set => _shield_value = value > MaxShield ? MaxShield : value;
        }

        public int CurrentShields => ShieldValue > 0 ? ShieldValue : 0;

        public float ShieldPercentage => CurrentShields / (float)MaxShield;

        public bool ShieldsOnline => ShieldValue > 0;

        public int RechargeRate { get; private set; }

        public ShieldGenerator(int shields, int rechargeRate)
        {
            MaxShield = shields;
            ShieldValue = shields;
            RechargeRate = rechargeRate;
        }

        public void Update()
        {
            bool wasOffline = !ShieldsOnline;
            ShieldValue += RechargeRate * Power;
            if (wasOffline && ShieldsOnline)
                ShieldValue = MaxShield / 2;
        }

        public void TakeDamage(int amount, DamageType type)
        {
            int damage;
            switch (type)
            {
                case DamageType.Thermal:
                    damage = amount;
                    break;
                case DamageType.Concussive:
                    damage = amount / 10;
                    break;
                default:
                    throw new DamageTypeUnexpectedException(type);
            }
            bool wasOnline = ShieldsOnline;
            ShieldValue -= damage;
            if (wasOnline && !ShieldsOnline)
                ShieldValue = -MaxShield;
        }
    }
}
