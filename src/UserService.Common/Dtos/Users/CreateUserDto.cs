namespace UserService.Common.Dtos.Users;

public record CreateUserDto(string FirstName,
                            string LastName,
                            string UserName,
                            string Password,
                            string Address,
                            string? Email,
                            string? PhoneNumber,
                            DateTime? DateOfBirth);
