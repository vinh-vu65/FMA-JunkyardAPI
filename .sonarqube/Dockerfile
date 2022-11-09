FROM mcr.microsoft.com/dotnet/sdk:6.0

ARG HOST_URL="https://sonarqube.svc.platform.myob.com/"
ARG PROJECT_KEY
ARG PROJECT_NAME
ARG TOKEN
ARG SOLUTION_FILE
ARG BRANCH
ARG PULL_REQUEST
ARG BASE_BRANCH

COPY . ./

# Install Java dependency for SonarScanner and restore SonarScanner
RUN mkdir -p /usr/share/man/man1

RUN apt-get update -y \
    && apt-get install --no-install-recommends -y apt-transport-https \
    && apt-get update -y \
    && apt-get install --no-install-recommends -y openjdk-11-jre \
    && apt-get install -y shellcheck
RUN dotnet tool install --global dotnet-sonarscanner
ENV PATH="$PATH:/root/.dotnet/tools"
ENV LANG C.UTF-8

# Start SonarScanner
RUN if [ "$PULL_REQUEST" = "false" ]; then \
        dotnet sonarscanner begin \
        /k:"$PROJECT_KEY" \
        /n:"$PROJECT_NAME" \
        /d:sonar.host.url="$HOST_URL" \
        /d:sonar.qualitygate.wait=true \
        /d:sonar.login="$TOKEN" \
        /d:sonar.branch.name="$BRANCH" \
        ; \
    else \
        dotnet sonarscanner begin \
        /k:"$PROJECT_KEY" \
        /n:"$PROJECT_NAME" \
        /d:sonar.host.url="$HOST_URL" \
        /d:sonar.qualitygate.wait=true \
        /d:sonar.login="$TOKEN" \
        /d:sonar.pullrequest.branch="$BRANCH" \
        /d:sonar.pullrequest.key="$PULL_REQUEST" \
        /d:sonar.pullrequest.base="$BASE_BRANCH" \
        ;\
    fi

# Run unit tests and build
RUN dotnet build "$SOLUTION_FILE"

# Finish SonarScanner
RUN dotnet sonarscanner end \
    /d:sonar.login="$TOKEN"