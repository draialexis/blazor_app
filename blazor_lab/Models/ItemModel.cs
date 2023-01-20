using System.ComponentModel.DataAnnotations;

namespace blazor_lab.Models
{
    public class ItemModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "50ch max")]
        public string DisplayName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "50ch max")]
        [RegularExpression(@"^[a-z''-'\s]{1,50}$", ErrorMessage = "lowercase only")]
        public string Name { get; set; }

        [Required]
        [Range(1, 64)]
        public int StackSize { get; set; }

        [Required]
        [Range(1, 125)]
        public int MaxDurability { get; set; }

        public List<string> EnchantCategories { get; set; }
        public List<string> RepairWith { get; set; }

        [Required]
        [Range(typeof(bool), "true", "true", ErrorMessage = "You must agree to thhe terms.")]
        public bool AcceptConditions { get; set; }

        [Required(ErrorMessage = "img mandatory")]
        public byte[] ImageContent { get; set; }
    }
}
