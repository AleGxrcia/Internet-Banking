namespace InternetBanking.Core.Application.Helpers
{
    public static class UniqueIdGenerator
    {
        private static readonly Random random = new();
        private const string characters = "0123456789";

        public static string GenerateUniqueId()
        {
            char[] id = new char[9];
            for (int i = 0; i < 9; i++)
            {
                id[i] = characters[random.Next(characters.Length)];
            }
            return new string(id);
        }
    }
}
