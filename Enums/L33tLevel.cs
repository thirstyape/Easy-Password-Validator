namespace Easy_Password_Validator.Enums
{
    /// <summary>
    /// Specifies the level of l33t speak to use with letter substitutions
    /// </summary>
    public enum L33tLevel
    {
        /// <summary>
        /// Makes common substitutions for some letters such as E => 3, A => 4, etc.
        /// </summary>
        Basic,

        /// <summary>
        /// Adds alternate substitutions for some of the basic ones
        /// </summary>
        Intermediate,

        /// <summary>
        /// Makes substitutions for all letters and contains complex items such as M => /\/\, A => /\, etc.
        /// </summary>
        Advanced,

        /// <summary>
        /// Indicates the user will provide the substitution list
        /// </summary>
        Custom
    }
}
