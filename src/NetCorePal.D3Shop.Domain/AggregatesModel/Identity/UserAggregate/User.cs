using NetCorePal.Extensions.Domain;

namespace NetCorePal.D3Shop.Domain.AggregatesModel.Identity.UserAggregate;

public partial record UserId : IGuidStronglyTypedId;

public class User : Entity<UserId>, IAggregateRoot
{
    public string Name { get; private set; } = string.Empty;
    public string Avatar { get; private set; } = string.Empty;
    public string Phone { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string PasswordSalt { get; private set; } = string.Empty;
    public int PasswordFailedTimes { get; private set; } = 0;
    public bool IsDisabled { get; private set; } = false;
    public DateTimeOffset? DisabledAt { get; private set; }
    public string DisabledReason { get; private set; } = string.Empty;
    public bool IsTwoFactorEnabled { get; } = false;
    public DateTimeOffset? LastLoginAt { get; private set; }
    public DateTimeOffset CreatedAt { get; init; }
    public UpdateTime UpdatedAt { get; private set; } = new UpdateTime(DateTimeOffset.UtcNow);
    public Deleted IsDeleted { get; private set; } = new Deleted(false);
    public DeletedTime? DeletedAt { get; private set; }

    protected User()
    {
    }

    public User(string name, string avatar, string phone, string email, string passwordHash, string passwordSalt)
    {
        Name = name;
        Avatar = avatar;
        Phone = phone;
        Email = email;
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
        Email = email;
        CreatedAt = DateTimeOffset.Now;
    }
}
