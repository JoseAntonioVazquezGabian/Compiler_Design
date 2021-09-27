# Drac compiler, version 1.0.0

This program is free software. You may redistribute it under the terms of the GNU General Public License version 3 or later. See `license.txt` for details.

Included in this release:

* Lexical analysis

## How to Build

At the terminal type:

    make

## How to Run

At the terminal type:

    mono drac.exe <drac_source_file>

Where `<drac_source_file>` is the name of a Buttercup source file. You can try with these files:

* `001_hello.drac`
* `002_binary.drac`
* `003_palindrome.drac`
* `004_factorial.drac`
* `005_arrays.drac`
* `006_next_day.drac`
* `007_literals.drac`
* `008_vars.drac`
* `009_operators.drac`
* `010_breaks.drac`

## How to Clean

To delete all the files that get created automatically, at the terminal type:

    make clean