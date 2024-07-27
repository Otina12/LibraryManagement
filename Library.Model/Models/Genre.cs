﻿namespace Library.Model.Models;

public class Genre
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public ICollection<OriginalBookGenre> BookGenres { get; } = [];
}
