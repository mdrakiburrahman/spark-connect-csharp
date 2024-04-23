import { spawnSync } from 'child_process';
import { Command, OptionValues } from 'commander';
import { writeFileSync, readFileSync } from 'fs';
import { load, dump } from 'js-yaml';
import { registry, name, devcontainerFile, pipelineToImageKeyList } from './const'

const opts: OptionValues = {
  registry: registry,
  name: name,
  file: devcontainerFile,
  pipelineToImageKeyList: pipelineToImageKeyList
};

const fullImageName = `${opts.registry}/${opts.name}`
const imageTag = readFileSync('.devcontainer/.devcontainer-hash.txt', 'utf8');

console.log(`Updating devcontainer config file: ${opts.file}`);
updateDevcontainerConfigFile(fullImageName, imageTag, opts.file);

// Github actions don't allow us to specify a runtime variable for an image,
// so we git commit the image name and tag to the pipeline config files
//
// >>> https://docs.github.com/en/actions/using-jobs/running-jobs-in-a-container
//
opts.pipelineToImageKeyList.forEach(([pipeline, key]) => {
  console.log(`Updating pipeline config file: ${pipeline}`);
  updatePipelineConfigFile(key, `${fullImageName}:${imageTag}`, pipeline);
});

if (checkIfImageExists(fullImageName, imageTag)) {
    console.log(`Image ${fullImageName}:${imageTag} already exists in ACR. Skipping push...`);
    process.exit(0);
}

console.log(`Pushing devcontainer image: ${fullImageName}:${imageTag}`);
pushDockerImage(fullImageName, imageTag);

console.log('Done!');

/**
 * Checks if the specified image exists in the registry
 * @param imageName The name of the image to push
 * @param imageTag The tag of the image to push
 * @returns True if the image exists in the registry, false otherwise
 */
function checkIfImageExists(imageName: string, imageTag: string): boolean {
    const cmd = ['docker', 'manifest', 'inspect', `${imageName}:${imageTag}`]

    // Check if manifest exists
    const output = spawnSync(cmd.join(' '), { shell: true });
    if (output.status !== 0) {
        if (output.stderr.toString().includes('no such manifest')) {
            return false;
        }
        throw new Error(`Error checking if image ${imageName}:${imageTag} exists: ${output.stderr.toString()}}`);
    }

    console.log(output.stdout.toString());
    return true;
}

/**
 * Pushes a docker image to the specified registry
 * @param imageName The name of the image to push
 * @param imageTag The tag of the image to push
 */
function pushDockerImage(imageName: string, imageTag: string) {
    const cmd = ['docker', 'image', 'push', `${imageName}:${imageTag}`]
    console.log(`Pushing docker image: ${imageName}:${imageTag}`);
    const output = spawnSync(cmd.join(' '), { stdio: 'inherit', shell: true });
    if (output.status !== 0) {
        throw new Error(`Failed to push docker image: ${imageName}:${imageTag}`);
    }

    console.log(`Successfully pushed docker image: ${imageName}:${imageTag}`);
}

/**
 * Creates/updates a devcontainer.json settings file with the
 * devcontainer image name and tag
 * @param imageName The name of the image
 * @param imageTag The tag of the image
 * @param filename The filename to write/update
 */
function updateDevcontainerConfigFile(imageName: string, imageTag: string, filename: string) {
    const data = {
        "name": "MonitoringDevcontainer",
        "image": `${imageName}:${imageTag}`,
        "remoteUser": "vscode",
        "containerUser": "vscode"
    }

    writeFileSync(filename, JSON.stringify(data, null, 4));
}

/**
 * Updates a specific key in a YAML file with a new value.
 *
 * @param keyToReplace A string representing the key to replace in the YAML file.
 *                     The string should contain the path to the key, with each level separated by a colon.
 *                     For example, to replace the key 'bar' in the object 'foo' which is in the object 'baz',
 *                     the string would be 'baz:foo:bar'.
 * @param value The new value to set for the key.
 * @param filename The path to the YAML file to update.
 *
 * This function reads the YAML file into a JavaScript object, traverses the object to find the key to replace,
 * updates the value of the key, and then writes the updated object back to the YAML file.
 *
 * Note: This function assumes that the YAML file and the keys exist. If they don't, you'll need to add error handling code.
 */
function updatePipelineConfigFile(keyToReplace: string, value: string, filename: string) {
  const fileContents = readFileSync(filename, 'utf8');
  const data = load(fileContents) as Record<string, any>;

  const keys = keyToReplace.split(':');
  let current: Record<string, any> = data;
  for (let i = 0; i < keys.length - 1; i++) {
      current = current[keys[i]] as Record<string, any>;
  }
  current[keys[keys.length - 1]] = value;

  const yaml = dump(data);
  writeFileSync(filename, yaml);
}
