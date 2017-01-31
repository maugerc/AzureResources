using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AzureResources;

namespace Utils.Tests
{
    [TestClass]
    public class AzureConfigurationTest_GetAzureConfig
    {
        [TestMethod]
        public void TestName()
        {
            // Arrange
            // Mock/Stub ReadFile method
            // Act
            var result = AzureConfiguration.GetAzureConfig();
            // Assert
            Assert.IsNotNull(result);
        }
    }
}
