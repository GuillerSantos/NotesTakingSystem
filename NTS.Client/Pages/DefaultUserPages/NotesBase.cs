using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.Razor;
using MudBlazor;
using NTS.Client.Components;
using NTS.Client.Models.DTOs;
using NTS.Client.Services.Contracts;

namespace NTS.Client.Pages.DefaultUserPages
{
    public class NotesBase : ComponentBase
    {
        [Inject] public INotesService notesService { get; set; }
        [Inject] public ISnackbar snackbar { get; set; }
        [Inject] public IDialogService dialogService { get; set; }

        public readonly DialogOptions dialogOptions = new DialogOptions()
        {
            MaxWidth = MaxWidth.Small,
            FullWidth = true,
            NoHeader = true,
        };

        public List<NoteDto> notes { get; set; } = new List<NoteDto>();
        public bool isLoggedIn = false;
        public bool isFetched = false;

        protected override async Task OnInitializedAsync()
        {
            await LoadNotesAsync();
        }


        public async Task LoadNotesAsync()
        {
            try
            {
                if (notesService == null)
                {
                    Console.WriteLine("notesService Is Null");
                    isFetched = false;
                    return;
                }

                var result = await notesService.GetAllNotesAsync();
                if (result == null)
                {
                    Console.WriteLine("GetAllNotesAsync Returned Null");
                    notes = new List<NoteDto>();
                }
                else
                {
                    notes = result.ToList();
                }

                isFetched = true;
            }
            catch (Exception error)
            {
                Console.WriteLine($"Error Fetching All Notes: {error.Message}");
                isFetched = false;
            }
        }



        public async Task OpenCreateOrUpdateNoteDialogAsync(NoteDto note = null)
        {
            try
            {
                string dialogHeader = note == null ? "Create Note" : "Update Note";

                var parameters = new DialogParameters
                {
                    { "note", note} // Pass The Selected Note If Updating, Or Null if Creating A Note
                };

                var dialog = dialogService.Show<CreateOrUpdateNoteDialog>(dialogHeader, parameters, dialogOptions);
                var result = await dialog.Result;

                if (!result.Canceled)
                {
                    notes = await notesService.GetAllNotesAsync();
                }
            }
            catch (Exception error)
            {
                snackbar.Add($"Faild To Refresh Notes :{error.Message}", Severity.Error);
            }
        }
    }
}
