# Cleaner

## Small program to test out C#

Lists possible compilation artifacts from c/c++ (a.out, *.o and linux executables *.) and deletes them upon request.<br>
Currently program is usable, but somewhat dangerous.
Flag [-a] searches all subdirectories of current folder.<br>
Flag [-c] allows for a custom pattern to be inputed for search.<br>
<br>
![Screenshot from 2022-11-21 09-50-43](https://user-images.githubusercontent.com/71012028/203006402-e0fb4e07-6d34-4e0a-a696-b8069ef01754.png)
<br>

## To Do

- Change initial pattern from "*." to searching for executable.
- Provide finer control on what is deleted (currently cleaner only asks for permission if itself or a Makefile will be deleted).
