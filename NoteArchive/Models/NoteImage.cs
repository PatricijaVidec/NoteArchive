namespace NoteArchive.Models;
using NoteArchive.Models;
using Microsoft.EntityFrameworkCore;

public class NoteImage
{
    public int ID { get; set; }

    public string ImagePath { get; set; } = null!;

    public int NoteID { get; set; }
    public Note? Note { get; set; }
}