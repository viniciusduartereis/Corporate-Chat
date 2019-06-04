FROM microsoft/dotnet:2.2-sdk AS builder

WORKDIR /src
COPY . .

RUN dotnet publish "Corporate.Chat.API.csproj" -c Release -o /out


FROM microsoft/dotnet:2.2-aspnetcore-runtime AS final
WORKDIR /app

COPY --from=builder /out .

ENTRYPOINT ["dotnet", "Corporate.Chat.API.dll"]
EXPOSE 80
EXPOSE 443
