﻿using FluentValidation.Results;
using TechLibrary.Api.Domain.Entities;
using TechLibrary.Api.Infraestructure.DataAccess;
using TechLibrary.Api.Infraestructure.Security.Cryptography;
using TechLibrary.Api.Infraestructure.Security.Tokens.Access;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;
using TechLibrary.Exception;

namespace TechLibrabry.Api.UseCases.Users.Register
{
    public class RegisterUserUseCase
    {
        public ResponseRegisteredUserJson Execute(RequestUserJson request)
        {
            var dbContext = new TechLibraryDbContext();

            Validate(request, dbContext);

            var cryptography = new BCryptAlgorithm();

            var user = new User
            {
                Email = request.Email,
                Name = request.Name,
                Password = cryptography.HashPassword(request.Password),
            };

            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            var tokenGenerator = new JwtTokenGenerator();

            return new ResponseRegisteredUserJson
            {
                Name = user.Name,
                AccessToken = tokenGenerator.Generate(user),
            };
        }

        private void Validate(RequestUserJson request, TechLibraryDbContext dbContext)
        {
            var validator = new RegisterUserValidator();

            var result = validator.Validate(request);

            var existUserWithEmail = dbContext.Users.Any(user => user.Email.Equals(request.Email));
            if (existUserWithEmail)
            {
                result.Errors.Add(new ValidationFailure("Email", "E-mail já registrado na plataforma"));
            }

            // !result.IsValid or result.IsValid == false
            if (result.IsValid == false)
            {
                var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
