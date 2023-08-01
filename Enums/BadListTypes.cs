namespace Easy_Password_Validator.Enums
{
	/// <summary>
	/// A listing of types of lists that contain bad passwords
	/// </summary>
	public enum BadListTypes
	{
		/// <summary>
		/// The top 10,000 bad passwords list
		/// </summary>
		Top10K,

		/// <summary>
		/// The top 100,000 bad passwords list
		/// </summary>
		Top100K,

		/// <summary>
		/// A list of user details not to include in passwords
		/// </summary>
		UserInformation,

		/// <summary>
		/// A custom bad passwords list supplied by the user
		/// </summary>
		UserSupplied
	}
}
