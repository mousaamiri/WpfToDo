# WpfToDo

> اپلیکیشن دسکتاپ مدیریت وظایف (Todo App) مشابه **Microsoft To Do**، ساخته‌شده با **WPF** و **.NET 10** بر پایه الگوی **MVVM**.

<p align="left">
  <img alt="Platform" src="https://img.shields.io/badge/Platform-WPF-blue" />
  <img alt=".NET" src="https://img.shields.io/badge/.NET-10-512BD4" />
  <img alt="Pattern" src="https://img.shields.io/badge/Pattern-MVVM-green" />
  <img alt="Status" src="https://img.shields.io/badge/Status-In%20Development-orange" />
</p>

---

## 🚀 معرفی

هدف این پروژه ساخت یک اپلیکیشن Todo دسکتاپی است که وظایف را به‌صورت محلی مدیریت می‌کند: لیست‌های چندگانه، آیتم‌ها، دسته‌بندی و تم روشن/تاریک. رابط کاربری با WPF نوشته می‌شود و **منطق دامنه در لایه مستقل `WpfToDo.Core` (بدون هیچ وابستگی به UI)** قرار می‌گیرد تا کاملاً تست‌پذیر باشد.

> ⚠️ **وضعیت فعلی:** پروژه در مرحله **راه‌اندازی اولیه (scaffold)** است. ساختار solution و پروژه‌ها آماده است، اما فیچرهای کاربری هنوز پیاده‌سازی نشده‌اند. جدول زیر وضعیت دقیق را نشان می‌دهد.

---

## ✨ وضعیت ویژگی‌ها

| ویژگی | وضعیت |
|---|---|
| ساختار solution و سه پروژه (App / Core / Tests) | ✅ آماده |
| ایجاد و مدیریت چند لیست Todo | ⬜ برنامه‌ریزی‌شده |
| افزودن، ویرایش و حذف آیتم | ⬜ برنامه‌ریزی‌شده |
| تغییر وضعیت آیتم (تکمیل / در حال انجام) | ⬜ برنامه‌ریزی‌شده |
| دسته‌بندی آیتم‌ها | ⬜ برنامه‌ریزی‌شده |
| تم روشن و تاریک (Dark/Light) با `DynamicResource` | ⬜ برنامه‌ریزی‌شده |
| ذخیره‌سازی محلی (Persistence) | ⬜ برنامه‌ریزی‌شده |

<sub>این جدول پس از تکمیل هر فیچر قابل‌مشاهده به‌روزرسانی می‌شود.</sub>

---

## 🧱 معماری و پشته فناوری

- **Framework:** WPF روی `net10.0-windows`
- **زبان:** C# با `Nullable` و `ImplicitUsings` فعال
- **الگو:** MVVM — بدون منطق تجاری در code-behind؛ فقط binding و `DataTemplate`
- **کتابخانه MVVM:** `CommunityToolkit.Mvvm` (`ObservableObject` / `RelayCommand`) — *پس از شروع پیاده‌سازی ViewModelها اضافه می‌شود*
- **تست:** xUnit
- **جداسازی لایه‌ها:** تمام منطق دامنه در `WpfToDo.Core` بدون ارجاع به WPF، تا بدون UI قابل تست باشد

---

## 📁 ساختار پروژه

```
WpfToDo/
├── src/
│   ├── WpfToDo/                 # پروژه اصلی WPF (App، Views، ViewModels، Themes)
│   │   ├── Models/             # (planned)
│   │   ├── ViewModels/         # (planned)
│   │   ├── Views/              # (planned)
│   │   ├── Services/           # (planned)
│   │   ├── Themes/             # ResourceDictionaryهای Dark/Light (planned)
│   │   └── Converters/         # (planned)
│   └── WpfToDo.Core/           # منطق دامنه، مستقل از UI (تست‌پذیر)
├── tests/
│   └── WpfToDo.Tests/          # تست‌های xUnit
└── WpfToDo.slnx                # فایل solution
```

---

## 🛠️ پیش‌نیازها

- **.NET 10 SDK** یا بالاتر
- **Windows** (به دلیل وابستگی WPF به `net10.0-windows`)

---

## ▶️ اجرا و ساخت

از پوشه ریشه پروژه:

```bash
# ساخت کل solution
dotnet build

# اجرای اپلیکیشن
dotnet run --project src/WpfToDo/WpfToDo.csproj
```

## ✅ اجرای تست‌ها

```bash
dotnet test
```

---

## 🧭 قواعد توسعه

این پروژه چند قانون سخت‌گیرانه دارد:

- **TDD الزامی:** چرخه Red → Green → Refactor برای هر فیچر یا تغییر رفتار.
- **MVVM خالص:** هیچ منطق تجاری در `.xaml.cs`؛ ViewModelها از `ObservableObject` ارث‌بری می‌کنند.
- **تم‌بندی:** رنگ‌ها فقط از طریق `DynamicResource`، هرگز hardcode نشوند.
- **کامیت:** پیام‌ها به سبک Conventional (`feat:`, `fix:`, `refactor:`, `test:`, `docs:`, `chore:`) و هر کامیت باید build و تست‌ها را سبز نگه دارد.

---

## 🗺️ نقشه راه (Roadmap)

1. تعریف مدل‌های دامنه (`TodoItem`, `TodoList`) در `WpfToDo.Core` همراه تست.
2. افزودن `CommunityToolkit.Mvvm` و ساخت ViewModelها.
3. پیاده‌سازی CRUD آیتم‌ها و لیست‌ها در UI.
4. سیستم تم Dark/Light با `ResourceDictionary`.
5. ذخیره‌سازی محلی (JSON یا SQLite).

---

<sub>این README وضعیت واقعی پروژه را منعکس می‌کند و با پیشرفت کار به‌روزرسانی خواهد شد.</sub>
