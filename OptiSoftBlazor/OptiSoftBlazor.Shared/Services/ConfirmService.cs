using Microsoft.FluentUI.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace OptiSoftBlazor.Shared.Services
{
    // Services/ConfirmService.cs
    public class ConfirmService
    {
        private readonly IDialogService _dialogService;

        public ConfirmService(IDialogService dialogService)
        {
            _dialogService = dialogService;
        }

        public async Task<bool> ConfirmAsync(string mensaje, string titulo = "Confirmación")
        {
            var dialog = await _dialogService.ShowConfirmationAsync(
                mensaje,
                "Si",
                "No",
                titulo
            );
            var result = await dialog.Result;
            return !result.Cancelled;
        }
    }
}
