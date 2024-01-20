using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace GuestAPI.enums
{
    /// <summary>
    /// Represents the title of a guest.
    /// </summary>
    public enum Title
    {
        /// <summary>
        /// Mr.
        /// </summary>
        [Display(Description = "MR")]
        Mr = 0,

        /// <summary>
        /// Mrs.
        /// </summary>
        [Display(Description = "MRS")]
        Mrs = 1,

        /// <summary>
        /// Miss.
        /// </summary>
        [Display(Description = "MISS")]
        Miss = 2,

        /// <summary>
        /// Dr.
        /// </summary>
        [Display(Description = "DR")]
        Dr = 3,

        /// <summary>
        /// Prof.
        /// </summary>
        [Display(Description = "PROF")]
        Prof = 4
    }
}
