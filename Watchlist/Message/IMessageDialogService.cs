using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watchlist.Message
{
    public interface IMessageDialogService
    {
        void ShowMessage(string message);
    }
}
