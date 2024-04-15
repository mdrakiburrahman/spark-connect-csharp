/**
 * This script is used to enable the nx docker cache - workaround for:
 *
 * >>>  https://github.com/nrwl/nx/issues/14126
 */

import { spawnSync } from 'child_process';
import { join } from 'path';
import { readFileSync, writeFileSync } from 'fs';

const node_modules_dir = get_node_modules_folder();
const nx_module_name = 'nx'
const nx_docker_cache_file = join('src', 'daemon', 'client', 'client.js')
const searchTerm = 'isDocker() ||';

// Read the file
const filename = join(node_modules_dir, nx_module_name, nx_docker_cache_file);
const content = readFileSync(filename, 'utf-8');

// Remove the matching line
const updatedContent = content.split('\n')
    .filter(line => !line.includes(searchTerm))
    .join('\n');

// Write the updated content back to the file
writeFileSync(filename, updatedContent);

console.log(`Removed '${searchTerm}' from ${filename}`)

// Reset nx and start the daemon
spawnSync('npx nx reset', { shell: true, stdio: 'inherit' });
spawnSync('npx nx daemon --start', { shell: true, stdio: 'inherit' });
spawnSync('npx nx daemon', { shell: true, stdio: 'inherit' })

/**
 * Gets the path to the node_modules folder
 * @returns The path to the node_modules folder
 */
function get_node_modules_folder(): string {
    const output = spawnSync('npm root', { shell: true });
    return output.stdout.toString().trim();
}
