using UnityEngine;

public static class NumberHelper
{
    private static readonly string[] suffixes = {
        "", "k", "M", "B", "T", "Qa", "Qi", "Sx", "Sp", "Oc", "No", "Dc"
    };

    public static string GetFormattedPowerOfTwo(int i)
    {
        if (i < 0) return "Invalid";

        double value = Mathf.Pow(2, i);
        return FormatLargeNumber(value);
    }

    private static string FormatLargeNumber(double number)
    {
        int suffixIndex = 0;

        while (number >= 1000 && suffixIndex < suffixes.Length - 1)
        {
            number /= 1000;
            suffixIndex++;
        }

        return number % 1 == 0
            ? ((int)number).ToString() + suffixes[suffixIndex]
            : number.ToString("0.#") + suffixes[suffixIndex];
    }
}
