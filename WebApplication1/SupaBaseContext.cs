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
    }
}