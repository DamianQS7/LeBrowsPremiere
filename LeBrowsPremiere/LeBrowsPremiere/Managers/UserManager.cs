using System.Security.Claims;

namespace LeBrowsPremiere.Managers
{
	public class UserManager
	{
		/// <summary>
		/// Retrieves the ID of the user from a ClaimsPrincipal object.
		/// </summary>
		/// <param name="user">The ClaimsPrincipal object representing the user.</param>
		/// <returns>The ID of the user, or null if the user is not authenticated.</returns>
		public static string GetUserId(ClaimsPrincipal user)
		{
			Claim? userClaim = user.FindFirst(ClaimTypes.NameIdentifier);
			return userClaim.Value;
		}
	}
}
