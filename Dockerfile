FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /app
COPY . .
RUN dotnet publish -o out

FROM microsoft/aspnetcore:2.0
WORKDIR /app
COPY --from=build /app/out /app
EXPOSE 80
ENTRYPOINT ["dotnet", "Deck2MTGA-Web.dll"]
