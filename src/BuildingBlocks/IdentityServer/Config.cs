using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace IdentityServer
{
    public class Config
    {
        public static IEnumerable<Client> Clients(IConfiguration configuration) =>
            new Client[]
            {
                   new Client
                   {
                       ClientId = "shop_mvc_client",
                       ClientName = "Shop MVC Web App",
                       AllowedGrantTypes = GrantTypes.ClientCredentials,
                       RequirePkce = false,
                       AllowRememberConsent = false,
                       RedirectUris = new List<string>()
                       {
                           $"{configuration["WebClientBaseAddress"]}/signin-oidc"
                       },
                       PostLogoutRedirectUris = new List<string>()
                       {
                           $"{configuration["WebClientBaseAddress"]}/signout-callback-oidc"
                       },
                       ClientSecrets = new List<Secret>
                       {
                           new Secret("secret".Sha256())
                       },
                       AllowedScopes = new List<string>
                       {
                           IdentityServerConstants.StandardScopes.OpenId,
                           IdentityServerConstants.StandardScopes.Profile,
                           IdentityServerConstants.StandardScopes.Address,
                           IdentityServerConstants.StandardScopes.Email,
                           "basketAPI",
                           "catalogAPI",
                           "discountAPI",
                           "orderingAPI",
                           "roles"
                       }
                   }
            };

        public static List<TestUser> TestUsers =>
        new List<TestUser>
        {
        };

        public static IEnumerable<ApiScope> ApiScopes =>
           new ApiScope[]
           {
               new ApiScope("basketAPI", "Basket API"),
               new ApiScope("catalogAPI", "Catalog API"),
               new ApiScope("discountAPI", "Discount API"),
               new ApiScope("orderingAPI", "Ordering API")
           };

        public static IEnumerable<IdentityResource> IdentityResources =>
          new IdentityResource[]
          {
              new IdentityResources.OpenId(),
              new IdentityResources.Profile(),
              new IdentityResources.Address(),
              new IdentityResources.Email(),
              new IdentityResource(
                    "roles",
                    "Your role(s)",
                    new List<string>() { "role" })
          };
    }
}
