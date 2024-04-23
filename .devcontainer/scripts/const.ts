export const devcontainerFile = '.devcontainer/devcontainer.json'
export const devcontainerHashFile = '.devcontainer/.devcontainer-hash.txt'
export const name = 'devcontainer/spark-connect-csharp'
export const registry = 'rakirahman.azurecr.io'

// Stores a list of pipeline files and the corresponding yaml key to update
export const pipelineToImageKeyList: [string, string][] = [
  ['.github/workflows/gci/gci.yaml', 'jobs:gci:container:image']
];
