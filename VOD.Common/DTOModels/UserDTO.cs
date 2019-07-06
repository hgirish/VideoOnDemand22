using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VOD.Common.DTOModels
{
   public class UserDTO
    {
        [Required]
        [Display(Name ="User Id")]
        public string Id { get; set; }
    }
}
