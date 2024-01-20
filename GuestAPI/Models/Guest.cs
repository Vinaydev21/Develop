using GuestAPI.enums;
using System.ComponentModel.DataAnnotations;

namespace GuestAPI.Models
{
    /// <summary>
    /// Represents a guest entity.
    /// </summary>
    public class Guest
    {
        /// <summary>
        /// Gets or sets the unique identifier for the guest.
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the title of the guest (e.g., Mr, Mrs, Miss).
        /// </summary>
        public Title Title { get; set; }

        /// <summary>
        /// Gets or sets the first name of the guest.
        /// </summary>
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the guest.
        /// </summary>
        [Required]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the birth date of the guest.
        /// </summary>
        [Required]
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Gets or sets the email address of the guest.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the phone numbers associated with the guest.
        /// </summary>
        [Required]
        public IEnumerable<string> PhoneNumbers { get; set; }
    }
}
