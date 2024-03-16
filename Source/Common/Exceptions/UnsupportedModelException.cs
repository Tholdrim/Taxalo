namespace Taxalo
{
    internal class UnsupportedModelException()
        : Exception("The specified model is not supported by this writer.")
    {
    }
}
