using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration.Claims;
using System;

namespace SP.Portal.Core.Helpers
{
    public static class CommonHelpers
    {
        public static string TrimLogin(string login)
        {
            if (login == null) return null;

            var arr = login.Split('|');
            if (arr.Length >= 2) return arr[1];
            return arr[0];
        }

        public static string RemoveClaim(this string login)
        {
            string result = login;
            var spClaimProviderManager = SPClaimProviderManager.Local;
            if (spClaimProviderManager != null)
            {
                if (SPClaimProviderManager.IsEncodedClaim(login))
                {
                    // return the normal domain/username without any claims identification data
                    result = spClaimProviderManager.ConvertClaimToIdentifier(login);
                }
            }

            result = result.Substring(result.IndexOf('\\') + 1);

            return result;
        }

        public static string TryGetUserDisplayName(SPWeb web, string userLogin)
        {
            var user = web.AllUsers.GetByLoginNoThrow(userLogin);

            if (user == null)
            {
                try
                {
                    web.AllowUnsafeUpdates = true;
                    user = web.EnsureUser(userLogin);
                }
                catch (Exception ex)
                {
                    Logger.WriteLog(Logger.Category.High, "aBPM", $"Не удалось получить пользователя \"{userLogin}\".");
                }
            }

            if (user != null)
            {
                return user.Name;
            }
            else
            {
                return userLogin;
            }
        }
    }
}
