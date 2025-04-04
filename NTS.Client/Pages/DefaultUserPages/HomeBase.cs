﻿using Microsoft.AspNetCore.Components;
using NTS.Client.Models;
using NTS.Client.Services.Contracts;
using YourApp.Client.Securities;

namespace NTS.Client.Pages.DefaultUserPages
{
    public class HomeBase : ComponentBase, IAsyncDisposable
    {
        #region Fields

        public List<SharedNotes> sharedNotesList = new();
        public Dictionary<Guid, List<Comment>> noteComments = new();
        public HashSet<Guid> visibleComments = new();
        public bool isFetched = false;

        #endregion Fields

        #region Properties

        public string newCommentContent { get; set; } = string.Empty;
        public Guid userId { get; set; }

        [Inject] private ISharedNotesService sharedNotesService { get; set; } = default!;
        [Inject] private ICommentSignalRService commentSignalRService { get; set; } = default!;
        [Inject] private ICommentsService commentsService { get; set; } = default!;
        [Inject] private CustomAuthenticationStateProvider authenticationStateProvider { get; set; } = default!;

        #endregion Properties

        #region Public Methods

        public async Task LoadSharedNotesAsync()
        {
            sharedNotesList = await sharedNotesService.GetAllSharedNotesAsync() ?? new();
            isFetched = true;
            StateHasChanged();
        }

        public async Task SendComment(Guid noteId)
        {
            if (string.IsNullOrWhiteSpace(newCommentContent)) return;

            var note = sharedNotesList.FirstOrDefault(n => n.NoteId == noteId);
            if (note is null) return;

            await commentSignalRService.SendCommentAsync(
                noteId, userId, note.SharedNoteId, note.FullName, DateTime.Now, newCommentContent
            );

            newCommentContent = string.Empty;
            await LoadComments(noteId);
        }

        public async Task LoadComments(Guid noteId)
        {
            noteComments[noteId] = (await commentsService.GetCommentsForNoteAsync(noteId))?.ToList() ?? new();
        }

        public void OnCommentReceived(Guid noteId, Guid userId, Guid sharedNoteId, string fullName, string commentContent, DateTime createdAt)
        {
            noteComments[noteId] ??= new List<Comment>();

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
            if (!visibleComments.Add(noteId))
            {
                visibleComments.Remove(noteId);
            }
            else
            {
                _ = LoadComments(noteId);
            }
        }

        public async ValueTask DisposeAsync()
        {
            commentSignalRService.OnCommentReceived -= OnCommentReceived;
            await commentSignalRService.StopAsync();
        }

        #endregion Public Methods

        #region Protected Methods

        protected override async Task OnInitializedAsync()
        {
            var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity?.IsAuthenticated == true)
            {
                var userIdClaim = user.FindFirst(c => c.Type == "sub")?.Value;
                userId = Guid.TryParse(userIdClaim, out var parsedUserId) ? parsedUserId : Guid.Empty;
            }

            await LoadSharedNotesAsync();
            await commentSignalRService.StartAsync();
            commentSignalRService.OnCommentReceived += OnCommentReceived;
        }

        #endregion Protected Methods
    }
}