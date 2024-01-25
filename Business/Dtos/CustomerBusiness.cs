
using Infrastructure.Entities;
using Shared.Interfaces;

namespace Infrastructure.Dtos
{
    public class CustomerBusiness
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }

        public class CustomerConverter : IDtoEntityConverter<CustomerEntity, CustomerBusiness>
        {
            public CustomerBusiness ConvertToDto(CustomerEntity entity)
            {
                return new CustomerBusiness
                {
                    FirstName = entity.FirstName,
                    LastName = entity.LastName,
                    Email = entity.Email,
                    PhoneNumber = entity.PhoneNumber
                };
            }

            public CustomerEntity ConvertToEntity(CustomerBusiness dto)
            {
                return new CustomerEntity
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    PhoneNumber = dto.PhoneNumber
                };
            }


        }

    }
}
