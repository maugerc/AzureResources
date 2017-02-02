using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Threading.Tasks;

namespace AzureResources
{
    public interface IAuthentificationService
    {
        /// <summary>
        /// Authentificate a user with an Azure Active Directory App
        /// </summary>
        Task<T> Auth<T>();

        /// <summary>
        /// Return the subscription ID
        /// </summary>
        string GetSubscriptionId();
    }
}
