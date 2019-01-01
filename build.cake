#load nuget:?package=Cake.Dungeon&version=1.0.0-pre.2

Build.SetParameters
(
    title: "BoardGameGeek.Dungeon",
    configuration: "Release",

    // defaultLog: true,
    // logBuildSystem: false,
    // logContext: false,

    runBuild: true,
    runUnitTests: true,

    sourceDirectory: Build.Directories.Root,

    unitTestProjectPatterns: new [] { "Tests/*.csproj" },

    buildEmbedAllSources: true,
    buildTreatWarningsAsErrors: true
);

Build.Run();
