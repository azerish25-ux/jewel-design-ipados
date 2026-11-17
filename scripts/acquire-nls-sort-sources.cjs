'use strict';
const fs = require('node:fs/promises');
const path = require('node:path');
const crypto = require('node:crypto');
const root = path.resolve(__dirname, '..');
const assets = [
  { name: 'Windows 11 Sorting Weight Table.txt', bytes: 21219085, sha256: 'f0bab873ed8901ae17afc4848c7d9e0a7de4310942effb2b54b538411b26d542' },
  { name: 'Windows 7 and Windows server 2008 R2 Sorting Weight Table.txt', bytes: 17304503, sha256: '6c719bcfd0bee899e5e41c5d1a21f295d457988427bca9283d6d955fd4b39c09' }
];
const sha = bytes => crypto.createHash('sha256').update(bytes).digest('hex');
async function acquire(asset) {
  const folder = path.join(root, '.local/reference/windows-sorting');
  const destination = path.join(folder, asset.name);
  const url = 'https://download.microsoft.com/download/c/f/7/cf713a5e-9fbc-4fd6-9246-275f65c0e498/' + encodeURIComponent(asset.name);
  await fs.mkdir(folder, { recursive: true });
  try {
    const existing = await fs.readFile(destination);
    if (existing.length !== asset.bytes || sha(existing) !== asset.sha256) throw Error('Existing source identity differs: ' + asset.name);
    return { ...asset, url, cached: true };
  } catch (error) { if (error.code !== 'ENOENT') throw error; }
  const response = await fetch(url);
  if (!response.ok) throw Error('Official source HTTP ' + response.status);
  const chunks = [];
  let count = 0;
  for await (const chunk of response.body) {
    count += chunk.length;
    if (count > asset.bytes) throw Error('Official source exceeds approved size');
    chunks.push(Buffer.from(chunk));
  }
  const bytes = Buffer.concat(chunks);
  if (bytes.length !== asset.bytes || sha(bytes) !== asset.sha256) throw Error('Official source digest differs');
  await fs.writeFile(destination, bytes);
  return { ...asset, url, cached: false };
}
async function main() {
  if (process.platform === 'win32' && !/^D:/i.test(root)) throw Error('Windows writes must remain on D:');
  const results = await Promise.allSettled(assets.map(acquire));
  for (const result of results) {
    if (result.status === 'rejected') throw result.reason;
    console.log(JSON.stringify(result.value));
  }
}
if (require.main === module) main().catch(error => { console.error(error); process.exitCode = 1; });
module.exports = { assets };
