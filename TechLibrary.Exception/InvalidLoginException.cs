using System.Net;

namespace TechLibrary.Exception
{
    public class InvalidLoginException : TechLibraryException
    {
        public override List<string> GetErrorMessages() => /*Simplificado*/ ["E-mail e/ou senha inválidos"]; // new List<string> { "E-mail e/ou senha inválidos" };

        public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
    }
}
