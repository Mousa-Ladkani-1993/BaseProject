using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseProjectApp.Library.Utility
{
    public class ValidatorUtil
    {
        public static bool Validate<T>(T obj, out ICollection<ValidationResult> results)
        {
            results = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, new ValidationContext(obj), results, true);
        }
    }
    public static class CheckUtils
    {
        public static bool HasData<T>(this IEnumerable<T> data)
        {
            return data == null ? false : data != null && data.Any();
        }
    }
}
