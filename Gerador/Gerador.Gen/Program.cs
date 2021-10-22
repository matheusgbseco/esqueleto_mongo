using Common.Gen;

namespace Gerador.Gen
{
    class Program
    {
        static void Main(string[] args)
        {
            HelperFlow.Flow(args, new HelperSysObjects(new ConfigContext().GetConfigContext()));
        }

    }
}
