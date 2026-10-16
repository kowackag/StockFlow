# StockFlow — harmonogram nauki i budowy aplikacji

System do zarządzania magazynem produktów (ubrania, buty, dodatki) budowany równolegle z nauką ASP.NET Core.
Wzór wyglądu i funkcji: `stock-view.png` (panel „Products” z katalogiem, stanami magazynowymi i zamówieniami).

Plan jest dopasowany do postępów w kursie **„Zostań programistą ASP.NET” (Kajetan Duszyński)**.

---

## 1. Gdzie jestem i jak plan łączy się z kursem

| Temat w kursie | Status | Co to oznacza dla projektu |
|---|---|---|
| Moduł 7 — Bazy danych | **w trakcie** | EF Core i SQL Server wchodzą do projektu od razu (etap 2) |
| Testowanie | pominięte, powrót później | Testy to osobny etap — „wpinamy” go, kiedy wrócisz do tego modułu |
| Web API | jeszcze nie | Etap API zaczynamy dopiero po przerobieniu tego modułu |

Zasada: **projekt nie wyprzedza kursu o więcej niż jeden krok.** Jeśli etap wymaga czegoś, czego jeszcze nie było
w kursie, dostajesz krótkie wyjaśnienie „na teraz”, a pełne zrozumienie przyjdzie z kursem.

---

## 2. Zasady współpracy

Te zasady obowiązują przez cały czas nauki i przy pracy nad dokumentacją.

1. **Nie piszę kodu za Ciebie.** Cały kod projektu piszesz samodzielnie.
2. **Sprawdzam to, co napiszesz.** Jeśli mam uwagi, mówię *dlaczego* coś jest źle i *co bym zmienił* —
   ale poprawkę wprowadzasz Ty. Kiedy utkniesz, dostajesz wskazówkę, wyjaśnienie albo link do dokumentacji, a nie gotowe rozwiązanie.
3. **Etapy odhaczamy razem** — po zakończeniu etapu zaznaczamy go w sekcji „Postęp” poniżej.
4. **Wszystkie commity robisz sama.** Ja nie wykonuję żadnych operacji w Git (commit, push itp.).
5. **Zapisuję postępy z każdej sesji** w pliku [`DZIENNIK.md`](DZIENNIK.md): co zrobiliśmy, na czym skończyliśmy,
   jakie uwagi zostały otwarte i od czego zaczynamy następnym razem — żeby kolejnego dnia sprawnie ruszyć dalej.

Jak wygląda praca nad etapem:

1. Dostajesz zadanie z opisem, *czego się uczysz* i *kiedy jest gotowe*.
2. Piszesz kod.
3. Robię code review: czy działa, czy pasuje do architektury, co i dlaczego poprawić.
4. Poprawiasz, aż etap spełnia warunek „gotowe, gdy”.
5. Robisz commit, odhaczamy etap, zapisuję postęp w dzienniku.

---

## 3. Kluczowe decyzje dotyczące kolejności i podejścia

1. **Pionowe wycinki zamiast budowania warstwa po warstwie.**
   Budowanie najpierw całej domeny, potem Application, potem Infrastructure i dopiero na końcu MVC oznacza
   wiele tygodni bez niczego widocznego w przeglądarce. Zamiast tego najpierw przeprowadzamy
   *jeden prosty* `Product` przez wszystkie cztery projekty (encja → baza → repozytorium → strona z listą),
   a potem rozbudowujemy. Szybciej widać efekt i od razu rozumiesz, jak warstwy ze sobą współpracują.

2. **Product najpierw prosty, warianty później.**
   `ProductVariant` i `VariantAttribute` dochodzą w etapie 6 i trafią do `StockFlow.Domain/Entities/`.

3. **Kategorie przed wariantami.** Relacja Category → Product (jeden-do-wielu) jest prostsza
   i dobrze ćwiczy temat z modułu 7, zanim dojdzie Product → Variant → Attribute.

4. **Testy nie na samym końcu.** Etap testów można zacząć w dowolnym momencie po etapie 7
   (domena z regułami biznesowymi świetnie nadaje się do testów jednostkowych). Robisz go, kiedy wrócisz do modułu testowania.

5. **Git i GitHub od pierwszego dnia** — projekt ma iść do portfolio, więc historia commitów też się liczy.

6. **.NET 10 (LTS, wsparcie do 2028).** SDK 8 i 9 tracą wsparcie **10.11.2026**.
   W .NET 10 `dotnet new sln` tworzy plik **`StockFlow.slnx`** (nowy format XML) zamiast `.sln` —
   Visual Studio 2026 i Rider go obsługują. Stary format: `dotnet new sln --format sln`.

7. **Konfiguracja EF Core w Infrastructure (Fluent API)**, nie atrybutami w encjach — dzięki temu Domain
   naprawdę „nie wie”, że istnieje baza danych.

8. **Osobny etap refaktoryzacji domeny** (enkapsulacja: prywatne settery, konstruktory, metody biznesowe).
   Najpierw piszesz prosto (`{ get; set; }`), potem przerabiasz — to najlepszy sposób, żeby zrozumieć, *po co* jest enkapsulacja.

9. **API nie wymaga drugiego projektu:** kontrolery MVC (`ProductsController`)
   i kontrolery API (`Api/ProductsApiController`) siedzą w tym samym `StockFlow.Web` i korzystają z tych samych serwisów z Application.

---

## 4. Zakres — co ze screena robimy, a co później

| Element ze screena | Zakres |
|---|---|
| Products: lista, wyszukiwarka, filtr Published/Hidden, sortowanie, paginacja, zdjęcie, Add Product | **MVP** |
| Catalog → Collections | **MVP** |
| Catalog → Inventory | **MVP** |
| Catalog → Orders | **MVP** |
| Customers | **MVP** |
| Home (dashboard ze statystykami) | później |
| Logowanie / użytkownik (Jenny Wilson, Log out) | później (Identity) |
| Finances, Marketing, Analytics | opcjonalnie |
| Integrations, Sales Channels (Online Store, Sell Via Link) | opcjonalnie / poza zakresem |
| Przełącznik sklepów („Mob Shop”) | poza zakresem (jeden sklep) |

---

## 5. Architektura (przypomnienie)

```
StockFlow/
├── StockFlow.slnx
├── src/
│   ├── StockFlow.Domain/          ← encje, enumy, reguły biznesowe (zero zależności)
│   ├── StockFlow.Application/     ← interfejsy, serwisy (przypadki użycia), DTO
│   ├── StockFlow.Infrastructure/  ← EF Core, DbContext, migracje, repozytoria
│   └── StockFlow.Web/             ← MVC (kontrolery, widoki, ViewModels), później API
└── tests/                         ← dojdzie w etapie testów
```

Zależności: `Application → Domain`, `Infrastructure → Application + Domain`, `Web → Application + Infrastructure`.
Domain nie zależy od niczego.

Świadomie **nie** używamy na razie: CQRS, MediatR, AutoMappera. Dojdziemy do nich (albo nie) wtedy, gdy będzie konkretny powód.

---

## 6. Decyzje domenowe do omówienia

Nie są jeszcze ustalone — omówimy je, zanim dojdziemy do danego etapu:

- **Cena na produkcie czy na wariancie?** Na screenie jest jedna cena na produkcie. Propozycja: na start cena na `Product`,
  w etapie 6 decydujemy, czy wariant może mieć własną cenę (np. większy rozmiar droższy).
- **Stan magazynowy per wariant (SKU), nie per produkt.** Kolumna „100 stock for 1 variants” to suma stanów wariantów.
- **Czy każdy produkt ma co najmniej jeden wariant?** Propozycja: tak (wariant domyślny) — to upraszcza magazyn i zamówienia.
- **Atrybuty ogólne (`Name` / `Value`) czy konkretne pola `Size` / `Color`?** Ogólne są elastyczniejsze
  (buty: rozmiar EU, dodatki: często brak rozmiaru).
- **Waluta:** PLN czy $ jak na screenie?
- **Jeden magazyn czy kilka?** Propozycja: jeden.

---

## 7. Etapy

Każdy etap: **czego się uczysz** · **co robisz** · **gotowe, gdy** · **związek z kursem**.

### Etap 0 — Środowisko i repozytorium
- **Uczysz się:** pracy z Git od początku projektu, wersji .NET.
- **Robisz:** instalujesz .NET 10 SDK i **Visual Studio 2026 Community** (VS 2022 17.14 nie pozwala celować w `net10.0`;
  obie wersje VS mogą być zainstalowane obok siebie); `git init`, `dotnet new gitignore`, dopisujesz `.playwright-mcp/` do `.gitignore`;
  zakładasz repozytorium na GitHubie; pierwszy commit (README + ten harmonogram).
- **Gotowe, gdy:** `dotnet --list-sdks` pokazuje 10.x, VS 2026 się uruchamia, a repozytorium jest na GitHubie.
- **Kurs:** —

### Etap 1 — Szkielet solution (4 projekty)
- **Uczysz się:** czym jest solution i projekt, jak działają referencje między projektami, czym folder rozwiązania różni się od folderu na dysku.
- **Robisz (w Visual Studio 2026):** pusta solucja, 3× Class Library, 1× ASP.NET Core Web App (MVC) — wszystkie w folderze `src/`,
  referencje między projektami. Usuwasz domyślne `Class1.cs`.
- **Gotowe, gdy:** `dotnet build` przechodzi bez błędów, a `dotnet run --project src/StockFlow.Web` pokazuje domyślną stronę MVC.
- **Kurs:** podstawy ASP.NET Core MVC.

### Etap 2 — Pierwszy pionowy wycinek: lista produktów z bazy ⭐
- **Uczysz się:** EF Core w praktyce, DbContext, migracje, seed danych, wstrzykiwanie zależności (DI), jak warstwy się łączą.
- **Robisz:**
  - Domain: `Product` (Id, Name, Price, Status) + enum `PublicationStatus` (Published, Hidden);
  - Infrastructure: `StockFlowDbContext`, konfiguracja `Product` przez Fluent API, connection string do SQL Server (LocalDB),
    pierwsza migracja, kilka produktów testowych (seed);
  - Application: `IProductRepository`, `ProductService` (metoda „pobierz listę”), `ProductDto`;
  - Infrastructure: `ProductRepository`; rejestracja w DI;
  - Web: `ProductsController.Index` + widok z tabelą.
- **Gotowe, gdy:** strona `/Products` pokazuje produkty z bazy SQL Server.
- **Kurs:** **Moduł 7 — Bazy danych** (to, czego teraz się uczysz).

### Etap 3 — CRUD produktu
- **Uczysz się:** formularze, ViewModel vs DTO vs encja, walidacja, wzorzec Post-Redirect-Get, `TempData`.
- **Robisz:** dodawanie, edycja, usuwanie produktu; przycisk „Add Product”; komunikaty o sukcesie.
- **Gotowe, gdy:** da się dodać, edytować i usunąć produkt, a błędne dane pokazują komunikaty walidacji.
- **Kurs:** formularze i walidacja w MVC.

### Etap 4 — Lista jak na screenie
- **Uczysz się:** LINQ i `IQueryable`, filtrowanie i sortowanie po stronie bazy, paginacja (`Skip`/`Take`), layout i partial views.
- **Robisz:** wyszukiwarka, filtr Published/Hidden („Show All Products”), sortowanie A–Z, paginacja;
  layout z bocznym menu jak na screenie (Bootstrap + własny CSS, bez przesadnego dopieszczania).
- **Gotowe, gdy:** lista zachowuje się jak na screenie (wyszukiwanie, filtr, sortowanie i strony działają razem).
- **Kurs:** LINQ, EF Core (zapytania).

### Etap 5 — Kategorie (relacja jeden-do-wielu)
- **Uczysz się:** relacja 1:N w EF Core, klucz obcy, `Include`, listy rozwijane w formularzu.
- **Robisz:** `Category` (Ubrania, Buty, Dodatki), przypisanie produktu do kategorii, filtr po kategorii.
- **Gotowe, gdy:** produkt ma kategorię, a migracja poprawnie tworzy relację w bazie.
- **Kurs:** Moduł 7 — relacje.

### Etap 6 — Warianty i atrybuty
- **Uczysz się:** zagnieżdżone relacje (Product → Variant → Attribute), unikalny indeks (SKU), formularze z kolekcjami.
- **Robisz:** **`VariantAttribute`** (Id, Name, Value), `ProductVariant` (SKU, atrybuty, ewentualnie cena),
  zarządzanie wariantami na stronie produktu.
- **Gotowe, gdy:** koszulka Adidas ma warianty „Biały M”, „Biały L”, „Czarny M”, a SKU nie mogą się powtarzać.
- **Kurs:** Moduł 7 — relacje.

### Etap 7 — Refaktoryzacja domeny (enkapsulacja)
- **Uczysz się:** dlaczego encja nie powinna mieć wszystkiego na `public set`, reguły biznesowe w domenie, prywatne konstruktory dla EF.
- **Robisz:** prywatne settery, konstruktory z walidacją, metody `Publish()`, `Hide()`, `AddVariant()`, `ChangePrice()`;
  kod w Application i Web korzysta z tych metod.
- **Gotowe, gdy:** nie da się utworzyć produktu z pustą nazwą ani ujemną ceną — pilnuje tego sama domena.
- **Kurs:** programowanie obiektowe (powtórka w praktyce).

### Etap 8 — Zdjęcia produktów
- **Uczysz się:** upload plików (`IFormFile`), abstrakcja na pliki (interfejs w Application, implementacja w Infrastructure).
- **Robisz:** `IFileStorage`, zapis do `wwwroot/uploads`, miniatura na liście.
- **Gotowe, gdy:** lista produktów wygląda jak na screenie — ze zdjęciami.
- **Kurs:** —

### Etap 9 — Inventory (stany magazynowe)
- **Uczysz się:** modelowanie ruchów magazynowych, transakcje, reguły typu „stan nie może spaść poniżej zera”.
- **Robisz:** stan per wariant, `StockMovement` (przyjęcie, wydanie, korekta), strona Inventory, kolumna „X stock for Y variants”.
- **Gotowe, gdy:** przyjęcie towaru zwiększa stan, wydanie go zmniejsza, a historia ruchów jest widoczna.
- **Kurs:** Moduł 7 — transakcje (jeśli są w kursie).

### Etap 10 — Collections (relacja wiele-do-wielu)
- **Uczysz się:** relacja N:M w EF Core.
- **Robisz:** kolekcje (np. „Lato 2026”, „Sneakersy”), przypisywanie produktów.
- **Gotowe, gdy:** produkt może być w kilku kolekcjach, a kolekcja zawiera wiele produktów.
- **Kurs:** Moduł 7 — relacje.

### Etap 11 — Customers
- **Uczysz się:** utrwalenie CRUD, value objects (np. adres).
- **Robisz:** lista klientów, dodawanie i edycja, adres dostawy.
- **Gotowe, gdy:** klientów można wybrać przy zamówieniu.
- **Kurs:** —

### Etap 12 — Orders (zamówienia)
- **Uczysz się:** agregaty (Order + OrderLine), statusy i przejścia między nimi, rezerwacja stanu, współbieżność (`RowVersion`).
- **Robisz:** tworzenie zamówienia, statusy (Nowe → Opłacone → Wysłane / Anulowane), rezerwacja przy złożeniu,
  zdjęcie ze stanu przy wysyłce, zwolnienie rezerwacji przy anulowaniu.
- **Gotowe, gdy:** nie da się zamówić więcej, niż jest dostępne, a anulowanie przywraca stan.
- **Kurs:** —

### Etap 13 — Web API ⏸ *(po module Web API w kursie)*
- **Uczysz się:** REST, kody HTTP, DTO w API, dokumentacja OpenAPI, ponowne użycie tych samych serwisów co w MVC.
- **Robisz:** `Controllers/Api/ProductsApiController` (lista, szczegóły, dodawanie, edycja), potem stany magazynowe i zamówienia.
- **Gotowe, gdy:** endpointy działają i są widoczne w dokumentacji OpenAPI, a logika nie jest skopiowana z kontrolerów MVC.
- **Kurs:** moduł Web API.

### Etap 14 — Testy ⏸ *(kiedy wrócisz do modułu testowania — możliwe w dowolnym momencie po etapie 7)*
- **Uczysz się:** testy jednostkowe (xUnit), testowanie reguł domeny i serwisów.
- **Robisz:** projekty `tests/StockFlow.Domain.Tests` i `tests/StockFlow.Application.Tests`; testy reguł produktu, stanów i zamówień.
- **Gotowe, gdy:** `dotnet test` przechodzi, a najważniejsze reguły biznesowe mają testy.
- **Kurs:** moduł testowania.

### Etap 15 — Logowanie i role
- **Uczysz się:** ASP.NET Core Identity, autoryzacja, role (Admin, Pracownik magazynu).
- **Robisz:** logowanie, wylogowanie, ograniczenie dostępu do stron, użytkownik w menu jak na screenie.
- **Gotowe, gdy:** bez logowania nie da się wejść do panelu.
- **Kurs:** Identity / bezpieczeństwo (jeśli jest w kursie).

### Etap 16 — Opcjonalnie / portfolio
- Dashboard (Home) ze statystykami, raporty sprzedaży (Finances / Analytics).
- Wdrożenie (np. Azure), Docker, GitHub Actions (automatyczny build i testy).
- README z opisem architektury i zrzutami ekranu.

---

## 8. Postęp

- [x] Etap 0 — Środowisko i repozytorium *(2026-10-16)*
- [ ] Etap 1 — Szkielet solution
- [ ] Etap 2 — Pierwszy wycinek: lista produktów z bazy
- [ ] Etap 3 — CRUD produktu
- [ ] Etap 4 — Lista jak na screenie
- [ ] Etap 5 — Kategorie
- [ ] Etap 6 — Warianty i atrybuty
- [ ] Etap 7 — Refaktoryzacja domeny
- [ ] Etap 8 — Zdjęcia produktów
- [ ] Etap 9 — Inventory
- [ ] Etap 10 — Collections
- [ ] Etap 11 — Customers
- [ ] Etap 12 — Orders
- [ ] Etap 13 — Web API ⏸
- [ ] Etap 14 — Testy ⏸
- [ ] Etap 15 — Logowanie i role
- [ ] Etap 16 — Opcjonalnie / portfolio
