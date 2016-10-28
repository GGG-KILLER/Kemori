# README #

Kemori is a manga downloader that I made after using [Hakuneko](https://sourceforge.net/projects/hakuneko/) for a long time (the interface was heavily influenced by it as I couldn't think of anything better than it).

### Why should I use this? ###
* Pluggable connectors (no need for others to compile the whole program or the connector)
* Portability of .NET framework: 1 binary for both Linux (Mono) and Windows (.NET)
* Hability to get past CloudFlare's challenge page (meaning more connectors for more websites)

### Why not contribute to Hakuneko instead? ###
1. I don't know a bit about C++ (apart from the basics)
2. I have no idea how to compile it
3. My specialty is C#
4. Implementing pluggable .dll's in C++ would probably be too hard and break all previous versions of Hakuneko.

### How do I get set up? ###
* Visual Studio Community 2015 (with NuGet)
* .NET 4.6
* [Jint](https://github.com/sebastienros/jint) (Automatically restored by NuGet)

### How do I install? ###
Currently there are no **official** installers, therefore you'll have to download the binary from the Downloads/Releases section and run the binary directly.

### Security Warnings ###
* **Don't use connectors that aren't open-source**
* Don't download connectors from other places that are not their oficial repositories
* Always analyze the connector's code for malicious code (not many ways to prevent this from Kemori that I know of, but you're always free to contribute if you're feeling generous)

### Contribution guidelines ###
* Mantain the code style already being used in the project (including spacing).
* Mantain (or increment) the Object Orientation Programming used in the project. Try to make everything object oriented or at least try not to add something that isn't OO (unless it's not possible) or transform something that is OO to non-OO.
* Only use english
* Comment (documentation comments such as <summary> in functions and classes) **everything**.

### Who do I talk to? ###
* [GGG KILLER](https://gggkiller.com)