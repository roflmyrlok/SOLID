3 test files with txt, csv and json data present. Solution_folder/test_data is used as a default directory for files
```random.txt```
```random.csv```
```random.json```

Commands Implemented: 

```login <account_name>```
if no account with such name registred new account will be added
if there is such account it will be chosen as current account
all file shortcuts are account bound
by default all users have basic plan

```change_plan <plan_name>```
avaliable plans: Gold (100 files, 1G of files allowed), Basic (10 files, 100mb of files allowed)
user is not allowed to downgrade plan if he exceeds desiered plan limits

```add <path_to_file> <shortcut>```
adding a file to system with a name specified as "shortcut". If no shortcut is provided, use full filename (including a path) as a name;

```remove <shortcut>```
if no file with shortcut found, it will look for file with such a pass

```list```
provides all added files to system

```info <shortcut>```
provides fullPath, fileSize, and extension of a file, provides avaliable list of additional commands for this file

```summary <shortcut>```
for .txt files, showing basic information about text (number of symbols, words, paragraphs);

```print <shortcut>```
for an any .csv file, printing nicely formatted table to a screen;
for an any .json file, printing correctly indented json on a screen;

```validate <shortcut>```
for a .csv and .json files checks if they have valid data acording to fomat





