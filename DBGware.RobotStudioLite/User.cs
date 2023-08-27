using System;
using DBGware.RobotStudioLite.SQLite;

namespace DBGware.RobotStudioLite
{
    [Table("Users")]
    public class User
    {
        [PrimaryKey]
        public Guid ID { get; set; } = Guid.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public byte[] ProfilePicture { get; set; } = Array.Empty<byte>();
        public bool IsAdmin { get; set; } = false;
        public string Tag { get; set; } = string.Empty;
    }
}