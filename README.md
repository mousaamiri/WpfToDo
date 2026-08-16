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

> ⚠️ **وضعیت فعلی:** بک‌اند پروژه شامل مدل‌های دامنه، EF Core/SQLite، سرویس‌های CRUD، Wrapperهای validation/change tracking و ViewModelهای اصلی است. ساخت View/XAML هنوز انجام نشده است.

---

## ✨ وضعیت ویژگی‌ها

| ویژگی | وضعیت |
|---|---|
| ساختار solution و سه پروژه (App / Core / Tests) | ✅ آماده |
| مدل‌های Category، TodoList و TodoItem | ✅ آماده |
| DbContext با EF Core و SQLite | ✅ آماده |
| سرویس‌های CRUD لیست، آیتم و دسته‌بندی | ✅ آماده |
| ViewModelهای TodoLists و TodoItems | ✅ آماده |
| Wrapperهای validation و change tracking | ✅ آماده |
| تست‌های واحد بک‌اند | ✅ آماده |
| ایجاد و مدیریت چند لیست Todo در UI | ⬜ برنامه‌ریزی‌شده |
| افزودن، ویرایش و حذف آیتم در UI | ⬜ برنامه‌ریزی‌شده |
| تغییر وضعیت آیتم در UI | ⬜ برنامه‌ریزی‌شده |
| دسته‌بندی و تم روشن/تاریک در UI | ⬜ برنامه‌ریزی‌شده |
| تم روشن و تاریک (Dark/Light) با `DynamicResource` | ⬜ برنامه‌ریزی‌شده |
| ذخیره‌سازی محلی در محیط اجرا | ✅ زیرساخت آماده |

<sub>این جدول پس از تکمیل هر فیچر قابل‌مشاهده به‌روزرسانی می‌شود.</sub>

---

## 🧱 معماری و پشته فناوری

- **Framework:** WPF روی `net10.0-windows`
- **زبان:** C# با `Nullable` و `ImplicitUsings` فعال
- **الگو:** MVVM — بدون منطق تجاری در code-behind؛ فقط binding و `DataTemplate`
- **کتابخانه MVVM:** `CommunityToolkit.Mvvm` (`ObservableObject` / `RelayCommand`)
- **داده:** `Microsoft.EntityFrameworkCore.Sqlite`
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

## 🐳 اجرا در Docker (فقط تست‌ها)

لایه منطق دامنه (`WpfToDo.Core`) و تست‌های واحد (`WpfToDo.Tests`) مستقل از WPF هستند و می‌توانند داخل کانتینر build و اجرا شوند. خودِ اپ گرافیکی WPF چون مخصوص ویندوز است در Docker build/اجرا نمی‌شود و از این image کنار گذاشته شده.

```bash
docker compose up --build
```

یا مستقیم:

```bash
docker build -t wpftodo-tests .
docker run --rm wpftodo-tests
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

1. ساخت View و اتصال آن‌ها به ViewModelها.
2. اضافه‌کردن سیستم تم Dark/Light با `ResourceDictionary`.
3. تکمیل wiring برنامه در `App.xaml.cs` و اجرای migration/database startup.

---

<sub>این README وضعیت واقعی پروژه را منعکس می‌کند و با پیشرفت کار به‌روزرسانی خواهد شد.</sub>
