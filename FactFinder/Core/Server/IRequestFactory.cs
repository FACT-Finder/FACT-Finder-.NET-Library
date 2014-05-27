
namespace Omikron.FactFinder.Core.Server
{
    interface IRequestFactory
    {
        /**
         * Returns a Request object all wired up and ready for use.
         */
        Request GetRequest();
    }
}
