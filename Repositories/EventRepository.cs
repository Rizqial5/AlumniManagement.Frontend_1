using AlumniManagement.Frontend.EventService;
using AlumniManagement.Frontend.FacultyService;
using AlumniManagement.Frontend.Interfaces;
using AlumniManagement.Frontend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlumniManagement.Frontend.Repositories
{
    public class EventRepository : IEventRepository
    {
        private  EventServiceClient _eventService;

        public EventRepository()
        {
            _eventService = new EventServiceClient();
        }
        public IEnumerable<EventModel> GetAllEvents()
        {
            var data = _eventService.GetAllEvents();
            var result = data.Select(f => Mapping.Mapper.Map<EventModel>(f));

            return result.ToList();
        }

        public EventModel GetEventById(int eventId)
        {
            var selectedData = _eventService.GetEventById(eventId);

            return Mapping.Mapper.Map<EventModel>(selectedData);
        }

        public void UpsertEvent(EventModel eventDTO)
        {
            var result = Mapping.Mapper.Map<EventDTO>(eventDTO);

            _eventService.UpsertEvent(result);
        }

        public void DeleteEvent(int eventId)
        {
            _eventService.DeleteEvent(eventId);
        }
    }
}