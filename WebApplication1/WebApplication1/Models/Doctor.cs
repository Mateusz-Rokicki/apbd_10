﻿namespace WebApplication1.Models;

public class Doctor
{
    public int idDoctor { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    
    public virtual ICollection<Prescription> Prescriptions { get; set; }
}