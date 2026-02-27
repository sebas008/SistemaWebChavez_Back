using Chavez_Logistica.Dtos.Usuarios;
using Chavez_Logistica.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Chavez_Logistica.Controllers
{
    [ApiController]
    [Route("api/seguridad/usuarios")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _service;

        public UsuarioController(IUsuarioService service)
        {
            _service = service;
        }

        // GET: api/seguridad/usuarios
        [HttpGet]
        public async Task<ActionResult<List<UsuarioDto>>> Listar(CancellationToken ct)
        {
            var data = await _service.ListarAsync(ct);
            return Ok(data);
        }

        // GET: api/seguridad/usuarios/5
        [HttpGet("{idUsuario:int}")]
        public async Task<ActionResult<UsuarioDto>> ObtenerPorId(int idUsuario, CancellationToken ct)
        {
            var item = await _service.ObtenerPorIdAsync(idUsuario, ct);
            if (item == null)
                return NotFound();

            return Ok(item);
        }

        // POST: api/seguridad/usuarios
        [HttpPost]
        public async Task<ActionResult<UsuarioCreateResponseDto>> Crear(
            [FromBody] UsuarioCreateRequestDto req,
            CancellationToken ct)
        {
            if (req == null)
                return BadRequest();

            var resp = await _service.CrearAsync(req, ct);
            return Ok(resp);
        }

        // PUT: api/seguridad/usuarios/5
        [HttpPut("{idUsuario:int}")]
        public async Task<IActionResult> Actualizar(
            int idUsuario,
            [FromBody] UsuarioUpdateRequestDto req,
            CancellationToken ct)
        {
            if (req == null)
                return BadRequest();

            await _service.ActualizarAsync(idUsuario, req, ct);
            return NoContent();
        }

        // POST: api/seguridad/usuarios/5/roles
        [HttpPost("{idUsuario:int}/roles")]
        public async Task<IActionResult> AsignarRoles(
            int idUsuario,
            [FromBody] UsuarioAsignarRolesRequestDto req,
            CancellationToken ct)
        {
            if (req == null)
                return BadRequest();

            await _service.AsignarRolesAsync(idUsuario, req, ct);
            return NoContent();
        }
    }
}
