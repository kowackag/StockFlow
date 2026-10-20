# StockFlow — dziennik sesji

Postępy z kolejnych sesji nauki. Najnowsza sesja na górze.
Plan i zasady współpracy: [`HARMONOGRAM.md`](HARMONOGRAM.md).

---

## ▶ Następnym razem zaczynamy od

**Etap 2, krok 2.4 — w trakcie (zadanie wydane 2026-10-20):** lista produktów w przeglądarce.
- `ProductController` → `ProductsController` (plik + klasa); `IProductService` przez konstruktor (`private readonly`);
- akcja `Index` asynchroniczna: `await` na serwisie, lista przekazana do widoku jako model;
- widok `Views/Products/Index.cshtml`: `@model` z listą `ProductDto`, `@using` (lub `_ViewImports.cshtml`),
  tabela Product / Status / Price (bez Inventory — etap 8), cena w zł, opcjonalnie badge dla Published/Hidden;
- link „Products” w menu `_Layout.cshtml` (tag helpery `asp-controller` / `asp-action`);
- pytania: dlaczego kontroler zależy od `IProductService` (a nie od repozytorium/DbContext); gdzie MVC szuka widoku
  i czemu nazwa folderu ma znaczenie; co daje `async Task<IActionResult>` i co się stanie bez `await`.

Po 2.4: odhaczyć etap 2 w HARMONOGRAM, uzupełnić README „Uruchomienie”.

Stan 2.4 (debug „nic nie wyświetla się na /Products”): konstruktor + `async Task<IActionResult> Index` + `View(products)` ✅.
Przyczyny: (1) klasa nadal `ProductController` → URL `/Product`, a `/Products` = 404; przy `/Product` MVC szuka
`Views/Product/` (folder jest `Products`) → „view not found”; (2) widok bez `@model` i tabeli — tylko nagłówek;
(3) dopisana druga trasa `products` w `Program.cs` — zbędna (domyślna `{controller}/{action}/{id?}` obsługuje każdy
kontroler; pierwsza pasująca trasa wygrywa) → usunąć. Link w `_Layout` jeszcze nie dodany.
Dalej: rename na `ProductsController` ✅ (Git: D + ??, po `git add` pokaże rename). `@model` najpierw z nieistniejącym
`ProductViewModel`, potem `IEnumerable<Product>` (encja z Domain) → kompiluje się, ale w runtime
`InvalidOperationException` (kontroler przekazuje listę `ProductDto`) — wskazane: `ProductDto` + `@using
StockFlow.Application.DTOs` w `_ViewImports`. Link w `_Layout`: brak `asp-action` → tag helper bierze bieżącą akcję
(z Privacy → `/products/Privacy` = 404) → dodać `asp-action="Index"`, `Products` wielką literą. Trasa w `Program.cs` wciąż do usunięcia.
Użytkowniczka zniechęcona („chyba się do tego nie nadaję”) — przypomniane realne postępy; tempo/zakres kroków do
pilnowania (mniejsze porcje, więcej konkretu przy nowych tematach jak Razor).

---

## Sesja 4 — 2026-10-19 / 2026-10-20

**Etap 2 — krok 2.3 ukończony ✅** (commity `641c525` feat, `d3ea7e3` docs)

Zrobione:
- `Application/DTOs/ProductDto` — `init`, `required` dla `Name` i `Status`, `Status` jako `string` (DTO nie wystawia enuma z Domain).
- `Application/Interfaces/IProductRepository` (`Task<IReadOnlyList<Product>> GetAllAsync()`) i `IProductService`
  (`Task<IReadOnlyList<ProductDto>> GetAllAsync()`).
- `Application/Services/ProductService` — `IProductRepository` przez konstruktor, `async`/`await`, mapowanie `Select` + `ToList()`.
- `Infrastructure/Repositories/ProductRepository` — `StockFlowDbContext` przez konstruktor, `AsNoTracking()` + `ToListAsync()`.
- `Program.cs` — 2× `AddScoped` (interfejs → implementacja) przed `Build()`.
- Build 0/0; Application bez EF/Infrastructure. Do commita weszła też pusta `ProductController` (zalążek 2.4).

Lekcje z review:
- Nazewnictwo: plik = nazwa typu; akronimy 3+ litery PascalCase (`ProductDto`); bez powtarzania „Product” w metodach
  `IProductRepository`; sufiks `Async` przy metodach zwracających `Task`.
- Ostrzeżenia nullable traktować poważnie (CS8618 przy `Status` → `required`).
- YAGNI — tylko metody potrzebne w tym kroku (Delete/GetById w etapie 3); bez zakomentowanego kodu w commitach.
- Dostęp do bazy asynchronicznie; „async all the way”; nigdy `async void` (dla „nic nie zwraca” → `Task`);
  przyszłe `Delete` → `Task DeleteAsync` z `FindAsync`/`SaveChangesAsync`; `GetById` → `Product?`.
- `AsNoTracking()` dla odczytu — opcja zapytania, przed jego wykonaniem.
- **DI** (Dependency Injection): zależności przez konstruktor zamiast `new`; kontener (`builder.Services`) = rejestr
  „interfejs → klasa”, sam składa łańcuch Controller → Service → Repository → DbContext; kolejność `Add...` dowolna
  (byle przed `Build()`); czasy życia Transient / Scoped / Singleton.
- Błąd MSB3027 „plik zablokowany przez StockFlow.Web” = aplikacja uruchomiona → Shift+F5 i build ponownie.
- Komendy wpisane w czacie się nie wykonują — terminal, Git Changes w VS albo prefiks `!` w tym oknie.

Pytania kontrolne 2.3:
1. DTO — „nie wszystkie dane potrzebne, bezpieczniej, szybciej” → bezpieczeństwo ✅, „szybciej” ❌ (encja i tak
   pobierana w całości). Dopowiedziane: oddzielenie widoku od kształtu encji, dane przygotowane pod ekran.
2. „Nie wiem” → wyjaśnione: Dependency Inversion — Application definiuje kontrakt, Infrastructure go spełnia;
   odwrotnie = Application zależne od EF + cykl referencji.
3. Scoped — „inny użytkownik może zmieniać dane” → kierunek nietrafiony. Właściwie: `DbContext` jest Scoped i nie jest
   bezpieczny wielowątkowo; obiekt nie może żyć dłużej niż jego zależności (captive dependency — Development rzuca
   wyjątek przy starcie); jeden `DbContext` na żądanie = wspólny zapis (etapy 10–11).

**Do powtórki:** Dependency Inversion, czasy życia DI i captive dependency.

---

## Sesja 3 — 2026-10-18 (po przerwie na kurs)

- Start kroku 2.3 — zadanie wydane.

---

## Sesja 2 — 2026-10-18

**Etap 2 — kroki 2.1 i 2.2 ukończone ✅** (commity `500f1f1`, `4f6028a`, `f3d483a`)

Decyzje:
- **Cena — model C:** cena bazowa na `Product`, w etapie 6 opcjonalna `decimal? Price` na wariancie.
  Waluta: PLN (tylko wyświetlanie). Pieniądze zawsze jako `decimal` (omówione: decimal vs double, dlaczego nie grosze ×100).
- Pozostałe klasy domeny (warianty, Brand, Supplier, Address) **nie teraz** — kodowane w swoich etapach.
  Na start etapu 5 projektujemy razem model domeny w `MODEL-DOMENY.md`. Do harmonogramu dopisane:
  `Brand` (etap 5), `Supplier` opcjonalnie (etap 9), `Address` jako value object (etap 11).
- Baza: zostajemy na **LocalDB**. Na komputerze jest też instancja SQL Server `SUPERTACHO` (zainstalowana przez inny
  program) — nie ruszamy jej. SSMS 20 można używać do podglądu: Server name `(localdb)\MSSQLLocalDB`, Windows Authentication.

Krok 2.1 — Domain:
- `Model/Products/Product.cs` (Id, Name = "", Description `string?` — własny dodatek, Price `decimal`, Status)
  + `PublicationStatus.cs` (enum: Published, Hidden).
- Lekcje z review: folder ≠ nazwa klasy (folder `Product` kolidował z klasą → CS0101; foldery w l. mnogiej),
  namespace = ścieżka folderów, `public` zamiast domyślnego `internal`, jeden typ na plik, enum to osobny rodzaj typu.

Krok 2.2 — EF Core + baza:
- `StockFlowDbContext` (konfiguracja w `OnModelCreating`), `HasData` z 6 produktami ze screena, Status zapisywany tekstem
  (`nvarchar(20)`), connection string `DefaultConnection` (LocalDB), `AddDbContext` w `Program.cs`, pakiet Tools w Web.
- Migracje: `InitialCreate` + `SeedRealProductsAndStatusLength` (użyty `Remove-Migration` do poprawki przed wykonaniem).
- Lekcje z review: kolejność w `Program.cs` (rejestracja usług → `Build()` → pipeline → `Run()`; rejestracja po `Run()`
  nigdy się nie wykona); czytać wygenerowaną migrację przed `Update-Database`; ostrzeżenie „may result in the loss of data”
  przy zwężaniu kolumny.
- Pytania: decimal vs double ✅; Tools w Web — wyjaśnione (narzędzia EF uruchamiają startup project); Fluent API zamiast
  atrybutów ✅ (niezależność Domain). **Do powtórki:** startup project vs default project.

Sprostowanie: referencje projektów **są** przechodnie (Web widzi Domain przez Application);
Web → Infrastructure była potrzebna, bo Infrastructure nie leży w łańcuchu Web → Application → Domain.

**Otwarte sprawy:**
- `OnModelCreating` vs osobna `ProductConfiguration` — wrócić w etapie 5 (gdy dojdą kolejne encje).
- Linia `entity.Property(e => e.Description);` nic nie robi — usunąć albo dodać limit długości.
- Skąd pakiet `System.Security.Cryptography.Pkcs` w Infrastructure? (jeśli przypadkiem — usunąć).
- `required` zamiast `Name = ""` — wrócić w etapie 7.
- Gdzie trzymać stan produktu bez wariantów (propozycja: ukryty wariant domyślny) — przed etapem 6.

---

## Sesja 1 — 2026-10-16

**Zrobione:**
- Omówiony screen `stock-view.png` i zakres: MVP = Products, Collections, Inventory, Orders, Customers.
- Powstał `HARMONOGRAM.md`: etapy 0–16, kolejność (najpierw jeden prosty produkt przez wszystkie warstwy,
  potem rozbudowa), decyzje domenowe do omówienia.
- Spisane zasady współpracy (sekcja 2 harmonogramu).

**Stan kursu:** Moduł 7 — Bazy danych (w trakcie). Testowanie pominięte, Web API jeszcze nie.

**Etap 0 ukończony ✅**
- Zainstalowane .NET 10 SDK (10.0.402) i VS Community 2026 (18.11); VS 2022 zostało obok.
- Repozytorium: `git init`, `.gitignore` (z `.playwright-mcp/` i `stock-view.png`), README.
  `HARMONOGRAM.md` był początkowo ignorowany — usunięty z `.gitignore`, idzie do repozytorium.
- Lokalny `user.email` = prywatny mail (globalnie zostaje firmowy).
- Push nie działał (`Invalid username or token`) — usunięte stare poświadczenia GitHub z Menedżera poświadczeń Windows,
  logowanie przez przeglądarkę (Git Credential Manager). Commit `1970893` jest na `github.com/kowackag/StockFlow`.
- VS może poprosić o ponowne zalogowanie do GitHuba (jego zapisane poświadczenia też zostały usunięte).

**Etap 1 ukończony ✅** (commity `4e4e844`, `8c39839`)
- Solution i projekty utworzone w Visual Studio 2026 (zmiana — początkowo miał być terminal), projekty w `src/`.
- Język VS zmieniony na angielski. Nazwa `StockFlow.Web` zostaje (zamiast `.Presentation`) — Web to też composition root.
- `net10.0`, `Class1.cs` usunięte, build 0 błędów.
- Po review: dodana referencja Web → Infrastructure, usunięty folder `Interfaces` z Domain
  (interfejsy tylko w Application). Folder `Model` w Domain zostaje (decyzja własna).
- Pytania sprawdzające: 1 — dobry kierunek (Domain = rdzeń, zależności wskazują do środka); 2 i 3 — omówione
  (composition root w `Program.cs`; `Domain → Infrastructure` = cykl + uzależnienie reguł biznesowych od technologii).
