using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glados.Core.Interfaces
{
    public interface IDialog
    {
        Task<bool> ShowDialog();

        Task<bool> ShowFavoriteConfirmationDialog();

        Task<bool> ShowCancelResponseConfirmationDialog();

        void showConfirmationDialog();

        void hideConfirmationDialog();

        void showErrorConfirmationDialog(string errorMessage);
        
    }
}
