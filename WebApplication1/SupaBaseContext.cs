using Supabase.Gotrue;

namespace WebApplication1
{
    public class SupaBaseContext
    {

        public async Task<List<User>> GetUsers(Supabase.Client _supabaseClient)
        {
            var result = await _supabaseClient.From<User>().Get();
            return result.Models;
        }

        public async Task<bool> InsertUser(Supabase.Client _supabaseClient, User user)
        {
            try
            {
                await _supabaseClient.From<User>().Insert(user);
                return true;
            }
            catch 
            {
                return false;
            }
        }

        public async Task<bool> UpdateUser(Supabase.Client _supabaseClient, int id, string newLogin, string newPassword, int newAge)
        {
            try
            {
                var user = await _supabaseClient.From<User>()
                    .Where(x => x.Id == id)
                    .Single();

                if (user != null)
                {
                    user.Login = newLogin;
                    user.Password = newPassword;
                    user.Age = newAge;
                    await user.Update<User>();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка обновления пользователя: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteUser(Supabase.Client _supabaseClient, int id)
        {
            try
            {
                var user = await _supabaseClient.From<User>()
                    .Where(x => x.Id == id)
                    .Single();

                if (user != null)
                {
                    await user.Delete<User>();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка удаления пользователя: {ex.Message}");
                return false;
            }
        }

        public async Task<List<City>> GetCities(Supabase.Client _supabaseClient)
        {
            var result = await _supabaseClient.From<City>().Get();
            return result.Models;
        }

        public async Task<bool> InsertCity(Supabase.Client _supabaseClient, City city)
        {
            try
            {
                await _supabaseClient.From<City>().Insert(city);
                return true;
            }
            catch 
            {
                return false;
            }
        }

        public async Task<bool> UpdateCity(Supabase.Client _supabaseClient, int id, string name, int population)
        {
            try
            {
                var city = await _supabaseClient.From<City>()
                    .Where(x => x.Id == id)
                    .Single();

                if (city != null)
                {
                    city.Name = name;
                    city.Population = population;
                    await city.Update<City>();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обновлении города: {ex.Message}");
                return false;
            }
        }
        
        public async Task<bool> DeleteCity(Supabase.Client _supabaseClient, long id)
        {
            try
            {
                var city = await _supabaseClient.From<City>()
                    .Where(x => x.Id == id)
                    .Single();

                if (city != null)
                {
                    await city.Delete<City>();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при удалении города: {ex.Message}");
                return false;
            }
        }
    }
}