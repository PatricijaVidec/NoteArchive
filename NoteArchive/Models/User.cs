namespace NoteArchive.Models;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

public class User : IdentityUser
{
    public string LastName { get; set; } = null!;
    public string FirstMidName { get; set; } = null!;

    public ICollection<Note>? Notes { get; set; }
    
}