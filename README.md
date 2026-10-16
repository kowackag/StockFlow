# StockFlow

System do zarządzania magazynem produktów — ubrań, butów i dodatków — tworzony w **ASP.NET Core**.
Obejmuje katalog produktów z wariantami (rozmiar, kolor), stany magazynowe, kolekcje, klientów i zamówienia.

> 🚧 **Projekt w budowie.** Powstaje etapami, równolegle z nauką .NET — aktualny postęp w [HARMONOGRAM.md](HARMONOGRAM.md).

## Technologie

- .NET 10, C#
- ASP.NET Core MVC (panel administracyjny), później ASP.NET Core Web API
- Entity Framework Core + SQL Server
- Bootstrap

## Architektura

Rozwiązanie jest podzielone na cztery projekty zgodnie z zasadami Clean Architecture:

| Projekt | Odpowiedzialność |
|---|---|
| `StockFlow.Domain` | Encje i reguły biznesowe — bez zależności od innych projektów |
| `StockFlow.Application` | Przypadki użycia, interfejsy, DTO |
| `StockFlow.Infrastructure` | Dostęp do danych (EF Core, SQL Server), implementacje interfejsów |
| `StockFlow.Web` | Interfejs użytkownika (MVC) i w przyszłości API |

## Planowane funkcje

- **Produkty** — lista z wyszukiwaniem, filtrowaniem, sortowaniem i paginacją; warianty i atrybuty; zdjęcia; status publikacji
- **Kategorie i kolekcje**
- **Magazyn** — stany per wariant, historia ruchów magazynowych
- **Klienci**
- **Zamówienia** — statusy, rezerwacja towaru
- **REST API**
- **Logowanie i role użytkowników**

## Uruchomienie

Instrukcja pojawi się, gdy w projekcie będzie pierwsza działająca wersja aplikacji.
