namespace PetPortalApplication.Helpers;

public static class EmailTemplates
{
    public static string ConfirmRegistration(string userName, string confirmLink) =>
        Wrap(
            "Подтверждение регистрации — PetPortal",
            $"""
             <p>Здравствуйте, {Escape(userName)}!</p>
             <p>Спасибо за регистрацию на PetPortal. Чтобы завершить создание аккаунта, подтвердите вашу почту:</p>
             <p><a href="{confirmLink}" style="display:inline-block;padding:12px 24px;background:#0095FF;color:#fff;text-decoration:none;border-radius:6px;">Подтвердить email</a></p>
             <p>Если кнопка не работает, скопируйте ссылку в браузер:</p>
             <p><a href="{confirmLink}">{confirmLink}</a></p>
             <p>Ссылка действительна 24 часа.</p>
             """);

    public static string NewRespond(string ownerName, string responderName, string projectName, string comment, string link) =>
        Wrap(
            "Новый отклик на ваш проект — PetPortal",
            $"""
             <p>Здравствуйте, {Escape(ownerName)}!</p>
             <p><strong>{Escape(responderName)}</strong> откликнулся на ваш проект «{Escape(projectName)}».</p>
             {(string.IsNullOrWhiteSpace(comment) ? "" : $"<p>Комментарий: {Escape(comment)}</p>")}
             <p><a href="{link}" style="display:inline-block;padding:12px 24px;background:#0095FF;color:#fff;text-decoration:none;border-radius:6px;">Посмотреть отклики</a></p>
             """);

    public static string UnreadChatMessage(string recipientName, string senderName, string preview, string link) =>
        Wrap(
            "Непрочитанное сообщение — PetPortal",
            $"""
             <p>Здравствуйте, {Escape(recipientName)}!</p>
             <p>У вас есть непрочитанное сообщение от <strong>{Escape(senderName)}</strong> (более 2 часов):</p>
             <blockquote style="border-left:3px solid #0095FF;padding-left:12px;color:#333;">{Escape(preview)}</blockquote>
             <p><a href="{link}" style="display:inline-block;padding:12px 24px;background:#0095FF;color:#fff;text-decoration:none;border-radius:6px;">Открыть чат</a></p>
             """);

    private static string Wrap(string title, string body) =>
        $"""
         <!DOCTYPE html>
         <html><head><meta charset="utf-8"><title>{Escape(title)}</title></head>
         <body style="font-family:Inter,Arial,sans-serif;color:#222;line-height:1.5;max-width:560px;margin:0 auto;padding:24px;">
         <h2 style="color:#0095FF;">PetPortal</h2>
         {body}
         <hr style="border:none;border-top:1px solid #eee;margin:24px 0;">
         <p style="font-size:12px;color:#888;">Это автоматическое письмо, отвечать на него не нужно.</p>
         </body></html>
         """;

    private static string Escape(string value) =>
        System.Net.WebUtility.HtmlEncode(value);
}
