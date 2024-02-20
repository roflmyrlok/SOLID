3 test files with txt, csv and json data present
random.txt
random.csv
random.json


Commands Implemented: 


file add <path_to_file> <shortcut>  
adding a file to system with a name specified as "shortcut". If no shortcut is provided, use full filename (including a path) as a name;


file remove <shortcut>
if no file with shortcut found, it will look for file with such a pass


file list
provides all added files to system


file options <shortcut> 
provides fullPath, fileSize, and extension of a file, provides avaliable list of additional commands for this file

file summary <shortcut>
for .txt files, showing basic information about text (number of symbols, words, paragraphs);

file print <shortcut>
for an any .csv file, printing nicely formatted table to a screen;
for an any .json file, printing correctly indented json on a screen;

file validate <shortcut> 
for a .csv and .json files checks if they have valid data acording to fomat



