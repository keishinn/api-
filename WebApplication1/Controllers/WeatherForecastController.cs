using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase 
    {

        private readonly Supabase.Client _supabaseClient;
        private readonly SupaBaseContext _supabaseContext;

        public WeatherForecastController(Supabase.Client supabaseClient, SupaBaseContext supaBaseContext)
        {
            _supabaseClient = supabaseClient;
            _supabaseContext = supaBaseContext;
        }

        [HttpGet("GetAllUsers", Name = "GetAllUsers")]
        public async Task<IActionResult> GetAllUsers() 
        {
            try
            {
                var result = await _supabaseContext.GetUsers(_supabaseClient);
                return Ok(JsonConvert.SerializeObject(result, Formatting.Indented)); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка сервера: {ex.Message}");
            }
        }

        [HttpPost("InsertUser", Name = "InsertUser")]
        public async Task<ActionResult> InsertUser([FromBody] UserData userData)
        {
            try
            {
                if (string.IsNullOrEmpty(userData.Login) || string.IsNullOrEmpty(userData.Password))
                {
                    return BadRequest("Или логин или пароль пустой.");
                }
                else
                {
                    User newUser = new User
                    {
                        Id = 0,
                        Login = userData.Login,
                        Password = userData.Password,
                        Age = userData.Age
                    };

                    bool result = await _supabaseContext.InsertUser(_supabaseClient, newUser);

                    if (result == true)
                    {
                        return Ok("Регистрация прошла успешно.");
                    }
                    else
                    {
                        return BadRequest("Не удалось добавить пользователя в БД.");
                    }
                } 
            }
            catch (Exception )
            {
                return BadRequest( "Неизвестная ошибка.");
            }
        }

        [HttpPut("UpdateUser", Name = "UpdateUser")]
        public async Task<ActionResult> UpdateUser([FromBody] UserUpdateData userData)
        {
            try
            {
                if (userData.Id <= 0 ||
                    userData.Age <= 0 ||
                    string.IsNullOrEmpty(userData.Login) ||
                    string.IsNullOrEmpty(userData.Password))
                {
                    return BadRequest("Недопустимые данные для обновления");
                }

                var existingUser = await _supabaseClient.From<User>()
                    .Where(x => x.Id == userData.Id)
                    .Single();

                if (existingUser == null)
                {
                    return NotFound("Пользователь не найден");
                }

                existingUser.Login = userData.Login;
                existingUser.Password = userData.Password;
                existingUser.Age = userData.Age;

                await existingUser.Update<User>();

                return Ok("Пользовательские данные успешно обновлены");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка сервера: {ex.Message}");
            }
        }

        [HttpDelete("DeleteUser/{id}", Name = "DeleteUser")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Неверный ID пользователя");
                }

                var userToDelete = await _supabaseClient.From<User>()
                    .Where(x => x.Id == id)
                    .Single();

                if (userToDelete == null)
                {
                    return NotFound("Пользователь не найден");
                }

                await userToDelete.Delete<User>();
                return Ok("Пользователь успешно удален");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка сервера: {ex.Message}");
            }
        }

        [HttpGet("GetAllCities", Name = "GetAllCities")]
        public async Task<IActionResult> GetAllCities()
        {
            try
            {
                var result = await _supabaseContext.GetCities(_supabaseClient);
                return Ok(JsonConvert.SerializeObject(result, Formatting.Indented));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка сервера: {ex.Message}");
            }
        }

        [HttpPost("InsertCity", Name = "InsertCity")]
        public async Task<ActionResult> InsertCity([FromBody] CityData cityData)
        {
            try
            {
                if (string.IsNullOrEmpty(cityData.Name) || cityData.Population <= 0)
                {
                    return BadRequest("Название города и население обязательны");
                }

                City newCity = new City
                {
                    Name = cityData.Name,
                    Population = cityData.Population
                };

                bool result = await _supabaseContext.InsertCity(_supabaseClient, newCity);
                return result ? Ok("Город успешно добавлен") : BadRequest("Ошибка при добавлении города");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка сервера: {ex.Message}");
            }
        }

        [HttpPut("UpdateCity", Name = "UpdateCity")]
        public async Task<ActionResult> UpdateCity([FromBody] CityUpdateData cityData)
        {
            try
            {
                if (cityData.Id <= 0 || string.IsNullOrEmpty(cityData.Name) || cityData.Population <= 0)
                {
                    return BadRequest("Некорректные данные для обновления");
                }

                var existingCity = await _supabaseClient.From<City>()
                    .Where(x => x.Id == cityData.Id)
                    .Single();

                if (existingCity == null)
                {
                    return NotFound("Город не найден");
                }

                existingCity.Name = cityData.Name;
                existingCity.Population = cityData.Population;
                await existingCity.Update<City>();

                return Ok("Данные города успешно обновлены");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка сервера: {ex.Message}");
            }
        }

        [HttpDelete("DeleteCity", Name = "DeleteCity")]
        public async Task<ActionResult> DeleteCity(long id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Некорректный ID города");
                }

                bool result = await _supabaseContext.DeleteCity(_supabaseClient, id);

                if (!result)
                {
                    return NotFound("Город не найден или ошибка при удалении");
                }

                return Ok("Город успешно удален");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка сервера: {ex.Message}");
            }
        }
    }

    public class UserData
    {
        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("age")]
        public int? Age { get; set; }
    }

    public class UserUpdateData
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("age")]
        public int? Age { get; set; }
    }

    public class CityData
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("population")]
        public int Population { get; set; }
    }

    public class CityUpdateData
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("population")]
        public int Population { get; set; }
    }
}