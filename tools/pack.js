import generateEvb from 'generate-evb';
import evb from 'enigmavirtualbox';
import * as FS from "fs";
import * as Path from 'path'

console.log("Combining paths...");

let dir = process.argv[2];
let input_exe = process.argv[3];
let output_exe = process.argv[4];

let proj_path = Path.resolve(dir, "app.evb")
let input_path = Path.resolve(dir, input_exe);
let output_path = Path.resolve(dir, output_exe);

console.log("Generating EVB project file...");

generateEvb(proj_path, input_path, output_path, dir, {
    filter: (fullPath, name, isDir) => name !== input_exe
});

console.log("Creating executable bundle...")

await evb.cli(proj_path);

console.log("Success!")
