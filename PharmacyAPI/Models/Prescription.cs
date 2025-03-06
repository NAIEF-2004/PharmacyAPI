namespace PharmacyAPI.Models
{
    public class Prescription
    {
        public int Id { get; set; }
        public string PatientName { get; set; } = "";
        public DateTime IssuedDate { get; set; }
    }
}
