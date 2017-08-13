using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glados.Core.Interfaces
{
    public interface IProgressBar
    {
        void ShowProgressDialog(string message);

        void HideProgressDialog();

    }
}
