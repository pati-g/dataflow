using Microsoft.AspNetCore.Mvc;

namespace DataFlowAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataflowController : ControllerBase
    {
        private readonly ILogger<DataflowController> _logger;
        private readonly IDataflowService _service;

        public DataflowController(ILogger<DataflowController> logger, IDataflowService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpPost()]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CreateNewDataflow([FromQuery] RequestDto dto)
        {
            if(dto == null)
            {
                _logger.LogWarning("Null argument of type {arg}", typeof(RequestDto));
                return BadRequest("Please provide the filepath of the datasource and the delimeter type");
            }
            if (string.IsNullOrEmpty(dto.Filepath))
            {
                _logger.LogWarning("Null or empty property {prop} in the query of type {type}", nameof(dto.Filepath), typeof(RequestDto));
                return BadRequest("Please provide the filepath of the datasource");
            }
            if (string.IsNullOrEmpty(dto.Delimeter))
            {
                _logger.LogWarning("Null or empty property {prop} in the query of type {type}", nameof(dto.Delimeter), typeof(RequestDto));
                return BadRequest("Please provide the delimeter type for the data");
            }

            var resultPath = await _service.ProcessDataAsync(dto.Filepath, dto.Delimeter);

            if (resultPath == null)
            {
                _logger.LogInformation("Resource has not been found, pathfile: {path}", dto.Filepath);
                return NotFound($"No resource found for the file path: {dto.Filepath}");
            }
            return Ok(resultPath);
        }
    }
}