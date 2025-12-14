using IntegratorMobile.MockData.Models;

namespace IntegratorMobile.MockData.Services;

public interface IAppointmentService
{
    Task<List<Appointment>> GetTodayAppointments();
    Task<List<Appointment>> GetPastAppointments();
    Task<List<Appointment>> GetFutureAppointments();
    Task<Appointment?> GetAppointmentById(string id);
    Task UpdateAppointmentStatus(string id, AppointmentStatus status);
    Task<List<WorkItem>> GetWorkItemsForAppointment(string appointmentId);
    Task<WorkItem?> GetWorkItemById(string id);
    Task UpdateWorkItemStatus(string id, WorkItemStatus status);
}
