// Copyright (c) Clickberry, Inc. All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE Version 3. See License.txt in the project root for license information.

using System.ComponentModel.DataAnnotations;
using Asp.Infrastructure.Validation;
using Xunit;

namespace Portal.Tests.Attributes
{
    public class EinAttrubuteTests
    {
        [Fact]
        public void ValidateValidEinWithValidCondition()
        {
            // Arrange
            var client = new Client
            {
                Country = "US",
                Ein = "00-0000000"
            };
            var attribute = new EinAttribute
            {
                OtherPropertyName = "Country",
                OtherPropertyValue = "US"
            };

            // Act
            ValidationResult result = attribute.GetValidationResult(client.Ein, new ValidationContext(client));

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void ValidateInvalidEinWithValidCondition()
        {
            // Arrange
            var client = new Client
            {
                Country = "US",
                Ein = "aa-0000000"
            };
            var attribute = new EinAttribute
            {
                OtherPropertyName = "Country",
                OtherPropertyValue = "US"
            };

            // Act
            ValidationResult result = attribute.GetValidationResult(client.Ein, new ValidationContext(client));

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void ValidateValidEinWithInvalidCondition()
        {
            // Arrange
            var client = new Client
            {
                Country = "RU",
                Ein = "00-0000000"
            };
            var attribute = new EinAttribute
            {
                OtherPropertyName = "Country",
                OtherPropertyValue = "US"
            };

            // Act
            ValidationResult result = attribute.GetValidationResult(client.Ein, new ValidationContext(client));

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void ValidateInvalidEinWithInvalidCondition()
        {
            // Arrange
            var client = new Client
            {
                Country = "RU",
                Ein = "aa-0000000"
            };
            var attribute = new EinAttribute
            {
                OtherPropertyName = "Country",
                OtherPropertyValue = "US"
            };

            // Act
            ValidationResult result = attribute.GetValidationResult(client.Ein, new ValidationContext(client));

            // Assert
            Assert.Null(result);
        }

        public class Client
        {
            public string Country { get; set; }

            public string Ein { get; set; }
        }
    }
}