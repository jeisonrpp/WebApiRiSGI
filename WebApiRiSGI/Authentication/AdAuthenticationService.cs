using Microsoft.AspNetCore.Authentication;
using System;
using System.DirectoryServices.AccountManagement;

namespace WebApiRiSGI.Authentication
{
    public class AdAuthenticationService
    {
        private readonly string _domain;
        private readonly string _ldapPath;

        public AdAuthenticationService(string domain, string ldapPath)
        {
            _domain = domain;
            _ldapPath = ldapPath;
        }

        public bool ValidateCredentials(string username, string password)
        {
            using (PrincipalContext principalContext = new PrincipalContext(ContextType.Domain, _domain, _ldapPath))
            {
                // Validate the credentials
                return principalContext.ValidateCredentials(username, password);
            }
        }

        public UserInfoModel GetUserInfo(string username)
        {
            using (PrincipalContext principalContext = new PrincipalContext(ContextType.Domain, "suprema-ji.gov.do", "DC=Suprema-ji,DC=gov,DC=do"))
            {
                try
                {
                    // Find the user in Active Directory
                    UserPrincipal userPrincipal = UserPrincipal.FindByIdentity(principalContext, username);

                    if (userPrincipal != null)
                    {
                        // Extract desired user information
                        UserInfoModel userInfo = new UserInfoModel
                        {
                            DomainUser = userPrincipal.SamAccountName,
                            DisplayName = userPrincipal.DisplayName,
                            Email = userPrincipal.EmailAddress,
                            Desc = userPrincipal.Description,
                            // Add other properties you want to retrieve
                        };

                        return userInfo;
                    }

                    return null; // User not found
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return null;
                }

            }
        }
    

    public class UserInfoModel
    {
        public string DomainUser { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Desc { get; set; }
        // Add other properties you want to retrieve
    }
}
}
