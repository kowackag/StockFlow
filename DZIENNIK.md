# StockFlow — dziennik sesji

Postępy z kolejnych sesji nauki. Najnowsza sesja na górze.
Plan i zasady współpracy: [`HARMONOGRAM.md`](HARMONOGRAM.md).

---

## ▶ Następnym razem zaczynamy od

**Etap 1 — Szkielet solution (4 projekty)** — w trakcie, robione w VS 2026: pusta solucja, 3× Class Library,
1× ASP.NET Core MVC w `src/`, referencje, build, uruchomienie strony MVC, odpowiedzi na 3 pytania sprawdzające.

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

**Ustalenia:**
- Solution i projekty tworzymy w Visual Studio 2026 (zmiana — początkowo miał być terminal), projekty w folderze `src/`.

**Otwarte sprawy:**
- Decyzje domenowe (cena na produkcie/wariancie, waluta, wariant domyślny) — do omówienia przed etapami 2 i 6.
