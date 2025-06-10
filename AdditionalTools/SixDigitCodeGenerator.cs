using System.Globalization;
using System.Security.Cryptography;


namespace AdditionalTools
{
    public static class SixDigitCodeGenerator
    {
        public static string GenerateSixDigitCode()
        {
            using var rng = RandomNumberGenerator.Create();
            byte[] bytes = new byte[4];
            int number = 0;

            rng.GetBytes(bytes);
            number = BitConverter.ToInt32(bytes, 0) & 0x7FFFFFFF;
            number %= 1_000_000;

            return number.ToString("D6");
        }
    }
}
