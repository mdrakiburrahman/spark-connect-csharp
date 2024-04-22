# Apache Spark Connect Client for C#

<!-- TOC -->

- [Apache Spark Connect Client for C#](#apache-spark-connect-client-for-c)
  - [Current State of the Project](#current-state-of-the-project)
  - [Getting started](#getting-started)
  - [High Level Design](#high-level-design)
  - [Features](#features)
  - [Contributing](#contributing)
  - [Built With](#built-with)

<!-- /TOC -->

This project houses the **experimental** client for [Spark Connect](https://spark.apache.org/docs/latest/spark-connect-overview.html) for [Apache Spark](https://spark.apache.org/) written in [C#](https://learn.microsoft.com/en-us/dotnet/csharp/).

## Current State of the Project

Currently, the Spark Connect client for C# is highly experimental and should
not be used in any production setting.

## Getting started

This section explains how run Spark Connect C# locally from scratch.

Step 1: Install [Docker Desktop](https://www.docker.com/products/docker-desktop/) - for running a Spark Server in a Container.

Step 2: Install [Dotnet 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) CLI.

> 💡 Tip: [Install Visual Studio Community Edition](https://visualstudio.microsoft.com/) for a rich debugging experience in C#.

Step 3: Clone this repo via `git`.

Step 4: Spin up Spark Connect Server in a container:

Powershell:

```powershell
$GIT_ROOT = git rev-parse --show-toplevel
Set-Location -Path "${GIT_ROOT}\projects\spark-connect-csharp\src\spark.connect.test\Common\SparkHost"

docker-compose -f docker-compose-spark-connect-server.yaml up -d
```

Bash:

```bash
GIT_ROOT=$(git rev-parse --show-toplevel)
cd "${GIT_ROOT}/projects/spark-connect-csharp/src/spark.connect.test/Common/SparkHost"

docker-compose -f docker-compose-spark-connect-server.yaml up -d
```

Step 5: Run the demo project `spark.connect.demo` for an end-to-end demonstration.

```powershell
$GIT_ROOT = git rev-parse --show-toplevel
Set-Location -Path "${GIT_ROOT}\projects\spark-connect-csharp\src\spark.connect.demo"

dotnet run --project spark.connect.demo.csproj
```

```bash
GIT_ROOT=$(git rev-parse --show-toplevel)
cd "${GIT_ROOT}/projects/spark-connect-csharp/src/spark.connect.demo"

dotnet run --project spark.connect.demo.csproj
```

## High Level Design

`TODO`

## Features

`TODO`

## Contributing

This project uses a [Devcontainer](https://code.visualstudio.com/docs/devcontainers/containers) to manage Build Dependencies. The Devcontainer is tested upon every commit, to ensure the development environment is always ready.

See the [Devcontainer](.devcontainer/README.md) on how to get a development environment setup using Docker Desktop.

## Built With

These are the tools this project is built and maintained with:

- [nx](https://nx.dev/getting-started/intro) - Build management tool
- [docker](https://www.docker.com/) - Containerization
- [devcontainer](https://code.visualstudio.com/docs/remote/containers) - VS Code dev container
