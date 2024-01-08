namespace BoardGameGeek.Dungeon.Extensions;

public static class ResiliencePipelineExtensions
{
    public static ValueTask<TResult> ExecuteAsync<TResult>(this ResiliencePipeline<TResult> pipeline, Func<Task<TResult>> callback) => pipeline.ExecuteAsync(_ => new ValueTask<TResult>(callback()));

    public static ValueTask<TResult> ExecuteAsync<TResult>(this ResiliencePipeline<TResult> pipeline, Func<CancellationToken, Task<TResult>> callback, CancellationToken cancellationToken = default) => pipeline.ExecuteAsync(ct => new ValueTask<TResult>(callback(ct)), cancellationToken);
}
