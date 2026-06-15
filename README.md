1. Sklonuj repozytorium:
git clone LINK_DO_REPO

2. Wejdź do folderu projektu:
cd MailSender

3. Przywróć paczki:
dotnet restore

4. Zbuduj projekt:
dotnet build

5. Ustaw lokalnie dane Brevo:
dotnet user-secrets set "Brevo:ApiKey" "KLUCZ_API"
dotnet user-secrets set "Brevo:SenderEmail" "ZWERYFIKOWANY_MAIL_NADAWCY"
dotnet user-secrets set "Brevo:SenderName" "MailSender App"

6. Uruchom projekt:
dotnet run

7. Wejdź w Swagger:
http://localhost:PORT/swagger

8. Najpierw wykonaj:
POST /client-app/register

Body:
{
  "appId": "TEST",
  "appName": "TESTapp",
  "pass": "q##wa25"
}

9. Skopiuj token z pola key.

10. Kliknij Authorize w Swaggerze i wklej token.

11. Wykonaj:
POST /mail/send

Body:
{
  "to": "adres-testowy@gmail.com",
  "subject": "Czy działa?",
  "body": "Test wiadomości Szynal"
}
