using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELittoral.Models
{
    /// <summary>
    /// Represents an instructional item that can be displayed in the welcome screen
    /// experience for new users.
    /// </summary>
    public class InstructionItem
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="headerText">The header text.</param>
        /// <param name="contentText">The content text.</param>
        /// <param name="image">The image Uri.</param>
        public InstructionItem(string headerText, string contentText,
            Uri image)
        {
            HeaderText = headerText;
            ContentText = contentText;
            Image = image;
        }

        /// <summary>
        /// Gets or sets the content text.
        /// </summary>
        public string ContentText { get; set; }

        /// <summary>
        /// Gets or sets the header text.
        /// </summary>
        public string HeaderText { get; set; }

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        public Uri Image { get; set; }
    }
}
