using TechLibrary.Api.Infraestructure.DataAccess;
using TechLibrary.Api.Infraestructure.Security.Cryptography;
using TechLibrary.Api.Infraestructure.Security.Tokens.Access;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;
using TechLibrary.Exception;

namespace TechLibrary.Api.UseCases.Login.DoLogin
{
    public class DoLoginUseCase
    {
        public ResponseRegisteredUserJson Execute(RequestLoginJson request)
        {
            var dbContext = new TechLibraryDbContext();

            // First (eu tenho certeza que o e-mail existe, se não encontrar gera uma runtime exception)
            // FirstOrDefault (caso não encontre vai me devolver nulo)
            var user = dbContext.Users.FirstOrDefault(user => user.Email.Equals(request.Email));
            if (user is null)
            {
                throw new InvalidLoginException();
            }

            var cryptography = new BCryptAlgorithm();
            var passwordIsValid = cryptography.Verify(request.Password, user);
            if (passwordIsValid == false)
            {
                throw new InvalidLoginException();
            }

            var tokenGenerator = new JwtTokenGenerator();

            return new ResponseRegisteredUserJson
            {
                Name = user.Name,
                AccessToken = tokenGenerator.Generate(user)
            };
        }
    }
}
