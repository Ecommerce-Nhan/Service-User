namespace UserService.Common.Dtos.Users;

public record UserDto(string Id,
                      string FirstName,
                      string LastName,
                      string UserName,
                      string Address,
                      string? Email,
                      string? PhoneNumber,
                      DateTime? DateOfBirth);
