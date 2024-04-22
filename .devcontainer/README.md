# The Devcontainer

This devcontainer is setup in such a way that we can push the prebuilt devcontainer (with its features and devcontainer settings) to our private Azure Container Registry. This achieves two goals:

- Developers can spend less time waiting for the devcontainer to build as they can pull the prebuilt image.
- The monitoring project can leverage the prebuilt devcontainer image in our CI pipelines.

## How to push changes to the prebuilt devcontainer

> You must have write access to perform these steps. Note that the ACR has [anonymous pull](https://learn.microsoft.com/en-us/azure/container-registry/anonymous-pull-access) enabled - so you can read without authenticating and build this project. To receive write access, open a Github issue.

Note that if you simply want to rebuild the development container to fix vulnerabilities, bump the `version.txt` file. This will trigger a rebuild of the devcontainer image since we rebuild when the file content changes in the `.devcontainer` folder.

1. Make your changes to the devcontainer.
2. Login to the Azure Container Registry:
    1. `az login --tenant 53299840-adc8-4223-b19e-7400531ed554 --use-device-code`
    2. `az acr login --name rakirahman`

3. Run `npx nx run devcontainer:publish --skip-nx-cache` to build and push the devcontainer to the Azure Container Registry (skipping the cache allows for cache invalidation related problems).
4. Check in your changes to the `main` branch.

## How does the prebuilt devcontainer work?

1. First, we compute the file hash of all contents of the `.devcontainer` folder. This is used as the tag for the prebuilt devcontainer image. `.devcontainer/devcontainer-hash.txt`
2. We then build the devcontainer image with the computed tag using the `devcontainer` cli tool.
    1. We instruct the `devcontainer` cli tool to use the `devcontainer.local.json` file instead of the default `devcontainer.json`.
    2. The `devcontainer.local.json` file uses the `Dockerfile.local` file to build it's image.
3. We then publish the image
    1. We publish the image to the Azure Container Registry.
    2. After this push happens, we automatically write the new image tag to the `devcontainer.json` file.
    3. Finally, we automatically update the `.github/workflows/templates/container.template.yaml` file to use the new image tag in CI pipelines.
4. Finally, when any engineer opens the devcontainer, the `devcontainer.json` file is used to pull the prebuilt image from the Azure Container Registry.

## Use of the Devcontainer in our Pipelines

The devcontainer is used in our pipelines as our build environment. This means a developer's build environment is exactly the same as the pipeline build environment other than a few caveats:

- Github Pipelines needs to start with the `root` user whilst a developer's devcontainer starts with the non-root `vscode` user.

## References

- [devcontainer Overview](https://code.visualstudio.com/docs/devcontainers/containers)
- [devcontainer CLI docs](https://containers.dev/implementors/reference/)
- [devcontainer image build example](https://github.com/devcontainers/cli/blob/main/example-usage/image-build/build-image.sh)
- [devcontainer metadata reference](https://containers.dev/implementors/json_reference/)
