﻿using System.ComponentModel.DataAnnotations;

namespace Library.ViewModels;

public class PublisherViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    [EmailAddress(ErrorMessage = "Wrong email format")]
    public string? Email { get; set; }

    [Phone(ErrorMessage = "Invalid phone number format")]
    public string? PhoneNumber { get; set; }
    public int YearPublished { get; set; }
    public int BookCount { get; set; }
}
