namespace lab1
{
    public class Program//класс для программы
    {
        public static void Main(string[] args)//главная функция
        {
            var builder = WebApplication.CreateBuilder(args);//управляет конфигурацией и зависимостями приложения
            builder.Services.AddHttpLogging(o => { });//настроить параметры логирования
            var app = builder.Build();//Строим приложение


            app.MapGet("/", () => "Мое первое ASPA");
            //определяем маршрут для запроса
            //при запросе на этот адрес будет возвращена строка "Мое первое ASPA".

            app.UseHttpLogging();//хттп логирование для регистрации входящих и исходящих хттп запросов

            app.Run();//хапускаем приложение
        }
    }
}