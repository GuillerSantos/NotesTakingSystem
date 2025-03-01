using Microsoft.AspNetCore.Components;
using NTS.Client.Models;
using NTS.Client.Services.Contracts;
using YourApp.Client.Securities;

namespace NTS.Client.Pages.DefaultUserPages
{
    public class HomeBase : ComponentBase, IAsyncDisposable
    {
        [Inject] private ISharedNotesService sharedNotesService { get; set; } = default!;
        [Inject] private ICommentSignalRService commentSignalRService { get; set; } = default!;
        [Inject] private ICommentsService commentsService { get; set; } = default!;
        [Inject] private CustomAuthenticationStateProvider authenticationStateProvider { get; set; } = default!;

        public List<SharedNotes> filteredNotes = new List<SharedNotes>();
        public List<SharedNotes> sharedNotesList = new();
        public Dictionary<Guid, List<Comment>> noteComments = new();
        public SharedNotes sharedNotes = new SharedNotes();
        public string newCommentContent { get; set; } = string.Empty;
        public HashSet<Guid> visibleComments = new HashSet<Guid>();
        public Guid userId { get; set; }
        public bool isFetched = false;

        protected override async Task OnInitializedAsync()
        {
            var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity!.IsAuthenticated)
            {
                var userIdClaim = user.FindFirst(c => c.Type == "sub")?.Value;
                userId = Guid.TryParse(userIdClaim, out Guid parsedUserId) ? parsedUserId : Guid.Empty;
            }

            await base.OnInitializedAsync();
            await LoadSharedNotesAsync();
            await commentSignalRService.StartAsync();
            commentSignalRService.OnCommentReceived += OnCommentReceived;
        }


        public async Task LoadSharedNotesAsync()
        {
            try
            {
                sharedNotesList = await sharedNotesService.GetAllSharedNotesAsync() ?? new List<SharedNotes>();
                filteredNotes = sharedNotesList.ToList();
            }
            catch (Exception error)
            {
                Console.WriteLine($"Error Fetching All Notes: {error.Message}");
            }

            isFetched = true;
            StateHasChanged();
        }


        public async Task SendComment(Guid noteId)
        {
            if (!string.IsNullOrWhiteSpace(newCommentContent))
            {
                var note = sharedNotesList.FirstOrDefault(n => n.NoteId == noteId);
                if (note != null)
                {
                    await commentSignalRService.SendCommentAsync(
                        noteId,
                        userId,
                        note.SharedNoteId,
                        note.FullName,

                        DateTime.Now,
                        newCommentContent
                    );

                    newCommentContent = string.Empty;
                    await LoadComments(noteId);
                }
            }
        }


        public async Task LoadComments(Guid noteId)
        {
            try
            {
                var comments = await commentsService.GetCommentsForNoteAsync(noteId);

                if (comments != null)
                {
                    noteComments[noteId] = comments.ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Loading Comments: {ex.Message}");
            }
        }


        public void OnCommentReceived(Guid noteId, Guid userId, Guid sharedNoteId, string fullName, string commentContent, DateTime createdAt)
        {
            if (!noteComments.ContainsKey(noteId))
            {
                noteComments[noteId] = new List<Comment>();
            }

            noteComments[noteId].Add(new Comment
            {
                NoteId = noteId,
                UserId = userId,
                SharedNoteId = sharedNoteId,
                FullName = fullName,
                CommentContent = commentContent,
                CreatedAt = createdAt
            });

            StateHasChanged();
        }


        public void ToggleCommentsVisibility(Guid noteId)
        {
            if (visibleComments.Contains(noteId))
            {
                visibleComments.Remove(noteId);
            }
            else
            {
                visibleComments.Add(noteId);
                _ = LoadComments(noteId);
            }
        }


        public async ValueTask DisposeAsync()
        {
            commentSignalRService.OnCommentReceived -= OnCommentReceived;
            await commentSignalRService.StopAsync();
        }
    }
}