#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see http://aka.ms/containercompat 

FROM microsoft/dotnet:2.2-aspnetcore-runtime-nanoserver-1803 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk-nanoserver-1803 AS build
WORKDIR /src
COPY FileManagerAPI/FileManagerAPI.csproj FileManagerAPI/
COPY FileManagerBussinessLogic/FileManagerBussinessLogic.csproj FileManagerBussinessLogic/
COPY FileManagerDBLogic/FileManagerDBLogic.csproj FileManagerDBLogic/
RUN dotnet restore FileManagerAPI/FileManagerAPI.csproj
COPY . .
WORKDIR /src/FileManagerAPI
RUN dotnet build FileManagerAPI.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish FileManagerAPI.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "FileManagerAPI.dll"]
