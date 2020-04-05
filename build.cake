#load nuget:?package=Cake.Dungeon&prerelease

Build.SetParameters
(
    title: "BoardGameGeek.Dungeon",
    configuration: "Release",

    defaultLog: true,

    runBuildSolutions: true,
    runDockerBuild: true,
    runUnitTests: true,
    runPublishToDocker: true,

    sourceDirectory: Build.Directories.Root,

    unitTestProjectPatterns: new[] { "Tests/*.csproj" },

    buildEmbedAllSources: true,
    dockerPushLatest: true,

    dockerImages: new[]
    {
        new DockerImage
        {
            Repository = "dockfool/boardgamegeek-dungeon",
            Context = "Application",
            Args = new[] { "configuration={{ Build.Parameters.Configuration }}" }
        }
    }
);

Build.Run();
