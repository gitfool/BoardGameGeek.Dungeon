#load nuget:?package=Cake.Dungeon&version=1.0.1-pre.2

Build.SetParameters
(
    title: "BoardGameGeek.Dungeon",
    configuration: "Release",

    defaultLog: true,

    runBuild: true,
    runBuildPublish: true,
    runUnitTests: true,
    runDockerBuild: true,
    runPublishToDocker: true,

    sourceDirectory: Build.Directories.Root,

    buildPublishProjectPatterns: new[] { "Application/*.csproj" },
    unitTestProjectPatterns: new[] { "Tests/*.csproj" },

    buildEmbedAllSources: true,
    buildTreatWarningsAsErrors: true,
    dockerPushLatest: true,

    dockerImages: new[] { new DockerImage { Repository = "dockfool/boardgamegeek-dungeon", Context = "Application" } }
);

Build.Run();
