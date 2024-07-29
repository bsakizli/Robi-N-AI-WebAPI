using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmptorUtility.Models
{
    public class EmptorFileUploadModel
    {
        public string? FileName { get; set; }
        public string? MimeType { get; set; }
        public int DataSize { get; set; }
        public string? Summary { get; set; }
        public DateTime CreateUserTime { get; set; }
        public int CreateUserPositionId { get; set; }
        public int CreateUserId { get; set; }
        public DateTime UpdateUserTime { get; set; }
        public int UpdateUserPositionId { get; set; }
        public int UpdateUserId { get; set; }
        public bool Active { get; set; }
        public string? FilePath { get; set; }
        public byte[]? BlobData { get; set; }
        public int CBlobTypeId { get; set; }
    }
}
