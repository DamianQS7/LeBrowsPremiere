using System.ComponentModel.DataAnnotations;

namespace LeBrowsPremiere.Models
{
    public class ClientEmailInformationModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MessageSubject { get; set; }

        public string MessageBody { get; set; }
    }
}
