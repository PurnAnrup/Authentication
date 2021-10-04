using JWToken.Controllers;
using JWToken.Model;
using JWToken.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using Xunit;

namespace Auth.Tests
{
    public class UnitTest1
    {
        Mock<IUserService> mock = new Mock<IUserService>();

       [Fact]
       public  void Return_InValidUserId()
        {
            var result = new Login()
            {
                Email = "anrup1000@gmail.com",
                Password = "string"
            };
            mock.Setup(p => p.UserLogin(result)).Returns("No User Found, Please Sign Up");
            var controller = new UserController(null,mock.Object);
            BadRequestObjectResult result1 = (BadRequestObjectResult)controller.Login(result);
            var message = result1.Value;
            Assert.Equal("No User Found, Please Sign Up",message.ToString());
        }

        [Fact]
        public void Return_InValid_Password()
        {
            var result = new Login()
            {
                Email = "anrup100@gmail.com",
                Password = "string12"
            };
            mock.Setup(p => p.UserLogin(result)).Returns("Invalid Password");
            var controller = new UserController(null,mock.Object);
            BadRequestObjectResult result1 = (BadRequestObjectResult)controller.Login(result);
            var message = result1.Value;
            Assert.Equal("Invalid Password", message.ToString());
        }

        [Fact]
        public void Return_SignUp_Unsuccessful()
        {
            var result = new User()
            {
                FirstName="anrup",
                LastName="purna",
                Email = "anrup100@gmail.com",
                PhoneNumber="1234567899",
                Address="Nad",
                Zipcode=530007,
                Password = "string",
                Role="Guest",
                Token="string"
            };
            mock.Setup(p => p.UserSignUp(result)).Returns("User Already Exits!");
            var controller = new UserController(null,mock.Object);
            BadRequestObjectResult result1 = (BadRequestObjectResult)controller.SignUp(result);
            var message = result1.Value;
            Assert.Equal("User Already Exits!", message.ToString());
        }

        [Fact]
        public void Return_SignUp_Successful()
        {
            var result = new User()
            {
                FirstName = "anrup1",
                LastName = "purna",
                Email = "anrup1001@gmail.com",
                PhoneNumber = "1234567899",
                Address = "Nad",
                Zipcode = 530007,
                Password = "string1",
                Role = "Guest",
                Token = "string"
            };
            mock.Setup(p => p.UserSignUp(result)).Returns("Successfully Registered");
            var controller = new UserController(null,mock.Object);
            OkObjectResult result1 = (OkObjectResult)controller.SignUp(result);
            var message = result1.Value;
            Assert.Equal("Successfully Registered", message.ToString());
        }

        [Fact]
        public void Return_User_Details()
        {
            var result = new User()
            {
                Id=3,
                FirstName = "anrup",
                LastName = "purna",
                Email = "anrup100@gmail.com",
                PhoneNumber = "1234567899",
                Address = "string",
                Zipcode = 123654,
                Password = "string",
                Role = "Guest",
                Token = "string"
            };
            mock.Setup(p => p.UserDetails(3)).Returns(result);
            var controller = new UserController(null,mock.Object);
            var result1 = controller.UserData(3);
            
            Assert.True(result.Equals(result1.Value));
        }
    } 
}
