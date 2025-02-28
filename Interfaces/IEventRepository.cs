using AlumniManagement.Frontend.EventService;
using AlumniManagement.Frontend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AlumniManagement.Frontend.Interfaces
{
    public interface IEventRepository
    {
        
        IEnumerable<EventModel> GetAllEvents();

        
        EventModel GetEventById(int eventId);

        
        void UpsertEvent(EventModel EventModel);

        
        void DeleteEvent(int eventId);
    }
}
