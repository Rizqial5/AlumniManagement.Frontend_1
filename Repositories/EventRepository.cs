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
        private EventServiceClient _eventService;

        public EventRepository()
        {
            _eventService = new EventServiceClient();
        }

        public IEnumerable<EventModel> GetAllEvents()
        {
            try
            {
                var data = _eventService.GetAllEvents();
                var result = data.Select(f => Mapping.Mapper.Map<EventModel>(f));

                return result.ToList();
            }
            catch (Exception ex)
            {
                // Log error untuk developer (bisa menggunakan logging framework seperti Serilog/NLog)
                throw new Exception("Failed to retrieve events. Please try again later.");
            }
        }

        public EventModel GetEventById(int eventId)
        {
            try
            {
                var selectedData = _eventService.GetEventById(eventId);
                return Mapping.Mapper.Map<EventModel>(selectedData);
            }
            catch (Exception ex)
            {
                // Log error untuk developer
                throw new Exception($"Failed to retrieve event . Please try again later.");
            }
        }

        public void UpsertEvent(EventModel eventDTO)
        {
            try
            {
                var result = Mapping.Mapper.Map<EventDTO>(eventDTO);
                _eventService.UpsertEvent(result);
            }
            catch (Exception ex)
            {
                // Log error untuk developer
                throw new Exception("Failed to save the event. Please try again later.");
            }
        }

        public void DeleteEvent(int eventId)
        {
            try
            {
                _eventService.DeleteEvent(eventId);
            }
            catch (Exception ex)
            {
                // Log error untuk developer
                throw new Exception($"Failed to delete event . Please try again later.");
            }
        }
    }
}