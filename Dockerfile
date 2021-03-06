FROM microsoft/dotnet:2.1-sdk AS dotnet-builder
RUN apt-get update
WORKDIR /opt/AgenciadeEmpleoVirtualapi
# copy csproj and restore as distinct layers
COPY . .
RUN dotnet restore
 
 
# copy everything else and build
RUN dotnet publish --output /output --configuration Release
 
 
# build runtime image
FROM microsoft/dotnet:2.1-aspnetcore-runtime
COPY --from=dotnet-builder /output /app
WORKDIR /app
 
 
#Expose port 80
EXPOSE 80
ENTRYPOINT ["dotnet", "AgenciaDeEmpleoVirutal.Services.dll"]