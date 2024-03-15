using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using ElevatorBackend.Models;

namespace ElevatorBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ElevatorController : ControllerBase
    {
        private readonly ElevatorDBContext _context;
        private bool moving;
        private Queue<int> requestsQueue;
        private static int[] Floors;

        public ElevatorController(ElevatorDBContext context)
        {
            _context = context;
            moving = false;
            requestsQueue = new Queue<int>();
            Floors = new[]
            {
                 1, 2, 3, 4, 5,  6
            };
        }



        [HttpPost("call")]
        public IActionResult CallElevator([FromBody] int floor)
        {
            if (floor < 1 || floor > Floors.Length)
            {
                return BadRequest("Piso inválido");
            }

            requestsQueue.Enqueue(floor);
            this.moving = true;


            return Ok($"Se ha llamado al ascensor al piso {Floors[floor - 1]}");
        }

        [HttpPost("open")]
        public IActionResult OpenDoors()
        {
            // identificar en qué piso está
            var elevator = _context.Elevators.First();
            if (elevator == null)
            {
                return BadRequest("Ascensor no encontrado");
            }

            if (!elevator.DoorsOpen)
            {
                elevator.DoorsOpen = true;
                _context.SaveChanges();
                var delete = requestsQueue.Dequeue();

                return Ok($"Puertas del ascensor {elevator.Name} abiertas");
            }

            return Ok($"Las puertas del ascensor {elevator.Name} ya están abiertas");
        }

        [HttpPost("close")]
        public IActionResult CloseDoors()
        {
            var elevator = _context.Elevators.First();
            if (elevator == null)
            {
                return BadRequest("Ascensor no encontrado");
            }

            if (elevator.DoorsOpen)
            {
                elevator.DoorsOpen = false;
                _context.SaveChanges();
                return Ok($"Puertas del ascensor {elevator.Name} cerradas");
            }

            return Ok($"Las puertas del ascensor {elevator.Name} ya están cerradas");
        }


        [HttpPost("start")]
        public IActionResult StartElevator()
        {
            if (!moving)
            {
                //moving = true;
                return Ok("Ascensor iniciado");
            }

            return Ok("El ascensor ya está en movimiento");
        }

        [HttpPost("stop")]
        public IActionResult StopElevator()
        {
            if (moving)
            {
                moving = false;
                return Ok("Ascensor detenido");
            }

            return Ok("El ascensor ya está detenido");
        }

    }
}
