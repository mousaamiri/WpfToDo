# توجه مهم:
# WPF یک UI framework مخصوص ویندوز است و روی لینوکس/کانتینر اجرا نمی‌شود.
# این Dockerfile برای *اجرای گرافیکی* WpfToDo.exe نیست؛ بلکه برای build و اجرای
# تست‌های خودکار (CI) روی لایه‌های مستقل از WPF است:
#   - WpfToDo.Core   (منطق دامنه)
#   - WpfToDo.Tests  (تست‌های واحد؛ ViewModel ها هم چون طبق CLAUDE.md مستقل از WPF‌اند اینجا تست می‌شوند)
#
# برای توسعه و اجرای واقعی اپ WPF همچنان روی ویندوز با dotnet run استفاده کن.

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# کپی فایل‌های csproj/sln به‌تنهایی برای بهره‌گیری از cache لایه‌ها در restore
COPY WpfToDo.slnx ./
COPY src/WpfToDo.Core/WpfToDo.Core.csproj src/WpfToDo.Core/
COPY tests/WpfToDo.Tests/WpfToDo.Tests.csproj tests/WpfToDo.Tests/

# پروژه WPF خودش قابل restore/build در لینوکس نیست (نیازمند Windows Desktop SDK)
# بنابراین فقط پروژه‌های Core و Tests را build می‌کنیم، نه کل solution.
RUN dotnet restore src/WpfToDo.Core/WpfToDo.Core.csproj
RUN dotnet restore tests/WpfToDo.Tests/WpfToDo.Tests.csproj

COPY src/WpfToDo.Core/ src/WpfToDo.Core/
COPY tests/WpfToDo.Tests/ tests/WpfToDo.Tests/

RUN dotnet build src/WpfToDo.Core/WpfToDo.Core.csproj -c Release --no-restore
RUN dotnet build tests/WpfToDo.Tests/WpfToDo.Tests.csproj -c Release --no-restore

ENTRYPOINT ["dotnet", "test", "tests/WpfToDo.Tests/WpfToDo.Tests.csproj", "-c", "Release", "--no-restore", "--logger", "console;verbosity=normal"]
