# StockFlow — dziennik sesji

Postępy z kolejnych sesji nauki. Najnowsza sesja na górze.
Plan i zasady współpracy: [`HARMONOGRAM.md`](HARMONOGRAM.md).

---

## ▶ Następnym razem zaczynamy od

**Etap 2, krok 2.3 — w trakcie (zadanie wydane w sesji 3):**
- Application: `ProductDto` (DTOs; ewentualnie `record`), `IProductRepository` (`GetAllAsync`, encje, `IReadOnlyList`?),
  `IProductService` + `ProductService` (repozytorium przez konstruktor, mapowanie ręczne przez `Select`);
- Infrastructure: `ProductRepository` (`StockFlowDbContext` przez konstruktor, `AsNoTracking()` + `ToListAsync()`);
- `Program.cs`: `AddScoped` dla repozytorium i serwisu;
- pytania: DTO zamiast encji; interfejs w Application / implementacja w Infrastructure; Scoped vs Singleton.
- Application bez referencji do EF Core / Infrastructure.

Potem krok 2.4: `ProductsController` + widok listy → produkty z bazy widoczne w przeglądarce (koniec etapu 2).

---

## Sesja 3 — 2026-10-18 (po przerwie na kurs)

- Start kroku 2.3 — zadanie wydane (szczegóły w sekcji „Następnym razem” powyżej).

## Sesja 4 — 2026-10-19

- Review punktu 1 kroku 2.3 (`Application/DTOs/ProductDTO.cs`):
  - ✅ osobny folder/namespace `StockFlow.Application.DTOs`, `public`, `required string Name`, `string? Description`, `decimal Price`.
  - ❌ brak `Status` — widok listy (screen) ma kolumnę Status; DTO = to, czego potrzebuje widok.
    Zalecenie: `string Status` (mapowane `.ToString()`), żeby DTO nie wystawiał enuma z Domain.
  - Drobne: nieużywane `using` (w tym `StockFlow.Domain.Model.Products`), nazwa wg konwencji .NET `ProductDto`
    (akronimy 3+ litery PascalCase), opcjonalnie `record` / `init` (DTO = niezmienna migawka danych).
- Poprawki punktu 1: plik i klasa → `ProductDto`, `init` zamiast `set`, usunięte `using`-i, dodany `string Status`.
  Build: 0 błędów, **1 ostrzeżenie CS8618** — `Status` bez `required` (niekonsekwencja względem `Name`).
  → poprawione (`required string Status`). **Punkt 1 zamknięty ✅**
- Punkt 2 (w trakcie) — `IProductRepository` miał `GetProductById` + `DeleteProduct` (synchronicznie).
  Uwagi: brakuje `GetAllAsync` (jedyna metoda potrzebna liście); Delete/GetById to etap 3 (YAGNI);
  dostęp do bazy → `Task<...>` + sufiks `Async`; `GetById` przy braku produktu → `Product?`;
  bez powtarzania „Product” w nazwach metod repozytorium produktów. Interfejs w Application + zwraca encje ✅.
  `ProductService` już `public` i implementuje `IProductService` ✅ (puste — punkt 3).
- Punkt 2, druga runda (2026-10-20): `Task<IReadOnlyList<Product>> GetAllAsync()` ✅.
  Pytanie „czy Delete też async?” — tak: zapis do bazy (`SaveChangesAsync`) → `Task DeleteAsync(int id)`;
  nigdy `async void` (nie da się `await`, wyjątek wywraca proces). Ale Delete i tak usunąć do etapu 3 (YAGNI),
  zakomentowany kod też usunąć (historia jest w Gicie).
  `IProductService`: `List<ProductDto> GetAllProducts()` → musi być async („async all the way”):
  `Task<IReadOnlyList<ProductDto>> GetAllAsync()`; usunąć szablonowe `using`-i.
- Trzecia runda: `IProductService` ✅ (`Task<IReadOnlyList<ProductDto>> GetAllAsync()`, czyste `using`-i).
  `IProductRepository` na dysku bez zmian (Delete + komentarz) — prawdopodobnie niezapisany plik.
  Build teraz nie przejdzie (CS0535 — `ProductService` nie implementuje `GetAllAsync`) — oczekiwane, to punkt 3.
- Przegląd całości: build 0/0. `ProductRepository` (punkt 4) już jest: konstruktor + `private readonly` ✅,
  `GetAllAsync` z `ToListAsync` ✅, ale **brak `AsNoTracking()`**; `Delete` synchroniczny (`Find`/`SaveChanges`)
  i nadal w interfejsie → usunąć do etapu 3 (albo `DeleteAsync` z `FindAsync`/`SaveChangesAsync`).
  `ProductService` = szkielet z `NotImplementedException` (brak konstruktora z repozytorium i mapowania).
  `Program.cs` — brak rejestracji `AddScoped` (punkt 5). Z csproj zniknęły puste `<Folder Include>` — OK, VS
  sam je usuwa, gdy folder ma pliki. Pytania kontrolne (DTO / interfejs w Application / Scoped) — jeszcze bez odpowiedzi.
- Kolejny przegląd: `ProductService` ma konstruktor z `IProductRepository` + `private readonly` ✅.
  `Delete` zakomentowany w interfejsie i repozytorium (zamiast usunięty) — prośba o usunięcie przed commitem.
  Zostało: `AsNoTracking()`, ciało `ProductService.GetAllAsync` (async/await, Select, ToString statusu, ToList),
  `AddScoped` ×2 w `Program.cs` (+ `using` do namespace'ów), build 0/0, odpowiedzi na 3 pytania, commit.
- Wyjaśnione: **DI (Dependency Injection)** — klasa dostaje zależności przez konstruktor zamiast robić `new`;
  kontener DI (`builder.Services`) = „rejestr przepisów” interfejs → klasa, sam składa łańcuch
  Controller → Service → Repository → DbContext; czasy życia Transient / Scoped / Singleton (ogólnie).
- Review kroków A–C: `AsNoTracking()` przed `ToListAsync()` ✅; `ProductService.GetAllAsync` — `async`/`await`,
  `Select` → `ProductDto`, `Status.ToString()`, `ToList()` ✅; `Program.cs` — 2× `AddScoped` przed `Build()` + `using`-i ✅
  (kolejność względem `AddDbContext` nie ma znaczenia — kontener składa obiekty dopiero przy żądaniu).
  Build (do osobnego folderu, bo uruchomiona aplikacja blokowała DLL-e — MSB3027): 0 błędów, 0 ostrzeżeń.
  Application bez EF/Infrastructure ✅. Do zamknięcia 2.3: usunąć zakomentowany kod, formatowanie (Ctrl+K, Ctrl+D),
  odpowiedzi na 3 pytania, commit.
- Odpowiedzi na pytania 2.3:
  1. DTO — „nie wszystkie dane potrzebne, bezpieczniej, szybciej” → bezpieczeństwo ✅, „szybciej” ❌ (u nas encja i tak
     pobierana w całości, mapowanie w pamięci). Dopowiedziane: oddzielenie widoku od kształtu encji (etap 7, nawigacje,
     API w etapie 13), dane przygotowane pod ekran (`Status` jako tekst).
  2. „Nie wiem” → wyjaśnione: Dependency Inversion — Application definiuje kontrakt, Infrastructure go spełnia;
     odwrotnie = Application zależne od EF + cykl referencji (Infrastructure → Application już istnieje).
  3. Scoped — „inny użytkownik może zmieniać dane” → kierunek nietrafiony (zapytanie i tak idzie do bazy).
     Właściwy powód: `DbContext` jest Scoped i **nie jest bezpieczny wielowątkowo**; serwis nie może żyć dłużej niż
     jego zależności (captive dependency — w Development kontener rzuci wyjątek przy starcie); jeden `DbContext`
     na żądanie = wspólna transakcja (etap 10/11).
  **Do powtórki:** Dependency Inversion, czasy życia DI i captive dependency.
- Zauważone przy okazji (jeszcze nie review): `Services/ProductService.cs` jest `internal` i pusty — musi być `public`;
  `Web/Controllers/ProductController.cs` to już krok 2.4 — konwencja nazwy `ProductsController`.

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
