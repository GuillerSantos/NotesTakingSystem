﻿using NTS.Client.Models;

namespace NTS.Client.Services.Contracts
{
    public interface IStarredNotesService
    {
        Task MarkNoteAsStarredAsync(StarredNotes request, Guid noteId);
    }
}
