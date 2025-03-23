[assembly: AssertionEngineInitializer(typeof(AssemblyInitializers), nameof(AssemblyInitializers.AcceptLicense))]

namespace BoardGameGeek.Dungeon;

public static class AssemblyInitializers
{
    public static void AcceptLicense() => License.Accepted = true;
}
