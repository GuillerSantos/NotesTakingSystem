using Microsoft.AspNetCore.Components;
using NTS.Client.Models;
using NTS.Client.Services;
using NTS.Client.Services.Contracts;
using YourApp.Client.Securities;

namespace NTS.Client.Pages.DefaultUserPages
{
    public class HomeBase : ComponentBase
    {
        [Inject] private ISharedNotesService sharedNotesService { get; set; } = default!;
        [Inject] private CommentSignalRService commentSignalRService { get; set; } = default!;
        [Inject] private ICommentsService commentsService { get; set; } = default!;
        [Inject] private CustomAuthenticationStateProvider authenticationStateProvider { get; set; } = default!;

        public List<SharedNotes> sharedNotesList = new List<SharedNotes>();
        public List<Comment> comments { get; set; } = new List<Comment>();
        public SharedNotes sharedNotes = new SharedNotes();
        public string newCommentContent { get; set; } = string.Empty;
        public Guid userId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity!.IsAuthenticated)
            {
                var userIdClaim = user.FindFirst(c => c.Type == "sub")?.Value;

                if (Guid.TryParse(userIdClaim, out Guid parsedUserId))
                {
                    userId = parsedUserId;
                }
                else
                {
                    userId = Guid.Empty;
                }
            }

            sharedNotesList = await sharedNotesService.GetAllSharedNotesAsync(userId);

            await commentSignalRService.StartAsync();
            commentSignalRService.OnCommentReceived += OnCommentReceived;
        }

        public async Task LoadComments(Guid noteId)
        {
            comments = await commentsService.GetCommentsForNoteAsync(noteId);
        }


        public void OnCommentReceived(Guid noteId, Guid userId, string title, string fullName, string commentContent, DateTime createdAt)
        {
            comments.Add(new Comment
            {
                NoteId = noteId,
                UserId = userId,
                Title = title,
                FullName = fullName,
                CommentContent = commentContent,
                CreatedAt = DateTime.Now
            });

            StateHasChanged();
        }


        public async Task SendComment()
        {
            if (!string.IsNullOrEmpty(newCommentContent))
            {
                await commentSignalRService.SendCommentAsync(
                    sharedNotes.NoteId,
                    sharedNotes.UserId,
                    sharedNotes.Title,
                    sharedNotes.FullName,
                    sharedNotes.CreatedAt,
                    newCommentContent
                    );

                newCommentContent = string.Empty;
            }
        }
    }
}
