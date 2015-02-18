// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.ComponentModel.DataAnnotations;
using Asp.Infrastructure.Validation;
using Xunit;

namespace Portal.Tests
{
    public class CountryAttributeTests
    {
        [Fact]
        public void ValidateValidCountryValue()
        {
            // Arrange
            var attribute = new CountryAttribute();
            var client = new Client
            {
                Country = "RU"
            };

            // Act
            ValidationResult result = attribute.GetValidationResult(client.Country, new ValidationContext(client));

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void ValidateInvalidCountryValue()
        {
            // Arrange
            var attribute = new CountryAttribute();
            var client = new Client
            {
                Country = "RUB"
            };

            // Act
            ValidationResult result = attribute.GetValidationResult(client.Country, new ValidationContext(client));

            // Assert
            Assert.NotNull(result);
        }

        private class Client
        {
            public string Country { get; set; }
        }
    }
}