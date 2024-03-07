namespace Easy_Password_Validator.Models
{
    /// <summary>
    /// Class used to create a map of a keyboard layout
    /// </summary>
    public class PatternMapItem
    {
        /// <summary>
        /// The value of the key when pressed on the keyboard
        /// </summary>
        public char RegularKey { get; set; }

        /// <summary>
        /// The value of the key when pressed on the keyboard while a Shift key is also being pressed
        /// </summary>
        public char ShiftKey { get; set; }

        /// <summary>
        /// The characters to be considered neighboring key presses (ex. e => 3, 4, w, r, s, d)
        /// </summary>
        public char[] NeighborKeys { get; set; }
    }
}
