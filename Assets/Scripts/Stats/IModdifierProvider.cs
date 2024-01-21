using System.Collections.Generic;

namespace RPG.Stats
{
    public interface IModdifierProvider
    {
        IEnumerable<float> GetAdditiveModifiers(Stat stat);
        IEnumerable<float> GetPercentageModifiers(Stat stat);
    }
}
