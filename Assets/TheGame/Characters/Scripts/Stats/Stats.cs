namespace TheGame
{
    public interface IStatsGetter
    {
        IStatGetter Power { get; }
        IStatGetter Speed { get; }
    }

    public class Stats : IStatsGetter
    {
        public Stat PowerStat;
        public Stat HealthStat;
        public Stat ManaStat;
        public Stat SpeedStat;

        public IStatGetter Power => PowerStat;
        public IStatGetter Speed => SpeedStat;
    }
}



