using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UsedGamesSale.Models.DTOs.GameDTOs
{
    public class EditGameDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Filed {0} is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Field {0} needs to be at least {2} and at most {1}")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Field {0} is required")]
        [Range(0.1, 10000, ErrorMessage = "Field {0} needs to be at least {1} and at most {2}")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Filed {0} is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Filed {0} needs to be at least {2} and at most {1}")]
        public string Details { get; set; }

        [Range(1, 100000, ErrorMessage = "Field {0} needs to be at least {1} and at most {2}")]
        public int StockQuantity { get; set; }
    }
}
