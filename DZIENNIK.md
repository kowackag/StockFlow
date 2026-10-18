# StockFlow — dziennik sesji

Postępy z kolejnych sesji nauki. Najnowsza sesja na górze.
Plan i zasady współpracy: [`HARMONOGRAM.md`](HARMONOGRAM.md).

---

## ▶ Następnym razem zaczynamy od

**Etap 2 — w trakcie: krok 2.1 (Domain) ✅ zamknięty (commit `500f1f1`, pytanie decimal/double — OK); teraz krok 2.2 (EF Core: pakiety, `StockFlowDbContext`,
`ProductConfiguration` + `HasData`, connection string, rejestracja w `Program.cs`, migracja `InitialCreate`)** + 3 pytania
sprawdzające (dlaczego Tools w Web; dlaczego Fluent API w Infrastructure zamiast atrybutów).
Pamiętać o konfiguracji `Description` (opcjonalne).
Potem: 2.3 (DTO, `IProductRepository`, `ProductService`, `ProductRepository`) i 2.4 (kontroler + widok listy).

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
- Po review: dodana referencja Web → Infrastructure, usunięty folder `Interfaces` z Domain
  (interfejsy tylko w Application). Folder `Model` w Domain zostaje (decyzja własna).
- Język VS zmieniony na angielski. Nazwa `StockFlow.Web` zostaje (zamiast `.Presentation`) — Web to też composition root.
- Projekty utworzone w VS w `src/` (struktura na dysku poprawna), `net10.0`, `Class1.cs` usunięte, build 0 błędów.
- Pytania sprawdzające: 1 — dobry kierunek (Domain = rdzeń, zależności wskazują do środka); 2 i 3 — omówione
  (composition root w `Program.cs`; `Domain → Infrastructure` = cykl + uzależnienie reguł biznesowych od technologii).
  Do powtórki przy etapie 2, gdy pojawi się rejestracja DI.

**Etap 2 — rozpoczęty**
- LocalDB dostępne (`MSSQLLocalDB`). Waluta: PLN (tylko wyświetlanie, `decimal` w kodzie).
- 2026-10-18: decyzja o cenie — model C (cena bazowa na `Product`, opcjonalna na wariancie od etapu 6).
  Otwarte: gdzie trzymać stan produktu bez wariantów (propozycja: ukryty wariant domyślny) — przed etapem 6.
- Omówione: enum to osobny rodzaj typu (nie klasa); enum w folderze `Model` w Domain.
- Review 2.1 (pierwsze podejście): build ❌ CS0101 — folder `Model/Product` (namespace) koliduje z klasą `Product`.
  Do poprawy: folder w l. mnogiej (`Products`) lub pliki w `Model`; namespace = ścieżka folderów; `internal` → `public`;
  `Enums.cs` → `PublicationStatus.cs`; dodać 4 właściwości `Product` (+ nullable dla `Name`).
  Opcjonalnie: usunąć zbędne `using`, file-scoped namespace.
- Review 2.1 (drugie podejście): ✅ build OK. `Model/Products/Product.cs` (Id, Name = "", Description `string?` — własny
  dodatek, Price `decimal`, Status) + `PublicationStatus.cs`. Do przemyślenia: `required` zamiast `= ""` (wrócić w etapie 7).
  Brakuje: commitu kroku 2.1 i odpowiedzi na pytanie „decimal vs double”. Następnie krok 2.2.
  Uwaga do 2.2: `Description` też wymaga konfiguracji (opcjonalne, ewentualnie limit długości).
- Review 2.2 (w trakcie): ✅ Tools przeniesione do Web, `DbContextOptions<StockFlowDbContext>`, connection string
  `DefaultConnection` (LocalDB) w `appsettings.json`. ❌ `AddDbContext` stoi PO `app.Run()` — do przeniesienia przed
  `builder.Build()` (omówiona kolejność w Program.cs: rejestracja usług → Build → pipeline → Run).
  Otwarte przed migracją: realistyczne dane w `HasData` (6 produktów, PLN; teraz 5× „Product N”), decyzja
  `OnModelCreating` vs osobna `ProductConfiguration`, pusta linia `Property(Description)`, skąd pakiet `System.Security.Cryptography.Pkcs`.
  Kursu Duszyńskiego nie znam w szczegółach — temat rejestracji DbContext szukać pod „DI / AddDbContext” (starsze .NET: `Startup.cs`).
- Decyzja: pozostałe klasy domeny (warianty, Brand, Supplier, Address) NIE teraz — kodowane w swoich etapach.
  Na start etapu 5 projektujemy razem model domeny w `MODEL-DOMENY.md`. Do harmonogramu dopisane:
  `Brand` (etap 5), `Supplier` opcjonalnie (etap 9), `Address` jako value object (etap 11).
- Sprostowanie z review etapu 1: referencje projektów SĄ przechodnie (Web widzi Domain przez Application);
  Web → Infrastructure była potrzebna, bo Infrastructure nie leży w łańcuchu Web → Application → Domain.

**Ustalenia:**
- Solution i projekty tworzymy w Visual Studio 2026 (zmiana — początkowo miał być terminal), projekty w folderze `src/`.

**Otwarte sprawy:**
- Decyzje domenowe (cena na produkcie/wariancie, waluta, wariant domyślny) — do omówienia przed etapami 2 i 6.
