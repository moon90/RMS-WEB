namespace RMS.Application.DTOs.UnitDTOs.InputDTOs
{
    public class CreateUnitDto
    {
        public string Name { get; set; }
        public string ShortCode { get; set; }
        public bool Status { get; set; } = true;
    }
}
