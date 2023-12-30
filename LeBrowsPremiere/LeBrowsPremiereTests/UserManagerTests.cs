using LeBrowsPremiere.Data;
using LeBrowsPremiere.Entities;
using LeBrowsPremiere.Managers;
using LeBrowsPremiere.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LeBrowsPremiereTests
{
    public class UserManagerTests
    {
        [Fact]
        public void GetUserId_ReturnsUserId_WhenUserHasNameIdentifierClaim()
        {
            // Arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
            new Claim(ClaimTypes.NameIdentifier, "123"),
            new Claim(ClaimTypes.Name, "testuser")
            }));

            // Act
            var userId = UserManager.GetUserId(user);

            // Assert
            Assert.Equal("123", userId);
        }
    }
}
