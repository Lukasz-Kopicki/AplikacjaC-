using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CompanyServices
    {
        public static string KeyGenerator(int range)
        {
            var chars = "abcdefghijklmnopqrstuwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, range)
                            .Select(s => s[random.Next(s.Length)])
                            .ToArray());

            return result;
        }
    }
}
