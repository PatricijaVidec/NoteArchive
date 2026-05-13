namespace NoteArchive.Models;
using NoteArchive.Models;
using Microsoft.EntityFrameworkCore;

public class Note
{
    public int ID { get; set; }

    public string Title { get; set; } = null!;
    public string Author { get; set; } = null!;
    public string Genre { get; set; } = null!;
    public string Performer { get; set; } = null!;

    public DateTime PublicationDate { get; set; }

    public string UserID { get; set; } = null!;
    public User? User { get; set; }
    
    public ICollection<NoteImage>? Images { get; set; }
}
