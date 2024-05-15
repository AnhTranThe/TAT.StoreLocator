using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAT.StoreLocator.Core.Models.Request.Store
{
    public class GetDetailStoreRequestModel
    {
        [Required]
        public string? Id { get; set; }
    }
}
