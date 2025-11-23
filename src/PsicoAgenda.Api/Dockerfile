FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER app 
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PsicoAgenda.Api/PsicoAgenda.Api.csproj", "PsicoAgenda.Api/"]
COPY ["PsicoAgenda.Application/PsicoAgenda.Application.csproj", "PsicoAgenda.Application/"]
COPY ["PsicoAgenda.Domain/PsicoAgenda.Domain.csproj", "PsicoAgenda.Domain/"]
COPY ["PsicoAgenda.Infrastructure/PsicoAgenda.Infrastructure.csproj", "PsicoAgenda.Infrastructure/"]
COPY ["PsicoAgenda.Persistence/PsicoAgenda.Persistence.csproj", "PsicoAgenda.Persistence/"]
RUN dotnet restore "PsicoAgenda.Api/PsicoAgenda.Api.csproj"
COPY . .
WORKDIR "/src/PsicoAgenda.Api"
RUN dotnet build "PsicoAgenda.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "PsicoAgenda.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PsicoAgenda.Api.dll"]