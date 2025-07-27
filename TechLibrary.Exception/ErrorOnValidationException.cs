using System.Net;

namespace TechLibrary.Exception
{
    public class ErrorOnValidationException : TechLibraryException
    {

        // readonly: somente o construtor pode setar valores para esse atributo
        // boa prática: começar atributos readonly com _ (underline)
        private readonly List<string> _errors;

        public ErrorOnValidationException(List<string> errorMessages)
        {
            _errors = errorMessages;
        }

        // funções de uma linha
        public override List<string> GetErrorMessages() => _errors;

        public override HttpStatusCode GetStatusCode() => HttpStatusCode.BadRequest;
    }
}
