FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Minecraft.Crafting.Api/Minecraft.Crafting.Api.csproj", "Minecraft.Crafting.Api/"]
RUN dotnet restore "Minecraft.Crafting.Api/Minecraft.Crafting.Api.csproj"
COPY src/. .
WORKDIR "/src/Minecraft.Crafting.Api"
RUN dotnet build "Minecraft.Crafting.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Minecraft.Crafting.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Minecraft.Crafting.Api.dll"]