// Code generated by Microsoft (R) AutoRest Code Generator 0.17.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace MarginTrading.Services.Generated.ClientAccountServiceApi.Models
{
    using System.Linq;

    public partial class GetByEmailRequest
    {
        /// <summary>
        /// Initializes a new instance of the GetByEmailRequest class.
        /// </summary>
        public GetByEmailRequest() { }

        /// <summary>
        /// Initializes a new instance of the GetByEmailRequest class.
        /// </summary>
        public GetByEmailRequest(string email)
        {
            Email = email;
        }

        /// <summary>
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (Email == null)
            {
                throw new Microsoft.Rest.ValidationException(Microsoft.Rest.ValidationRules.CannotBeNull, "Email");
            }
        }
    }
}