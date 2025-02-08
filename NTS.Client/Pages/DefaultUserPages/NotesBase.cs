﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.Razor;
using MudBlazor;
using NTS.Client.Components;
using NTS.Client.DTOs;
using NTS.Client.Services.Contracts;
using NTS.Client.Utilities;

namespace NTS.Client.Pages.DefaultUserPages
{
    public class NotesBase : ComponentBase
    {
        [Inject] public INotesService notesService { get; set; } = default!;
        [Inject] public ISnackbar snackbar { get; set; } = default!;
        [Inject] public IDialogService dialogService { get; set; } = default!;


        public List<NoteDto> allNotes { get; set; } = new List<NoteDto>();
        public string searchQuery { get; set; }
        public List<NoteDto> filteredNotes { get; set; } = new List<NoteDto>();
        public bool isLoggedIn = false;
        public bool isFetched = false;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await LoadNotesAsync();
        }


        public async Task LoadNotesAsync()
        {
            try
            {

                allNotes = (await notesService.GetAllNotesAsync())?.ToList() ?? new List<NoteDto>();

                if (allNotes is null)
                {
                    Console.WriteLine("GetAllNotesAsync Returned Null");
                    allNotes = new List<NoteDto>();
                }
                else
                {
                    allNotes = allNotes.ToList();
                }

                filteredNotes = allNotes.ToList();
                isFetched = true;

                StateHasChanged();
            }
            catch (Exception error)
            {
                Console.WriteLine($"Error Fetching All Notes: {error.Message}");
                isFetched = false;
            }
        }


        public async Task SearchNotesAsync(string searchQuery)
        {
            try
            {
                isFetched = false;
                if (!string.IsNullOrWhiteSpace(searchQuery))
                {
                    var searchResult = await notesService.SearchNotesAsync(searchQuery);
                    filteredNotes = searchResult?.ToList() ?? new List<NoteDto>();
                }
                else
                {
                    filteredNotes = allNotes.ToList();

                }
                isFetched = true;
            }
            catch (Exception error)
            {
                Console.WriteLine($"Error Searching Notes: {error.Message}");
                isFetched = true;
            }
        }


        public async Task OpenCreateOrUpdateNoteDialogAsync(NoteDto note = null)
        {
            try
            {
                // If The Note Is Not Null = Update Note
                string dialogHeader = note == null ? "Create Note" : "Update Note";

                var dialogOptions = new DialogOptions()
                {
                    // If The Note Is Note Null = MaxWidth.Large
                    MaxWidth = note == null ? MaxWidth.Small : MaxWidth.Large,
                    FullWidth = true,
                    NoHeader = true,
                };

                var parameters = new DialogParameters
                {
                    { "note", note }, // Pass The Selected Note If Updating, Or Null if Creating A Note
                    { "notesBase", this }
                };

                var dialog = dialogService.Show<CreateOrUpdateNoteDialog>(dialogHeader, parameters, dialogOptions);
                var result = await dialog.Result;

                if (!result.Canceled)
                {
                    await LoadNotesAsync();
                }
            }
            catch (Exception error)
            {
                snackbar.Add($"Faild To Refresh Notes :{error.Message}", Severity.Error);
            }
        }


        public string GetNoteStyle(NoteDto note)
        {
            string lighterColor = NotesCardColorUtil.GetLighterOrDarkerColor(note.Color);
            return $"background-image: linear-gradient(135deg, {note.Color}, {lighterColor});";
        }
    }
}
