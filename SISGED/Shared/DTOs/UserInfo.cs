using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SISGED.Shared.DTOs
{
    public class UserInfo
    {
        [StringLength(20, ErrorMessage = "Name is too long.")]
        [Required(ErrorMessage = "Debe ingresar el Nombre de Usuario obligatoriamente")]
        public string usuario { get; set; }
        [Required(ErrorMessage = "Debe ingresar la Contraseña obligatoriamente")]
        public string clave
        {
            get; set;
        }
    }
}
