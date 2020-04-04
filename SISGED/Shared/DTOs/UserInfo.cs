using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SISGED.Shared.DTOs
{
    public class UserInfo
    {
        [Required]
        public string usuario { get; set; }
        [Required]
        public string clave
        {
            get; set;
        }
    }
}
