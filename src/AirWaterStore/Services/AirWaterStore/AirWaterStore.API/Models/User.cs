namespace AirWaterStore.API.Models;

public class User : IdentityUser<int>
{
    //public int UserId { get; set; }

    //public string Email { get; set; } = null!;

    //public string UserName { get; set; } = null!;

    //public string Password { get; set; } = null!;

    //public int Role { get; set; }

    public bool? IsBan { get; set; }

    //public virtual ICollection<ChatRoom> ChatRoomCustomers { get; set; } = new List<ChatRoom>();

    //public virtual ICollection<ChatRoom> ChatRoomStaffs { get; set; } = new List<ChatRoom>();

    //public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

}

public class Role : IdentityRole<int>
{
    public Role() : base() { }
    public Role(string roleName) : this()
    {
        Name = roleName;
    }

}
