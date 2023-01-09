namespace TheGame
{
    public interface IStatsGetter
    {
        float TotalPower { get; }
        float SpeedValue { get; }
    }

    public class Stats : IStatsGetter
    {
        public Stat Power;
        public Stat Health;
        public Stat Mana;
        public Stat Speed;

        public float SpeedValue => Speed.Value;
        public float TotalPower => Power.Value; 
    }
}



