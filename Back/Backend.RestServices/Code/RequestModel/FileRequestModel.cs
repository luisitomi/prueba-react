using System.ComponentModel.DataAnnotations;

namespace Backend.RestServices.Code.RequestModel
{
    public class FileRequestModel
    {

        public string Bytes { get; set; }
  
        [Required]
        public string Auditoria {get;set;}
        [Required]
        public string Campo2 { get; set; }

    }
}
