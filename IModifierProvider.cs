using System.Collections.Generic;

public interface IModifierProvider
{
    IEnumerable<float> GetAdditiveModifiers(Stat stat); // To IEnumerable einai to idio me to IEnumerator alla mporeis na kaneis FOREACH loop 
    IEnumerable<float> GetPercentageModifiers(Stat stat);
}