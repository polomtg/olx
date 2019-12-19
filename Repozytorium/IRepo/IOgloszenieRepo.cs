using Repozytorium.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repozytorium.IRepo
{
    interface IOgloszenieRepo
    {
        IQueryable<Ogloszenie> PobierzOgloszenia();
    }
}
