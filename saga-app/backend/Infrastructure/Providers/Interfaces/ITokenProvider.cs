using System;
using saga.Models.Entities;

namespace saga.Infrastructure.Providers
{
    /// <summary>
    /// Provides functionality to generate tokens.
    /// </summary>
    public interface ITokenProvider
    {
        /// <summary>
        /// Generates a JWT (JSON Web Token) for the specified user.
        /// </summary>
        /// <param name="user">The user for which to generate the token.</param>
        /// <returns>The generated JWT.</returns>
        string GenerateJwtToken(UserEntity user);

        /// <summary>
        /// Generates a JWT (JSON Web Token) for resetting the user's password.
        /// </summary>
        /// <param name="user">The user for which to generate the reset password token.</param>
        /// <param name="durationTime">The duration time of the token.</param>
        /// <returns>The generated reset password JWT.</returns>
        string GenerateResetPasswordJwt(UserEntity user, TimeSpan durationTime);
    }
}
