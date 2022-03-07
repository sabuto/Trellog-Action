# Set the base image as the .NET 6.0 SDK (this includes the runtime)
FROM mcr.microsoft.com/dotnet/sdk:6.0 as build-env

# Copy everything and publish the release (publish implicitly restores and builds)
COPY . ./
RUN dotnet publish ./Trellog-Action/Trellog-Action.csproj -c Release -o out --no-self-contained

# Label the container
LABEL maintainer="Robert Dunne"
LABEL repository="https://github.com/sabuto/trellog-action"
LABEL homepage="https://github.com/sabuto/trellog-action"

# Label as GitHub action
LABEL com.github.actions.name="Trello Changelog Builder"
LABEL com.github.actions.description="A Github action that will allow you to build a changelog from a trello board."
LABEL com.github.actions.color="red"

# Relayer the .NET SDK, anew with the build output
FROM mcr.microsoft.com/dotnet/sdk:6.0
COPY --from=build-env /out .
ENTRYPOINT [ "dotnet", "/Trellog-Action.dll" ]