﻿using System;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Rest;

namespace AzureResources
{
    public class AuthentificationService : IAuthentificationService
    {
        private string _clientId;
        private string _tenantId;
        private string _email;
        private string _password;
        private string _subscriptionId;

        public AuthentificationService(string clientId, string tenantId, string email, string password, string subscriptionId)
        {
            _clientId = clientId;
            _tenantId = tenantId;
            _email = email;
            _password = password;
            _subscriptionId = subscriptionId;
        }
        
        public async Task<T> Auth<T>()
        {
            var context = new AuthenticationContext(string.Format("https://login.microsoftonline.com/{0}", _tenantId));

            var authentificationResult = await context.AcquireTokenAsync(
               // "we want to access the Windows Azure REST API"
               resource: "https://management.core.windows.net/",

               // The client ID from portal WAAD application UX
               clientId: _clientId,
               userCredential: new UserCredential(_email, _password));

            // Get the token within the response
            string token = authentificationResult.CreateAuthorizationHeader().Substring("Bearer ".Length);

            if (typeof(T) == typeof(TokenCloudCredentials))
            {
                return (T)Convert.ChangeType(new TokenCloudCredentials(_subscriptionId, token), typeof(T));
            }
            return (T)Convert.ChangeType(new TokenCredentials(token), typeof(T));
        }

        public string GetSubscriptionId()
        {
            return _subscriptionId;
        }
    }
}