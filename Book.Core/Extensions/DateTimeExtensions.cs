using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Books.Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static int CalculateAge(this DateTime dob)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            var age = today.Year - dob.Year;

            return age;
        }
    }
}
