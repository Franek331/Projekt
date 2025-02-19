using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeeSharp.Egzaminer.Domain.Entities;

namespace SeeSharp.Egzaminer.Application.Interfaces
{
    public interface ITestPublicationRepository : IKeyRepository<TestPublication, Guid>
    {
        Task<int> GetPendingGradingCount(); 
    }

}
