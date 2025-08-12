FROM hub.nexdev.net/base/dotnet/core/sdk:8.0-alpine AS builder

WORKDIR /src

RUN dotnet nuget add source https://nuget.nexdev.net/api/v2

COPY ["src/Fanex.DepositSettingAPI.Domain/Fanex.DepositSettingAPI.Domain.csproj", "./Fanex.DepositSettingAPI.Domain/"]
RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet restore "Fanex.DepositSettingAPI.Domain/Fanex.DepositSettingAPI.Domain.csproj" -v n -r linux-x64

COPY ["src/Fanex.DepositSettingAPI.Application/Fanex.DepositSettingAPI.Application.csproj", "./Fanex.DepositSettingAPI.Application/"]
RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet restore "Fanex.DepositSettingAPI.Application/Fanex.DepositSettingAPI.Application.csproj" -v n -r linux-x64

COPY ["src/Fanex.DepositSettingAPI.Infrastructure/Fanex.DepositSettingAPI.Infrastructure.csproj", "./Fanex.DepositSettingAPI.Infrastructure/"]
RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet restore "Fanex.DepositSettingAPI.Infrastructure/Fanex.DepositSettingAPI.Infrastructure.csproj" -v n -r linux-x64

COPY ["src/Fanex.DepositSettingAPI.Presentation/Fanex.DepositSettingAPI.Presentation.csproj", "./Fanex.DepositSettingAPI.Presentation/"]
RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet restore "Fanex.DepositSettingAPI.Presentation/Fanex.DepositSettingAPI.Presentation.csproj" -v n -r linux-x64

COPY ["src/Fanex.DepositSettingAPI.WebApi/Fanex.DepositSettingAPI.WebApi.csproj", "./Fanex.DepositSettingAPI.WebApi/"]
RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet restore "Fanex.DepositSettingAPI.WebApi/Fanex.DepositSettingAPI.WebApi.csproj" -v n -r linux-x64

COPY src .

RUN dotnet publish "Fanex.DepositSettingAPI.WebApi/Fanex.DepositSettingAPI.WebApi.csproj" \
    -c Release \
    -o /publish \
    -r linux-x64 \
    --use-current-runtime \
    --self-contained false

FROM hub.nexdev.net/base/dotnet/core/aspnet:8.0-alpine-25h2 AS final

ENV DOTNET_HOSTBUILDER__RELOADCONFIGONCHANGE=false
ENV ASPNETCORE_HTTP_PORTS=5000
EXPOSE 5000

# Dont run as root
RUN adduser --disabled-password \
    --home /app \
    --gecos '' dotnetuser && chown -R dotnetuser /app && chown -R dotnetuser /tmp
USER dotnetuser
#

WORKDIR /app

COPY --from=builder /publish .
ENV TZ="America/La_Paz"

ENTRYPOINT ["dotnet", "Fanex.DepositSettingAPI.WebApi.dll"]
