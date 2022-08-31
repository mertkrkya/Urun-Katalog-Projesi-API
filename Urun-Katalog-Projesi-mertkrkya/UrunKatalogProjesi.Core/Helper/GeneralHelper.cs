using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace UrunKatalogProjesi.Data.Helper
{
    public static class GeneralHelper
    {
        public static string ErrorToString(this IEnumerable<IdentityError> errors)
        {
            string error = "";
            bool isFirst = true;
            foreach (var item in errors)
            {
                if(isFirst)
                    error += item.Description;
                else
                    error += " "+item.Description;

            }
            return error;
        }
    }
}
