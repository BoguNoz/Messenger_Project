using Microsoft.AspNetCore.Mvc;
using Messager_Project.DTO.Emotes;
using Messager_Project.Model.Enteties;

namespace Messager_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmotesController : ControllerBase
    {
        public readonly Messager_Project.Repository.Emote.IEmotesRepository _emotesRepository;
        public EmotesController(Messager_Project.Repository.Emote.IEmotesRepository emotesRepository)
        {
            _emotesRepository = emotesRepository;
        }
        /// <summary>
        /// Zwraca emotkę o podanym Id
        /// </summary>
        /// <returns></returns>

        [HttpGet("{Emote_Id}")]
        public async Task<IActionResult> GetEmoteById(int Emote_Id)
        {
            var emote = await _emotesRepository.GetEmotesByIdAsync(Emote_Id);
            if (emote == null)
                return (NotFound());
            return Ok(emote);
        }
            /// <summary>
            /// Zwraca wszystkie emotki
            /// </summary>
            /// <returns></returns>

        [HttpGet]
        public async Task<IActionResult> GetAllEmotesById()
        {
            var emotes = await _emotesRepository.GetAllEmotesByIdAsync();
            return Ok(emotes);
        }
        /// <summary>
        /// Zmienia nazwę, domyślny kolor oraz unicode emotki o podanym Id
        /// </summary>
        /// <returns></returns>

        [HttpPut("{Emote_Id}")]
        public async Task<IActionResult> ChangeEmote(int Emote_Id, [FromBody] EmotesDto emotesDto)
        {
            var emote = await _emotesRepository.GetEmotesByIdAsync(Emote_Id);
            if (emote == null)
                return BadRequest();
            emote.Emote_Name = emotesDto.Emote_Name;
            emote.Emote_Default_Color = emotesDto.Emote_Default_Color;
            emote.Emote_Unicode = emotesDto.Emote_Unicode;

            //Dodany result
            var result = await _emotesRepository.SaveEmoteAsync(emote);

            //Zmiany -> BoguNoz
            if (!result.Status)
                throw new Exception("Error saving user to database");
            //
            return Ok();
        }
        /// <summary>
        /// Zmienia nazwę emotki o podanym Id
        /// </summary>
        /// <returns></returns>

        [HttpPut("{Emote_Id}/name")]
        public async Task<IActionResult> ChangeEmoteName(int Emote_Id, [FromBody] EmotesDto emotesDto)
        {
            var emote = await _emotesRepository.GetEmotesByIdAsync(Emote_Id);
            if (emote == null)
                return BadRequest();
            emote.Emote_Name = emotesDto.Emote_Name;
            var result = await _emotesRepository.SaveEmoteAsync(emote);

            //Zmiany -> BoguNoz
            if (!result.Status)
                throw new Exception("Error saving user to database");
            //
            return Ok();
        }
        /// <summary>
        /// Zmienia domyślny kolor emotki o podanym Id
        /// </summary>
        /// <returns></returns>

        [HttpPut("{Emote_Id}/color")]
        public async Task<IActionResult> ChangeEmoteDefaultColor(int Emote_Id, [FromBody] EmotesDto emotesDto)
        {
            var emote = await _emotesRepository.GetEmotesByIdAsync(Emote_Id);
            if (emote == null)
                return BadRequest();
            emote.Emote_Default_Color = emotesDto.Emote_Default_Color;
            var result = await _emotesRepository.SaveEmoteAsync(emote);

            //Zmiany -> BoguNoz
            if (!result.Status)
                throw new Exception("Error saving user to database");
            //

            return Ok();
        }

        /// <summary>
        /// Zmienia unicode emotki o podanym Id
        /// </summary>
        /// <returns></returns>

        [HttpPut("{Emote_Id}/unicode")]
        public async Task<IActionResult> ChangeEmotenewEmoteUnicode(int Emote_Id, [FromBody] EmotesDto emotesDto)
        {
            var emote = await _emotesRepository.GetEmotesByIdAsync(Emote_Id);
            if (emote == null)
                return BadRequest();
            emote.Emote_Unicode = emotesDto.Emote_Unicode;

            var result = await _emotesRepository.SaveEmoteAsync(emote);

            //Zmiany -> BoguNoz
            if (!result.Status)
                throw new Exception("Error saving user to database");
            //
            return Ok();
        }
        /// <summary>
        /// Dodaje nową emotkę
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        public async Task<IActionResult> AddNewEmote([FromBody] EmotesDto emotesDto)
        {
            //Zmiany -> BoguNoz

            if (emotesDto == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //

            var newEmote = new Emotes
            {
                Emote_Name = emotesDto.Emote_Name,
                Emote_Unicode = emotesDto.Emote_Unicode,
                Emote_Default_Color = emotesDto.Emote_Default_Color
            };

            var result = await _emotesRepository.SaveEmoteAsync(newEmote);

            //Zmiany -> BoguNoz
            if (!result.Status)
                throw new Exception("Error saving user to database");
            //
            return Ok();
        }
        /// <summary>
        /// Usuwa emotkę o podanym Id
        /// </summary>
        /// <returns></returns>

        [HttpDelete("{Emote_Id}")]
        public async Task<IActionResult> RemoveEmote(int Emote_Id)
        {
            var result = await _emotesRepository.DeleteEmoteAsync(Emote_Id);
            //Zmiany -> BoguNoz
            if (!result.Status)
                throw new Exception("Error saving user to database");
            return Ok();
        }
    }
}
