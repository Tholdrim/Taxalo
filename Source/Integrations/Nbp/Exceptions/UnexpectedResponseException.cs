namespace Taxalo.Integrations.Nbp
{
    internal class UnexpectedResponseException(Exception? innerException = null)
        : Exception("An unexpected response was received from the NBP server.", innerException)
    {
    }
}
