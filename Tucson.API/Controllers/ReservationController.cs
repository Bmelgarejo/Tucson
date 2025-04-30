using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Tucson.Application.Dto;
using Tucson.Application.Service.Interface;
using Tucson.Domain.Interfaces;

namespace Tucson.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;
        private readonly IReservationRepository _reservationRepository;

        public ReservationController(IReservationService reservationService, IReservationRepository reservationRepository)
        {
            _reservationService = reservationService;
            _reservationRepository = reservationRepository;
        }

        /// <summary>
        /// Obtiene todas las reservas existentes.
        /// </summary>
        /// <returns>Lista de reservas.</returns>
        [HttpGet("reservations")]
        public IActionResult GetReservations()
        {
            var reservations = _reservationRepository.GetAllReservations()
                .Select(r => new ReservationDto
                {
                    ReservationId = r.Id,
                    CustomerId = r.Customer.Id,
                    ReservationDate = r.ReservationDate
                });

            return Ok(reservations);
        }

        /// <summary>
        /// Elimina una reserva específica.
        /// </summary>
        /// <param name="reservationId">ID de la reserva a eliminar.</param>
        /// <returns>Resultado de la operación.</returns>
        [HttpDelete("reservations/{reservationId}")]
        public async Task<IActionResult> DeleteReservation(int reservationId)
        {
            try
            {
                await _reservationService.CancelReservationAsync(reservationId);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        /// <summary>
        /// Crea una nueva reserva.
        /// </summary>
        /// <param name="request">Datos de la solicitud de reserva.</param>
        /// <returns>Información de la mesa asignada.</returns>
        [HttpPost("create-reservation")]
        public async Task<IActionResult> CreateReservation([FromBody] ReservationRequestDto request)
        {
            try
            {
                var customer = _reservationRepository.GetCustomerById(request.CustomerId);
                if (customer == null)
                {
                    return NotFound(new { Message = "Cliente no encontrado." });
                }

                var reservation = await _reservationService.CreateReservationAsync(customer, request.ReservationDate, request.SeatCount);
                return Ok(new { TableId = reservation.TableId });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene la lista de clientes en espera
        /// </summary>
        /// <returns>Lista de clientes en espera.</returns>
        [HttpGet("waiting-list")]
        public IActionResult GetWaitingList()
        {
            var waitingList = _reservationRepository.GetWaitingList()
                .Select(w => new WaitingListDto
                {
                    CustomerId = w.Customer.Id,
                    CustomerName = w.Customer.Name,
                    ReservationDate = w.ReservationDate,
                    SeatCount = w.SeatCount
                });

            return Ok(waitingList);
        }
    }
}
