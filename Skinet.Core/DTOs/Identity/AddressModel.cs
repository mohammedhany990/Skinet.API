using System.ComponentModel.DataAnnotations;

namespace Skinet.API.DTOs.Identity
{
    public  class AddressModel
    {
      
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Street { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }
    }
}
