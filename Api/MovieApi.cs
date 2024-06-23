using FilmViewer.Models;

namespace FilmViewer.Api
{
    public class MovieApi
    {
        public static async Task<Movies> GetPopularMovies(int page = 1)
        {
            return await SendApi<Movies>($"https://api.themoviedb.org/3/movie/popular?language=ru-ru&page={page}&include_adult=true");
        }
        public static async Task<Movies> GetSimilarMovies(int movieId, int page = 1)
        {
            return await SendApi<Movies>($"https://api.themoviedb.org/3/movie/{movieId}/similar?&language=ru-ru&page={page}&include_adult=true");
        }
        public static async Task<Movies> GetMoviesByYearAndName(string name, int year, int page = 1)
        {
            return await SendApi<Movies>($"https://api.themoviedb.org/3/search/movie?language=ru-ru&query={name}&page={page}&include_adult=false&year={year}");
        }
        public static async Task<Movies> GetMoviesByName(string name, int page = 1)
        {
            return await SendApi<Movies>($"https://api.themoviedb.org/3/search/movie?language=ru-ru&query={name}&page={page}&include_adult=true");
        }
        public static async Task<Movies> GetMoviesByGenre(int genreId, int page = 1)
        {
            return await SendApi<Movies>($"https://api.themoviedb.org/3/discover/movie?with_genres={genreId}&page={page}&include_adult=true?language=ru-ru");
        }
        public static async Task<Movie> GetMovieById(int id)
        {
            return await SendApi<Movie>($"https://api.themoviedb.org/3/movie/{id}?language=ru-ru");
        }
        public static async Task<Genres> GetGenreList()
        {
            return await SendApi<Genres>("https://api.themoviedb.org/3/genre/movie/list?language=ru-ru");
        }
        private static async Task<T> SendApi<T>(string query)
        {
            string apiKey = GetMovieApiKey();
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage responseMessage = await httpClient.GetAsync(query + $"&api_key={apiKey}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var result = await responseMessage.Content.ReadFromJsonAsync<T>();
                return result;
            }
            else
            {
                throw new Exception("Error. Try again later.");
            }
        }
        private static string GetMovieApiKey()
        {
            var builder = new ConfigurationBuilder();
            // установка пути к текущему каталогу
            builder.SetBasePath(Directory.GetCurrentDirectory());
            // получаем конфигурацию из файла appsettings.json
            builder.AddJsonFile("appsettings.json");
            // создаем конфигурацию
            var config = builder.Build();
            // получаем строку подключения
            var connectionString = config.GetSection("MovieApi:ApiKey");
            return connectionString.Value;
        }
    }
}
