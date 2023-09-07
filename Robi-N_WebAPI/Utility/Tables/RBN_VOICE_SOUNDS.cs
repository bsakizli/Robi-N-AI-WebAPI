using System.ComponentModel.DataAnnotations;

namespace Robi_N_WebAPI.Utility.Tables
{
    public class RBN_VOICE_SOUNDS
    {
        [Key]
        public int Id { get; set; }
        public string? fileName { get; set; }
        public string? platform { get; set; }
        public string? text { get; set; }
        public Byte[]? soundContent { get; set; }
        public string? soundBase64Content { get; set; }
        public DateTime addDate { get; set; }
        public bool active { get; set; }
    }
}
