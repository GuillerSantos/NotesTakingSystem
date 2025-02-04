using Microsoft.AspNetCore.Components;
using NTS.Client.Models;
using NTS.Client.Services.Contracts;

namespace NTS.Client.Pages.DefaultUserPages
{
    public class HomeBase : ComponentBase
    {
        [Inject] private ISharedNotesService sharedNotesService { get; set; } = default!;

        public SharedNotes sharedNotes { get; set; } = default!;


    }
}
