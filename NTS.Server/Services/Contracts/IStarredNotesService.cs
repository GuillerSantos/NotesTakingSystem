namespace NTS.Server.Services.Contracts
{
    public interface IStarredNotesService
    {
        Task<bool> MarkNoteAsStarredAsync(Guid noteId, Guid userId);

        Task RemoveByNoteIdAsync(Guid noteId);
    }
}
