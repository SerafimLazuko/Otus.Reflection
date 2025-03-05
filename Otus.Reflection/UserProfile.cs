namespace Otus.Reflection;

public class UserProfile
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public List<string> Interests { get; set; }
    public string Gender { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsActive { get; set; }

    public static UserProfile GetUserProfile()
    {
        return new UserProfile
        {
            FirstName = "Иван",
            LastName = "Иванов",
            DateOfBirth = new DateTime(1990, 1, 1),
            Email = "ivan.ivanov@example.com",
            PhoneNumber = "+1234567890",
            Interests = new List<string> { "Программирование", "Музыка", "Спорт" },
            Gender = "Мужской",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            IsActive = true
        };
    }
}
