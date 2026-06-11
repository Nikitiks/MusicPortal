FROM mcr.microsoft.com/dotnet/sdk:9.0 as build
WORKDIR /app

COPY MusicPortal.sln .
COPY MusicPortal/MusicPortal.csproj ./MusicPortal/
COPY MusicPortal.BLL/MusicPortal.BLL.csproj ./MusicPortal.BLL/
COPY MusicPortal.Common/MusicPortal.Common.csproj ./MusicPortal.Common/
COPY MusicPortal.DAL/MusicPortal.DAL.csproj ./MusicPortal.DAL/

RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o /out


FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

COPY --from=build /out .

ENTRYPOINT ["dotnet","MusicPortal.dll"]