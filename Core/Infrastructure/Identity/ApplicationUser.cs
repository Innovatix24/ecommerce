﻿
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; }
    public short UserId { get; set; }
}
