using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models;

public class Prescription_Medicament
{
    
    public Medicament Medicament { get; set; }
    
    public Prescription Prescription { get; set; }
    
    [Key]
    public int IdMedicament { get; set; }
    [Key]
    public int IdPrescription { get; set; }
    
    public int? Dose { get; set; }
    
    public string Details { get; set; }
    
    
}