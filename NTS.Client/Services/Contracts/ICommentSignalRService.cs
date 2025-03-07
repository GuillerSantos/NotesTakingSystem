namespace NTS.Client.Services.Contracts
{
    public interface ICommentSignalRService
    {
        #region Events

        event Action<Guid, Guid, Guid, string, string, DateTime> OnCommentReceived;

        #endregion Events

        #region Public Methods

        Task StartAsync();

        Task StopAsync();

        Task SendCommentAsync(Guid noteId, Guid userId, Guid sharedNoteId, string fullName, DateTime createdAt, string commentContent);

        #endregion Public Methods
    }
}