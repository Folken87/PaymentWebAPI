# PaymentWebAPI

Добро пожаловать в репозиторий PaymentWebAPI. 
Это репозиторий приложения, который был разработан во время работы над тестовым заданием.

*Все данные хранятся только во время запуска проекта*.

*Для того чтобы внести информацию об оплате необходимо:*

1. Отправить *POST* запрос в **/api/PaymentActions** в теле которого будут указаны назначение платежа и сумма, например:

    {
      "transaction_price":215,
      "appointment":"game_pass"
    }

В ответ сервер вернёт идентификатор сессии.

2. Надо отправить *POST* запрос в **/api/PaymentActions/details** в теле которого будут указаны номер карты, cvv-код и дата истечения срока карты, например:

    {
      "card_number":4000123456789010,
      "card_cvv": 000,
      "card_date":"10/21"
    }

В ответ сервер вернёт код успешно проведённой транзакции, либо ответ 400 - если номер карты невалидный, либо 401 - если истекло время пользовательской сессии.

*Пользовательская сессия будет сброшена, если пользователь в течение какого-либо времени не отправлял запросов. Время ожидания ответа пользователя можно средактировать в **Startup.cs** в этой части кода:*

    services.AddSession(options =>
    {
      options.Cookie.Name = ".My_firstWebAPI_App.Session";
      options.IdleTimeout = TimeSpan.FromSeconds(15); //время ожидания запроса пользователя
      options.Cookie.IsEssential = true;
      options.Cookie.HttpOnly = true;
    });
    
*К сожалению не успел внедрить сервис авторизации, поэтому история операций доступна всем пользователям.*
Чтобы получить данные о всех транзакциях необходимо отправить *GET* запрос по пути **/api/PaymentActions**
    
 
