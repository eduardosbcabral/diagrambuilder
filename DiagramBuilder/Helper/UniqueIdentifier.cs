using HashidsNet;
using System;
using System.Linq;

namespace DiagramBuilder.Helper
{
    internal static class UniqueIdentifier
    {
        private static readonly string _digits = "0123456789";
        private static readonly string _lowerCaseCharacters = "abcdefghijklmnopqrstuvwxyz";
        private static readonly string _upperCaseCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private static Random _random = new Random();

        /// <summary>
        /// Criar código externo
        /// </summary>
        /// <param name="prefix">Prefixo</param>
        /// <returns>Código externo</returns>
        public static string Create(string prefix = null, int length = 16,
                                    bool useLowerCase = true, bool useUpperCase = true, bool useDigits = true)
        {
            var alphabet = string.Empty;

            if (useLowerCase) alphabet = string.Concat(alphabet, _lowerCaseCharacters);
            if (useUpperCase) alphabet = string.Concat(alphabet, _upperCaseCharacters);
            if (useDigits) alphabet = string.Concat(alphabet, _digits);

            var salt = Guid.NewGuid().ToString();

            var hashids = new Hashids(salt, length, alphabet: alphabet);

            //Escolhe 3 números aleatoriamente para gerar o hash
            var numbers = Enumerable.Range(0, 3).Select(r => _random.Next(100)).ToList();

            var hash = hashids.Encode(numbers);

            if (string.IsNullOrWhiteSpace(prefix) == false) hash = string.Concat(prefix, hash);

            return hash;
        }
    }
}
